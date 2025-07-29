using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class NewsLike
{
    [Key]
    public int Id { get; set; }

    public int NewsId { get; set; }
    public News News { get; set; }

    // public int UserId { get; set; }
    // public User User { get; set; }

    public DateTime? CreatedDate { get; set; }

    [StringLength(255)]
    public string? UserAgent { get; set; }

    [StringLength(50)]
    public string? IpAddress { get; set; }

}