namespace NewWebsite.Models;

public class ChangeNewsRequest
{
    public int? Delete { get; set; }
    public int? Publish { get; set; }
    
    public SimpleNews? News { get; set; }
}
