using Business.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public SmtpEmailSender(IConfiguration config) => _config = config;

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var s = _config.GetSection("Smtp");
            using var client = new SmtpClient(s["Host"], int.Parse(s["Port"]!))
            {
                EnableSsl = bool.Parse(s["EnableSsl"]!),
                Credentials = new NetworkCredential(s["User"], s["Password"])
            };

            using var message = new MailMessage
            {
                From = new MailAddress(s["User"]!, s["FromName"]),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
}
