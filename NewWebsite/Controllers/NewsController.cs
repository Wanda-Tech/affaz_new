using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewWebsite.Models;
using NewWebsite.Services;

namespace NewWebsite.Controllers;

public class NewsController : Controller
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _newsService;

    public NewsController(ILogger<NewsController> logger, INewsService newsService)
    {
        _logger = logger;
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        List<SimpleNews> newsList = await _newsService.GetAllNewsAsync(); 
        
        return View(newsList);
    }

    // GET: News/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        SimpleNews? news = await _newsService.GetNewsByIdAsync(id);
        
        if (news == null)
        {
            return NotFound();
        }
        
        NewsDetailResponse response = new NewsDetailResponse
        {
            News = news,
            ReadNextNews = await _newsService.GetRandomNewsListAsync(3),
        };

        return View(response);
    }

}
