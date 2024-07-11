namespace HotelManagement_MVC.Models
{
    public class HotelRoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public int BedCount { get; set; }
        public int BathCount { get; set; }
        public int MaxGuestCount { get; set; }
        public int Price { get; set; }
        public int Area { get; set; }
        public string? BedType { get; set; }
        public string? View { get; set; }
        public string? Decor { get; set; }
        public string? UniqueFeatures { get; set; }
    }
}
