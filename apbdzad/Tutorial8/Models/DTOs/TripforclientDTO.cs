using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class TripforclientDTO
{
    [Required]
    public int idTrip{get;set;}
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateFrom { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateTo { get; set; }
    [Required]
    public int MaxPeople { get; set; }
    [Required]
    public int RegisteredAt { get; set; }
    public int PaymentDate { get; set; }
}