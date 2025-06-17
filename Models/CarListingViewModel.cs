using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Models
{
    public class CarListingViewModel
    {
        [Key]
        public int CarId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        //raw input from create multiline text
        public string PicturesRaw { get; set; }
        //not bound from form but used internally
        public List<string> Pictures { get; set; } = new List<string>();
        public bool isAvailable { get; set; } = true;
    }
}
