using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class TripDTO
{   
    [Required]
    public int Id { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    [Required]
    public List<CountryDTO> Countries { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string Description {get; set;}
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateFrom {get; set;}
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DateTo {get; set;}
    [Required]
    public int MaxPeople {get; set;}
}

public class CountryDTO
{
    public string Name { get; set; }
}