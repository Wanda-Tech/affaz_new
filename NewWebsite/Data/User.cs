using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(10)]
    public string Phone { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    [MaxLength(150)]
    public string Password { get; set; }

    public DateTime CreatedDate { get; set; }

    public List<News> NewsList { get; set; } = new List<News>();
    
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
}