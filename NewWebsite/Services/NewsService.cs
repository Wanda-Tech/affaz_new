
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Models;

namespace NewWebsite.Services;

public class NewsService : INewsService
{
    private readonly NewsWebsiteContext _context;
    private readonly IMapper _mapper;

    public NewsService(NewsWebsiteContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SimpleNews>> GetAllNewsAsync()
    {
        List<News> news = await _context.News
            .Include(n => n.User)
            .Include(n => n.NewsCategory)
            .ToListAsync();

        List<SimpleNews> response = _mapper.Map<List<SimpleNews>>(news);

        return response;
    }
    
    
    public async Task<List<SimpleNews>> GetRandomNewsListAsync(int limit = 5)
    {
        List<News> news = await _context.News
            .Include(n => n.User)
            .Include(n => n.NewsCategory)
            .OrderBy(n => Guid.NewGuid()) // Random order
            .Take(limit)
            .ToListAsync();

        List<SimpleNews> response = _mapper.Map<List<SimpleNews>>(news);

        return response;
    }

    public async Task<SimpleNews> GetNewsByIdAsync(int id)
    {
        News? news = await _context.News
            .Include(n => n.User)
            .Include(n => n.NewsCategory)
            .FirstOrDefaultAsync(n => n.NewsId == id);

        ArgumentNullException.ThrowIfNull(news, $"News not found for id {id}");

        SimpleNews response = _mapper.Map<SimpleNews>(news);

        return response;
    }
}