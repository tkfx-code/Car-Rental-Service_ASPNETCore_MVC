using MVC_Project.Models;

namespace MVC_Project.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;

        // Booking is automatically 14 days  
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(14);

        public Customer? Customer { get; set; }
        public CarListing? Car { get; set; }

        public List<BookingViewModel> Bookings { get; set; }
    }
}
