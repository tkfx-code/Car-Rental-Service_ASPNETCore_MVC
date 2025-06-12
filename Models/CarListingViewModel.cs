using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Models
{
    public class CarListingViewModel
    {
        [Key]
        public int CarId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalPrice { get; set; } 
        public bool isAvailable { get; set; } = true;
    }
}
