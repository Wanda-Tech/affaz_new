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


    public async Task<IActionResult> News(NewsSearchRequest request)
    {
        ViewBag.Search = request.Search;
        ViewBag.Filter = (int?)request.Status;

        PaginatedResponse<SimpleNews> newsList = await _newsService.GetAllNewsAsync(request);

        return View(newsList);
    }


    public async Task<IActionResult> CreateNews()
    {
        NewsRequest request = new NewsRequest();

        request.NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync();

        return View(request);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNews(NewsRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                News news = await _newsService.CreateOrUpdateAsync(request);

                string message = $"News {news.NewsId} was created successfully";

                if (request.IsUpdating)
                {
                    message = "News was just updated";
                }

                SetTempMessage(message);

                return RedirectToAction(nameof(News));
            }
        }
        catch (System.Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        
        request.NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync(request.NewsCategoryId);

        return View(request);
    }

    public async Task<IActionResult> EditNews(int id)
    {
        SimpleNews news = await _newsService.GetNewsByIdAsync(id);
        
        NewsRequest request = new NewsRequest(news);

        request.NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync(request.NewsCategoryId);

        return View("CreateNews", request);
    }



    public async Task<IActionResult> ChangeNews(int id, int? delete = null, int? publish = null)
    {
        SimpleNews news = await _newsService.GetNewsByIdAsync(id);

        ChangeNewsRequest request = new ChangeNewsRequest()
        {
            News = news,
            Delete = delete,
            Publish = publish
        };
        
        return View(request);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeNews(ChangeNewsRequest request)
    {
        try
        {
            await _newsService.ChangeNewsAsync(request);

            string message = $"News {request.News.NewsId} was created successfully " + (request.Delete == 1 ? "Deleted" : "Published");

            SetTempMessage(message);

            return RedirectToAction(nameof(News));
        }
        catch (System.Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(request);
    }


    public async Task<IActionResult> Users(SearchRequest request)
    {
        ViewBag.Search = request.Search;
        // ViewBag.Filter = (int?)request.Status;

        PaginatedResponse<User> newsList = await _accountService.GetAllUsersAsync(request);

        return View(newsList);
    }


    public async Task<IActionResult> CreateUser()
    {
        UserRequest request = await _accountService.GetUserByIdForEditAsync();
        
        return View("EditUser", request);
    }
    
    public async Task<IActionResult> EditUser(int id)
    {
        UserRequest request = await _accountService.GetUserByIdForEditAsync(id);
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                User user = await _accountService.CreateOrUpdateAsync(request);

                string message = $"User {user.UserId} was created successfully, New password is '{user.Password}'";

                if (request.IsUpdating)
                {
                    message = $"User {user.Email} was just updated";
                }

                SetTempMessage(message);

                return RedirectToAction(nameof(Users));
            }
        }
        catch (System.Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        
        request = await _accountService.GetUserByIdForEditAsync(request.UserId);
        
        return View(request);
    }

    
    
    private void SetTempMessage(string message)
    {
        TempData["Tmp.Message"] = message;
    }
}
