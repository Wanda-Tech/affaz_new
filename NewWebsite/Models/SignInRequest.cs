
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;

public class SignInRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be between 3 and 100 characters.", MinimumLength = 3)]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
}