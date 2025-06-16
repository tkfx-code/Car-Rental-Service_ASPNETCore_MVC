namespace MVC_Project.Models
{
    public class AdminViewModel
    {
        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();

        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();
    }
}
