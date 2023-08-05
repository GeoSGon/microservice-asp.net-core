using System.ComponentModel.DataAnnotations;

namespace userService.Api.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [EmailAddress(ErrorMessage = "Email invalid.")]
    [MaxLength(60, ErrorMessage = "This field must be between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must be between 3 and 60 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(60, ErrorMessage = "This field must be between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must be between 3 and 60 characters.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirmation Password is required.")]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    public string? ConfirmPassword { get; set; }

    public string? Role { get; set; }
}
