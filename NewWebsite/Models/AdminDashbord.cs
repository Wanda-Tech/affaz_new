namespace NewWebsite.Models;

public class AdminDashbord
{
    public List<SimpleNews> RecentNews { get; set; }
    public List<User> RecentUsers { get; set; }
    public Dictionary<int, int> NewsVisitsByDay { get; set; }
}
