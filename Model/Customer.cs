using System.ComponentModel.DataAnnotations;
using MVC_Project.Models;

namespace MVC_Project.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        //setting phonenumber to string since int would not allow leading zeros
        //Which will make ModelState be invalid and return fals, and model binding to silently fail.
        public string PhoneNumber { get; set; } = string.Empty;
        public string? UserId { get; set; } //Foreign key to IdentityUser

        //One to many : One customer can have many bookings
        public virtual List<Booking> Bookings { get; set; }
    }
}
