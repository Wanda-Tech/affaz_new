
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;

public class NewsSearchRequest : SearchRequest
{
    public NewsStatus? Status { get; set; }
    public string? CategorySlug { get; set; }
}