using HotelManagement_MVC.Models;
using System.Net;
using System.Net.Mail;

namespace HotelManagement_MVC.Helper
{
	public static class EmailSettings
	{
		public static void sendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl=true;
			client.Credentials = new NetworkCredential("bookingroom177@gmail.com", "dmhbveehplfhjgto");
			client.Send("hotelbooking@bookingroom.com", email.To, email.Subject, email.Body);
		}
	}
}
