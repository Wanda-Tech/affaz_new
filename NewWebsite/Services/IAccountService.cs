
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public interface IAccountService
{
    public Task<User> AuthenticateAsync(SignInRequest request);
    public Task<User> RegisterNewUser(SignUpRequest request);

    public Task SignOutAsync();

    public Task<List<User>> GetRecentUsers(int limit = 10);
    public Task<PaginatedResponse<User>> GetAllUsersAsync(SearchRequest request);
    public Task<UserRequest> GetUserByIdForEditAsync(int? id = null);
    public Task<User> CreateOrUpdateAsync(UserRequest request);
    public int GetCurrentUserId();
}