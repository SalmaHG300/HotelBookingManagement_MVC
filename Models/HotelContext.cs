using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelManagement_MVC.Models
{

    public class HotelContext :IdentityDbContext<ApplicationUser>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BookingDining> BookingDinings { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<Dining> Dinings { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ExperienceType> ExperienceTypes { get; set; }
        public DbSet<BookingExperience> BookingExperiences { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelFloor> HotelFloors { get; set; }
        public DbSet<HotelReview> HotelReviews { get; set; }
        public DbSet<HotelRoomType> HotelRoomTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public HotelContext(DbContextOptions<HotelContext> options)
          : base(options)  { }//inject ==>IOC resolve

        //fill some data in table flut api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotelRoomType>().HasData(new HotelRoomType
            {
                Id =6,
                Name = "Premium King Room",
                Description = "Experience luxury and comfort in our Premium King Room, a corner room offering unparalleled views and deluxe amenities. Relax in a spacious environment featuring one King-size bed adorned with Frette Linens, a separate sitting area, and floor-to-ceiling windows showcasing breathtaking vistas. Stay productive with a full-size work desk and stay entertained with a large screen HDTV equipped with Google Chromecast. Pamper yourself in the luxurious marble shower and indulge in C.O Bigelow bath amenities. Perfect for discerning travelers seeking the ultimate in sophistication and relaxation.",
                Images = "PremiumKingRoom.jpg",
                BedCount = 1,
                BathCount = 1,
                MaxGuestCount = 2,
                Price = 6000,
                Area = 250,
                BedType = "Panel bed with plush Frette Linens",
                View = "Spectacular sea view from floor-to-ceiling windows",
                Decor = "Modern decor with elegant touches",
                UniqueFeatures = "Spacious corner room with abundant natural light, High-end furnishings and decor, Marble bathroom with luxurious jacuzzi bathtub, Complimentary high-speed Wi-Fi, In-room dining available 24/7, Personalized concierge service for all your needs."
            });
            modelBuilder.Entity<Dining>().HasData(new Dining()
            {
                Id = 1,
                Name = "ONCE IN A BLUE MOON",
                Description = "Picture an elegant table for two, sheltered by palm trees and serenaded by gentle ocean waves. Our team of chefs will prepare a special menu, paired with fine wines and served by your own private attendant. Toes in the sand, glimmering candles and tiki torches make for an incredibly romantic evening.",
                Images = "KOH_1450_original.jpg",
                Duration = "3 HOURS",
                Price = 5000,
                TimeOfDay = "EVENING"
            }, new Dining(){
                Id = 2,
                Name = "VIRADOR EXPERIENCE",
                Description = "Framed by towering palm trees, pristine white sand and the gentle waves of the Pacific Ocean, linger over creative cocktails while the sun sinks gently into the ocean. Then dine barefoot with the sand between your toes, at your very own private table on the beach.",
                Images = "COS_2047_original.jpg",
                Duration = "4 HOURS",
                Price = 7000,
                TimeOfDay = "MORNING, AFTERNOON, EVENING"
            }, new Dining(){
                Id = 3,
                Name = "COUPLES DINING EXPERIENCE",
                Description = "An intimate dining experience for couples who wish to spend a special evening with toes in the sand as the sun sets and the stars come out. The scene is set for romance with Ruinart Rose, flowers and a private server, as you enjoy a personally curated four course menu highlighting local ingredients.",
                Images = "KON_1192_original.jpg",
                Duration = "2 HOURS",
                Price = 6000,
                TimeOfDay = "EVENING"
            }, new Dining(){
                Id = 4,
                Name = "DINING UNDER THE STARS",
                Description = "Before the sun begins to set, take a seat at a private table for two overlooking Hulopoe Bay. As evening descends, enjoy a Champagne welcome followed by a customized menu prepared just for you in consultation with our expert chefs.",
                Images = "MAN_2010_original.jpg",
                Duration = "3 HOURS",
                Price = 8000,
                TimeOfDay = "EVENING"
            }, new Dining(){
                Id = 5,
                Name = "WAILEA POINT PRIVATE DINING",
                Description = "A custom gourmet dinner prepared exclusively by our chef, with cocktails or wines selected by our sommelier and served by your own personal waiter on a private grassy knoll overlooking Wailea Beach.",
                Images = "MAU_2025_original.jpg",
                Duration = "3 HOURS",
                Price = 8000,
                TimeOfDay = "EVENING"}
            );

            modelBuilder.Entity<Offer>().HasData(
              new Offer()
              {
                  Id = 1,
                  OfferName = "Breakfast Only",
                  OfferDescription = "Start your day with our mouthwatering breakfast options",
                  OfferImage = "breakfast.jpg",
                  OfferPrice = 250
              },
              new Offer()
              {
                  Id = 2,
                  OfferName = "Lunch Only",
                  OfferDescription = "Indulge in our savory lunch specials that will tantalize your taste buds",
                  OfferImage = "lunch.jpg",
                  OfferPrice = 300
              },
              new Offer()
              {
                  Id = 3,
                  OfferName = "Both Breakfast and Lunch",
                  OfferDescription = "Experience the ultimate culinary journey with our delicious breakfast and lunch combo",
                  OfferImage = "combo.jpg",
                  OfferPrice = 500
              }
              );         


            base.OnModelCreating(modelBuilder);

        }


    }
}
