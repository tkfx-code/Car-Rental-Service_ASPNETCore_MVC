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
        [NotMapped]
        public IFormFile? Picture { get; set; }
        public bool isAvailable { get; set; } = true;

        //One to many: One car can get booked several times
        public List<Booking> Bookings { get; set; }
    }
}
