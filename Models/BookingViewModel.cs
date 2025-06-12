using System.ComponentModel.DataAnnotations;
using MVC_Project.Model;
namespace MVC_Project.Models
{
    public class BookingViewModel
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        //Booking is automatically 14 days
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(14);
        public double TotalPrice { get; set; }
        // Navigation properties
        public Customer? Customer { get; set; }
        public CarListing? Car { get; set; }

    }
}
