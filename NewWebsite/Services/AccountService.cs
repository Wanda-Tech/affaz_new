
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
}