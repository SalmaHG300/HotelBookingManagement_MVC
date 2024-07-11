using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.ViewModel
{
    public class ExperienceWithTypesViewModel
    {
        public int ExperienceId { get; set; }
        public string ExperienceName { get; set; }
        public string? Image { get; set; }
        public string? CoverImage { get; set; }
        public List<ExperienceType> ? Type { get; set; } 
        public string Description { get; set; }
        public int Price { get; set; }
        public string Duration { get; set; }
        public string instructions { get; set; }
        public string Requirements { get; set; }
        public int TypeId { get; set; }

    }
}
