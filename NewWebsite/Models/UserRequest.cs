
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewWebsite.Models;

public class UserRequest
{
    public int? UserId { get; set; }

    [MaxLength(10)]
    public string Phone { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public List<int>? ExistingRoles { get; set; } = new List<int>();
    
    public int[]? SelectedRoleIds { get; set; }

    public Dictionary<int, string>? RolesList { get; set; }

    public bool IsUpdating => UserId.HasValue && UserId > 0;
}