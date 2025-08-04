
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewWebsite.Models;

public class NewsRequest
{
    public int? NewsId { get; set; }


    [MaxLength(100)]
    public string Title { get; set; }

    public string Content { get; set; }
    public string? Summary { get; set; }

    public NewsStatus NewsStatus { get; set; }
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Please choose a News Category?")]
    public int NewsCategoryId { get; set; }


    public bool IsUpdating => NewsId.HasValue && NewsId > 0;

    public SelectList? NewsCategorySelectList { get; set; }
    
    public NewsRequest()
    {
        
    }
    
    public NewsRequest(SimpleNews news)
    {
        NewsId = news.NewsId;
        Content = news.Content;
        Title = news.Title;
        Summary = news.Summary;
        NewsCategoryId = news.NewsCategoryId;
    }
}