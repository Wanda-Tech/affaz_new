
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public interface INewsService
{
    public Task<List<SimpleNews>> GetAllNewsAsync();
    public Task<SimpleNews> GetNewsByIdAsync(int id);
    public Task<List<SimpleNews>> GetRandomNewsListAsync(int limit = 5);

    public Task<int> UpdateLikesAsync(int newsId);
}