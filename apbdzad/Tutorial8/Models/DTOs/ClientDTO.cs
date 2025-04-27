using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class ClientDTO
{
    [Required]
    public int IdClient { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string FirstName { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Telephone { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string Pesel { get; set; }
}