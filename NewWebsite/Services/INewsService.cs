
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public interface INewsService
{
    public Task<PaginatedResponse<SimpleNews>> GetAllNewsAsync(SearchRequest request);
    public Task<SimpleNews> GetNewsByIdAsync(int id);
    public Task<List<SimpleNews>> GetRandomNewsListAsync(int limit = 5);
    public Task<List<SimpleNews>> GetRecentNews(int limit = 10);
    public Task<int> UpdateLikesAsync(int newsId);
}