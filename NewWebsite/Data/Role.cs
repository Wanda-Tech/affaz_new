using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
public class Role
{
    [Key]
    public int RoleId { get; set; }

    [MaxLength(10)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }
    
    public List<UserRole> UserRoles { get; set; }
    
}