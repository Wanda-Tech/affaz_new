
using System.ComponentModel.DataAnnotations;

namespace NewWebsite.Models;

public class SignUpRequest
{

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be between 3 and 100 characters.", MinimumLength = 3)]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [DataType(DataType.PhoneNumber)]
    public required string Phone { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    
    
    public required bool IsAgreedTerms { get; set; }
    
}