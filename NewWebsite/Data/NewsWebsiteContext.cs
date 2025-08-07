using Microsoft.EntityFrameworkCore;

public class NewsWebsiteContext : DbContext
{
    public NewsWebsiteContext(DbContextOptions<NewsWebsiteContext> options) : base(options)
    {
    }


    public DbSet<News> News { get; set; }
    public DbSet<NewsCategory> NewsCategories { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<NewsLike> NewsLikes { get; set; }
}