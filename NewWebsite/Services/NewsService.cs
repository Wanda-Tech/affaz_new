
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewWebsite.Extension;
using NewWebsite.Models;

namespace NewWebsite.Services;

public class NewsService : INewsService
{
    private readonly NewsWebsiteContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountService _accountService;

    public NewsService(NewsWebsiteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountService accountService)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _accountService = accountService;
    }

    public async Task<PaginatedResponse<SimpleNews>> GetAllNewsAsync(NewsSearchRequest request)
    {
        IQueryable<News> query = _context.News
            .Include(n => n.User)
            .Include(n => n.NewsCategory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query
                .Where(q => EF.Functions.Like(q.Title, $"%{request.Search}%") ||
                q.User.Email == request.Search);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(q => q.NewsStatus == request.Status);
        }

        int totalCount = await query.CountAsync();

        List<News> news = await query
            .OrderByDescending(q => q.CreatedDate)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();

        List<SimpleNews> simpleNews = _mapper.Map<List<SimpleNews>>(news);

        var response = new PaginatedResponse<SimpleNews>(simpleNews, request, totalCount);

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

    public async Task<List<SimpleNews>> GetRecentNews(int limit = 10)
    {
        DateTime daysBefore = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));

        List<News> news = await _context.News
            .Include(n => n.User)
            .Include(n => n.NewsCategory)
            .Where(q => q.CreatedDate >= daysBefore)
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


    public async Task<News> CreateOrUpdateAsync(NewsRequest request)
    {
        if (request.IsUpdating)
        {
            News? existingNews = await _context.News.FindAsync(request.NewsId);

            if (existingNews == null)
            {
                throw new Exception("Existing news does not exist");
            }

            existingNews.Title = request.Title;
            existingNews.Summary = request.Summary;
            existingNews.Content = request.Content;
            existingNews.NewsCategoryId = request.NewsCategoryId;
            existingNews.NewsStatus = request.NewsStatus;

            await _context.SaveChangesAsync();

            return existingNews;
        }
        else
        {
            News newsToCreate = _mapper.Map<News>(request);

            newsToCreate.UserId = _accountService.GetCurrentUserId();

            _context.News.Add(newsToCreate);

            await _context.SaveChangesAsync();

            return newsToCreate;
        }
    }

    public async Task<SelectList> GetNewsCategorySelectListAsync(int? selectedId = null)
    {
        var categories = await _context.NewsCategories.ToDictionaryAsync(q => q.Id, q => q.Name);

        return new SelectList(categories, "Key", "Value", selectedId);
    }

    public async Task ChangeNewsAsync(ChangeNewsRequest request)
    {
        if (request.Delete != 1 && request.Publish != 1)
        {
            throw new ApplicationException($"Request is invalid");
        }
        
        News? news = await _context.News.FindAsync(request.News.NewsId);

        if (news == null)
        {
            throw new ApplicationException($"News {request.News.NewsId} does not exist");
        }

        if (request.Delete == 1)
        {
            _context.News.Remove(news);
        }
        else if (request.Publish == 1)
        {
            news.PublishedDate = DateTime.UtcNow;

            news.NewsStatus = NewsStatus.Published;

            _context.News.Update(news);
        }
        else
        {
            throw new ApplicationException("Request is Invalid");
        }
        

        await _context.SaveChangesAsync();
    }
}