using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
public class News
{
    [Key]
    public int NewsId { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    public string Content { get; set; }
    public string? Summary { get; set; }

    public DateTime CreatedDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }


    public NewsStatus NewsStatus { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? ImageUrl { get; set; }

    public int NewsCategoryId { get; set; }
    public NewsCategory NewsCategory { get; set; }

}