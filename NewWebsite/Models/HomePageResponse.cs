namespace NewWebsite.Models;

public class HomePageResponse
{
    public SimpleNews TopNews { get; set; }

    /// <summary>
    /// 1 x 1 News (2)
    /// </summary>
    public List<SimpleNews> SecondNews { get; set; }

    /// <summary>
    /// List is of all the recent news
    /// </summary>
    public List<SimpleNews> RecentNews { get; set; }
    
    
    public Dictionary<string, List<SimpleNews>> CategoryNewsList { get; set; }
}
