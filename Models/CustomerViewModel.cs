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
        public int PhoneNumber { get; set; }

        public string? UserId { get; set; } //Foreign key to IdentityUser

    }
}
