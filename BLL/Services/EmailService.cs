using BLL.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace BLL.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var settings = _config.GetSection("EmailSettings");

            var smtp = new SmtpClient(settings["SmtpHost"])
            {
                Port = int.Parse(settings["SmtpPort"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                                            settings["Username"],     // ← Username not SenderEmail
                                            settings["AppPassword"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            smtp.Send(mail);
        }
        public void SendContactEmail(ContactDTO dto)
        {
            var settings = _config.GetSection("EmailSettings");
            var orgEmail = settings["SenderEmail"];

            var toOrgMail = new MailMessage
            {
                From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                Subject = "New Contact Message from " + dto.FullName,
                Body = $@"<h2>New Contact Message — IPMS</h2>
                        <p><strong>Name:</strong> {dto.FullName}</p>
                        <p><strong>Email:</strong> {dto.Email}</p>
                        <p><strong>Message:</strong> {dto.Message}</p>",
                IsBodyHtml = true
            };
            toOrgMail.To.Add(orgEmail);

            var toUserMail = new MailMessage
            {
                From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                Subject = "We received your message — IPMS",
                Body = $@"<h2>Thank you for contacting us, {dto.FullName}!</h2>
                        <p>We have received your message and our team will get back to you shortly.</p>
                        <hr/>
                        <p><strong>Your Message:</strong> {dto.Message}</p>
                        <br/>
                        <p>Regards,<br/><strong>IPMS Support Team</strong></p>",
                IsBodyHtml = true
            };
            toUserMail.To.Add(dto.Email);

            var smtp = new SmtpClient(settings["SmtpHost"])
            {
                Port = int.Parse(settings["SmtpPort"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                                            settings["SenderEmail"],
                                            settings["AppPassword"]),
                EnableSsl = true
            };

            try
            {
                smtp.Send(toOrgMail);
                Console.WriteLine("✅ Org email sent successfully");

                smtp.Send(toUserMail);
                Console.WriteLine("✅ User acknowledgement sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Email error: {ex.Message}");
                Console.WriteLine($"❌ Inner: {ex.InnerException?.Message}");
                throw; // ← re-throw so controller catches it
            }
        }
    }
}
