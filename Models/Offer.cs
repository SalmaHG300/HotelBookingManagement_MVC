namespace HotelManagement_MVC.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public string OfferDescription { get; set; }
        public string? OfferImage { get; set; }
        public int OfferPrice { get; set; }

    }
}
