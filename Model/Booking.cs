using MVC_Project.Models;

namespace MVC_Project.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Customer? Customer { get; set; }
        public CarListing? Car { get; set; }


        //No list since bookings are always one-to-one with a customer and car
        //Automatically set starting date as the day of booking and end date 14 days later
        public Booking()
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = StartDate.AddDays(14);
        }
    }
}
