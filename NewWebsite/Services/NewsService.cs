
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Extension;
using NewWebsite.Models;

namespace NewWebsite.Services;

public class NewsService : INewsService
{
    private readonly NewsWebsiteContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NewsService(NewsWebsiteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<int> UpdateLikesAsync(int newsId)
    {
        var news = await _context.News.FindAsync(newsId);
        
        
        NewsLike newsLike = new NewsLike
        {
            NewsId = newsId,
            CreatedDate = DateTime.UtcNow,
            UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown",
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
        };

        news.TotalLikes += 1;

        news.Likes.Add(newsLike);
        
        
        await _context.SaveChangesAsync();

        return news.TotalLikes;
    }
}