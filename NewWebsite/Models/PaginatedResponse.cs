
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;


public interface IPaginatedResponse
{
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    public int TotalRecordsCount { get; set; }
}


public class PaginatedResponse<T> : IPaginatedResponse
{
    public int CurrentPage { get; set; } = 1;
    public int LastPage { get; set; }
    public int TotalRecordsCount { get; set; }

    public List<T> Response { get; set; }

    public PaginatedResponse(List<T> records, SearchRequest request, int totalItemsCount)
    {
        Response = records;
        TotalRecordsCount = totalItemsCount;
        CurrentPage = request.Page;

        decimal pagesCount = TotalRecordsCount / request.Limit;
        LastPage = int.Parse(Math.Ceiling(pagesCount).ToString());
    }
}
