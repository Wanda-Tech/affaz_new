using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewWebsite.Models;
using NewWebsite.Services;

namespace NewWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INewsService _newsService;

    public HomeController(ILogger<HomeController> logger, INewsService newsService)
    {
        _logger = logger;
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        HomePageResponse response = await _newsService.GetHomePageAsync();
        
        return View(response);
    }
    
    
    [Route("/news/{category}")]
    public async Task<IActionResult> News(string category)
    {
        NewsSearchRequest request = new NewsSearchRequest();
        request.CategorySlug = category;

        ViewBag.header = category + " News";
        
        PaginatedResponse<SimpleNews> response = await _newsService.GetAllNewsAsync(request);
        
        return View(response);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
