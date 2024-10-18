using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrybeHotel.Models;

// 1. Adicione o atributo State na model City
public class City {
    [Key]
    public int CityId { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public ICollection<Hotel>? Hotels { get; set; }
}