namespace HotelManagement_MVC.Models
{
    public class ExperienceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Experience> Experiences { get; set; }
    }
}
