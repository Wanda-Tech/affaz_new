
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Extension;
using NewWebsite.Helpers;
using NewWebsite.Models;

namespace NewWebsite.Services;

public class AccountService : IAccountService
{
    private readonly NewsWebsiteContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string AUTH_SCHEME = CookieAuthenticationDefaults.AuthenticationScheme;

    public AccountService(NewsWebsiteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> AuthenticateAsync(SignInRequest request)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new ApplicationException("HttpContext is null");
        }

        User? user = await _context.Users.FirstOrDefaultAsync(q => q.Email == request.Username || q.Phone == request.Username);

        if (user == null)
        {
            throw new ApplicationException("Invalid phone or email address");
        }

        bool checkPassword = PasswordHasher.VerifyPassword(request.Password, user.Password);

        if (checkPassword == false)
        {
            throw new ApplicationException("Invalid username and password");
        }


        // login 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email)
        };

        string[] roles = await _context.UserRoles
            .Where(q => q.UserId == user.UserId)
            .Select(q => q.Role.Name)
            .ToArrayAsync();

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var identity = new ClaimsIdentity(claims, AUTH_SCHEME);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(AUTH_SCHEME, principal);

        return user;
    }


    public async Task<User> RegisterNewUser(SignUpRequest request)
    {
        bool hasExistingUser = await _context.Users.AnyAsync(
            q => q.Email == request.Email && q.Phone == request.Phone);

        if (hasExistingUser)
        {
            throw new ApplicationException($"User with this phone and email already exists, {request.Phone}, {request.Email}");
        }

        User user = _mapper.Map<User>(request);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task SignOutAsync()
    {
        if (_httpContextAccessor.HttpContext == null)
            return;


        await _httpContextAccessor.HttpContext.SignOutAsync(AUTH_SCHEME);
    }


    public async Task<List<User>> GetRecentUsers(int limit = 10)
    {
        DateTime daysBefore = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));

        var users = await _context.Users
            .Include(q => q.UserRoles)
                .ThenInclude(q => q.Role)
            .Where(q => q.CreatedDate >= daysBefore)
            .ToListAsync();

        return users;
    }
    
    
    public async Task<PaginatedResponse<User>> GetAllUsersAsync(SearchRequest request)
    {
        IQueryable<User> query = _context.Users
            .Include(n => n.UserRoles)
            .ThenInclude(n => n.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query
                .Where(q => EF.Functions.Like(q.Phone, $"%{request.Search}%") ||
                q.Email == request.Search);
        }

        int totalCount = await query.CountAsync();

        List<User> news = await query
            .OrderByDescending(q => q.CreatedDate)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();

        List<User> simpleNews = _mapper.Map<List<User>>(news);

        var response = new PaginatedResponse<User>(simpleNews, request, totalCount);

        return response;
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        User? user = await _context.Users
            .Include(n => n.UserRoles)
            .ThenInclude(q=> q.Role)
            .FirstOrDefaultAsync(n => n.UserId == id);

        ArgumentNullException.ThrowIfNull(user, $"User not found for id {id}");

        return user;
    }
    
    
    public async Task<UserRequest> GetUserByIdForEditAsync(int? id = null)
    {
        UserRequest request = new UserRequest();
        
        if (id.HasValue)
        {
            User? user = await _context.Users
                .Include(n => n.UserRoles)
                .ThenInclude(q => q.Role)
                .FirstOrDefaultAsync(n => n.UserId == id);

            if (user != null)
            {
                request = _mapper.Map<UserRequest>(user);
            }
        }
        
        request.RolesList = await _context.Roles.ToDictionaryAsync(q => q.RoleId, q => q.Name);
        

        return request;
    }


    public async Task<User> CreateOrUpdateAsync(UserRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        User? user = null;
        string? passwordText = null;

        if (request.IsUpdating)
        {
            User? existingUser = await _context
            .Users
            .Include(q => q.UserRoles)
            .FirstOrDefaultAsync(q => q.UserId == request.UserId);

            if (existingUser == null)
            {
                throw new Exception("Existing user does not exist");
            }

            existingUser.Phone = request.Phone;
            existingUser.Email = request.Email;

            if (existingUser.UserRoles.Any())
            {
                _context.UserRoles.RemoveRange(existingUser.UserRoles);
            }

            await _context.SaveChangesAsync();

            user = existingUser;
        }
        else
        {
            User userToCreate = _mapper.Map<User>(request);

            passwordText = PasswordGenerator.Generate(8);

            userToCreate.Password = PasswordHasher.HashPassword(passwordText);
            

            _context.Users.Add(userToCreate);

            await _context.SaveChangesAsync();

            user = userToCreate;
        }

        if (request.SelectedRoleIds != null)
        {
            foreach (int roleId in request.SelectedRoleIds)
            {
                Role? role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    throw new ApplicationException("Role does not exist");
                }

                user.UserRoles.Add(new UserRole() { RoleId = roleId });
            }
        }

        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        if (!string.IsNullOrWhiteSpace(passwordText))
        {
            user.Password = passwordText;
        }

        return user;
    }


    public int GetCurrentUserId()
    {
        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext);


        if (_httpContextAccessor.HttpContext.User.HasClaim(q => q.Type == ClaimTypes.NameIdentifier))
        {
            string? userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
        }

        throw new Exception("User is not authorized");
    }
}