
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;

public class NewsSearchRequest : SearchRequest
{
    public NewsStatus? Status { get; set; }
}