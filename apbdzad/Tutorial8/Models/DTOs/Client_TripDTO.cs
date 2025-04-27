using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class Client_TripDTO
{
    [Required]
    int IdClient{ get; set; }
    [Required]
    int IdTrip{ get; set; }
    [Required]
    int RegisteredAt{ get; set; }
    int PaymentDate{ get; set; }
}