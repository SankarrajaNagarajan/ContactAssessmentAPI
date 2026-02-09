using System.ComponentModel.DataAnnotations;

namespace ContactApi.Domain.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } 

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } 

    [Required, MaxLength(256)]
    public string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
