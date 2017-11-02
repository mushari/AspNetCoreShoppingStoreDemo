using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(
            string email, string subject, string message)
        {
            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("martinwang7963@gmail.com", "Shopping Store");
            var to = new EmailAddress(email, "Dear User");
            var plainTextContent = message;
            var htmlContent = "<strong>" + message + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
