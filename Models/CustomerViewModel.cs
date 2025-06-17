using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Models
{
    public class CustomerViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        //setting phonenumber to string since int would not allow leading zeros
        //Which will make ModelState be invalid and return fals, and model binding to silently fail.
        public string PhoneNumber { get; set; } = string.Empty;

        public string? UserId { get; set; } //Foreign key to IdentityUser

    }
}
