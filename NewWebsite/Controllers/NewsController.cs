using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWebsite.Models;
using NewWebsite.Services;

namespace NewWebsite.Controllers;

[Authorize(Roles = $"{Constants.Roles.Admin},{Constants.Roles.Editor}")]
public class NewsController : Controller
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _newsService;

    public NewsController(ILogger<NewsController> logger, INewsService newsService)
    {
        _logger = logger;
        _newsService = newsService;
    }

    public async Task<IActionResult> Index(SearchRequest request)
    {
        PaginatedResponse<SimpleNews> newsList = await _newsService.GetAllNewsAsync(request);

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

    [HttpPost]
    public async Task<IActionResult> Like(string newsId)
    {
        if (int.TryParse(newsId, out int id))
        {
            int newCounter = await _newsService.UpdateLikesAsync(int.Parse(newsId));

            return Ok(new { totalLikes = newCounter });
        }


        return BadRequest("News ID cannot be null or empty.");
    }

}
