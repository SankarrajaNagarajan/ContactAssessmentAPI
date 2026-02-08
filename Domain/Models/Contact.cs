using System.ComponentModel.DataAnnotations;

namespace ContactApi.Domain.Models;

public class Contact
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [MaxLength(50)]
    public string State { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(10)]
    public string PostalCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? UserId { get; set; }
}
