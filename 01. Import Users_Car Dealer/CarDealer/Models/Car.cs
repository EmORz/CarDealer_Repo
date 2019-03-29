using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public long TravelledDistance { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public ICollection<PartCar> PartCars { get; set; } = new List<PartCar>();
    }
}