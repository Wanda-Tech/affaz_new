using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWebsite.Models;
using NewWebsite.Services;

namespace NewWebsite.Controllers;

[Authorize(Roles = $"{Constants.Roles.Admin}")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly INewsService _newsService;
    private readonly IAccountService _accountService;

    public AdminController(ILogger<AdminController> logger, INewsService newsService, IAccountService accountService)
    {
        _logger = logger;
        _newsService = newsService;
        _accountService = accountService;
    }

    public async Task<IActionResult> Index()
    {
        AdminDashbord adminDashbord = new AdminDashbord();
        adminDashbord.RecentNews = await _newsService.GetRecentNews();

        adminDashbord.RecentUsers = await _accountService.GetRecentUsers();

        return View(adminDashbord);
    }
    
    
    public async Task<IActionResult> News(SearchRequest request)
    {
        ViewBag.Search = request.Search;

        PaginatedResponse<SimpleNews> newsList = await _newsService.GetAllNewsAsync(request);
        
        return View(newsList);
    }
}
