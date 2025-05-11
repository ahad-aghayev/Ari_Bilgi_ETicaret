using Eticaret.Core.Entities;
using System.Net;
using System.Net.Mail;
namespace Eticaret.WebUI.Utils
{
    public class MailHelper
    {
        public static async Task<bool> SendMailAysnc(Contact contact)
        {
            SmtpClient smtpClient = new SmtpClient("mail.siteadi.com", 587);
            smtpClient.Credentials = new NetworkCredential("aghayev.ahad@gmail.com", "123456789");
            smtpClient.EnableSsl = false;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("aghayev.ahad@gmail.com");
            message.To.Add("a.aghayev.ahad7@gmail.com");
            message.Subject = "Siteden mesaj geldi";
            message.Body = $"İsim:{contact.Name} - Soyadı: {contact.Surname}- Mail: {contact.Email}- Telefon: {contact.Phone}- Mesaj: {contact.Message}";
            message.IsBodyHtml = true;//html kodlarını desteklenmesi için
            try
            {
                await smtpClient.SendMailAsync(message);
                smtpClient.Dispose();
                return true;
            }
            catch (Exception)
            {
               return false;
            }
        }   
        public static async Task<bool> SendMailAysnc(string email,string subject,string mailBody)
        {
            SmtpClient smtpClient = new SmtpClient("mail.siteadi.com", 587);
            smtpClient.Credentials = new NetworkCredential("info@siteadi.com", "mailşifresi");
            smtpClient.EnableSsl = false;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("info@siteadi.com");
            message.To.Add(email);
            message.Subject = subject;
            message.Body = mailBody;
            message.IsBodyHtml = true;//html kodlarını desteklenmesi için
            try
            {
                await smtpClient.SendMailAsync(message);
                smtpClient.Dispose();
                return true;
            }
            catch (Exception)
            {
               return false;
            }
        }
    }
}
