
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public interface IAccountService
{
    public Task<User> AuthenticateAsync(SignInRequest request);
    public Task<User> RegisterNewUser(SignUpRequest request);
    
    public Task SignOutAsync();
}