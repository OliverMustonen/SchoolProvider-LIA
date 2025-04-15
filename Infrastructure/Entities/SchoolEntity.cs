using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class SchoolEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Address { get; set; } = null!;
    [Required]
    public string PostalCode { get; set; } = null!;
    [Required]
    public string City { get; set; } = null!;
    [Required]
    public string Country { get; set; } = null!;
    public DateTime Created { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImagePath { get; set; }

    
}



