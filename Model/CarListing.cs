using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Project.Model
{
    public class CarListing
    {
        [Key]
        public int CarId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        [Required]
        public List<string> Pictures { get; set; } = new List<string>();
        public bool isAvailable { get; set; } = true;

        //One to many: One car can get booked several times
        public List<Booking> Bookings { get; set; }
    }
}
