namespace NewWebsite.Models;

public class NewsDetailResponse
{
    public SimpleNews News { get; set; }
    
    public List<SimpleNews> ReadNextNews { get; set; }

}
