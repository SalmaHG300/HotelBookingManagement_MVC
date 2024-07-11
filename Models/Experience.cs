using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public string CoverImage { get; set; }

        [ForeignKey("ExperienceType")]
        public int TypeId { get; set; } // Foreign key for Type
        public ExperienceType Type { get; set; } // Navigation property for Type
        public string Description { get; set; }
        public string instructions { get; set; }
        public string Requirements { get; set; }
        public int Price { get; set; }
        public string Duration { get; set; }
        public int? NumAdults { get; set; }
        public List<BookingExperience> BookingExperiences { get; set; }
    }
}
