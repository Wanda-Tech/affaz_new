public static class DbInitializer
{
    public static void Seed(NewsWebsiteContext context)
    {
        if (!context.NewsCategories.Any())
        {
            context.NewsCategories.AddRange(
                new NewsCategory { Name = "Articles" },
                new NewsCategory { Name = "Reports" },
                new NewsCategory { Name = "Breaking" }
            );
            context.SaveChanges();
        }
        
        
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Editor" }
            );
            context.SaveChanges();
        }
    }
}