using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
public class NewsCategory
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }


    public List<News> NewsList { get; set; }

}