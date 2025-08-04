
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public interface INewsService
{
    public Task<PaginatedResponse<SimpleNews>> GetAllNewsAsync(NewsSearchRequest request);
    public Task<SimpleNews> GetNewsByIdAsync(int id);
    public Task<List<SimpleNews>> GetRandomNewsListAsync(int limit = 5);
    public Task<List<SimpleNews>> GetRecentNews(int limit = 10);
    public Task<int> UpdateLikesAsync(int newsId);

    public Task<News> CreateOrUpdateAsync(NewsRequest request);
    public Task<SelectList> GetNewsCategorySelectListAsync(int? selectedId = null);
}