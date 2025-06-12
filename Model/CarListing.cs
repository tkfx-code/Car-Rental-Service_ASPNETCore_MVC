using System.ComponentModel.DataAnnotations;
using MVC_Project.Models;

namespace MVC_Project.Model
{
    public class CarListing
    {
        [Key]
        public int CarId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public bool isAvailable { get; set; } = true;

        public List<CarListingViewModel> CarListings { get; set; }
    }
}
