using System.ComponentModel.DataAnnotations;
namespace MVC_Project.Models
{
    public class BookingViewModel
    {
        [Key]
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CustomerViewModel? Customer { get; set; }
        public CarListingViewModel? Car { get; set; }

        public BookingViewModel()
        {
            StartDate = DateOnly.FromDateTime(DateTime.Today);
            EndDate = StartDate.AddDays(14);
        }
    }
}
