using System.Collections.Generic;

namespace MVC_Project.Models
{
    public class ProfileViewModel
    {
        public CustomerViewModel Customer { get; set; } = new CustomerViewModel();
        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
    }
}
