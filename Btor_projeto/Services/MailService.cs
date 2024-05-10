using System.Net.Mail;
using System.Net;

namespace Btor_projeto
{
    public class MailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            var fromAddress = new MailAddress("cmsfabrica@gmail.com", "Btor");
            var toAddress = new MailAddress(to);

            const string fromPassword = "txvp rizf qvzg fbhf";
            const string smtpServer = "smtp.gmail.com";
            const int port = 587;

            var smtp = new SmtpClient
            {
                Host = smtpServer,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Se o corpo do e-mail contém HTML
            };

            smtp.Send(message);
        }
    }
}