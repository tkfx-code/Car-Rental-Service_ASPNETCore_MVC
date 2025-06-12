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
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Phone Number is required")]
        [MinLength(10, ErrorMessage = "Phone Number must be at least 10 digits")]
        public int PhoneNumber { get; set; }

        //Create virtual list for bookings that can store customer info 
        public virtual List<CustomerViewModel> Customers { get; set; }
    }
}
