using System.ComponentModel.DataAnnotations;

namespace ContactApi.Domain.Models;

public class Contact
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(200)]
    public string Address { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(50)]
    public string State { get; set; }

    [MaxLength(50)]
    public string Country { get; set; } 

    [MaxLength(10)]
    public string PostalCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? UserId { get; set; }
}
