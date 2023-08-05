using System.ComponentModel.DataAnnotations.Schema;

namespace userService.Domain.Entities;

[Table("Users")]
public class User
{
    [Column("Id")]  
    public int Id { get; private set; }

    [Column("Username")]  
    public string? Username { get; private set; }

    [Column("Password")]  
    public string? Password { get; private set; }

    [Column("ConfirmPassword")]
    public string? ConfirmPassword { get; private set; }

    [Column("Role")]  
    public string? Role { get; private set; }
}