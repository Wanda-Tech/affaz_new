
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;

public class SearchRequest
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 6;
}