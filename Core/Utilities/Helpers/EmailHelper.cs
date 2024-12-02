using Core.Utilities.Results;
using System.Net;
using System.Net.Mail;

namespace Core.Utilities.Helpers
{
    public static class EmailHelper
    {
        public static IResult SendPasswordResetEmail(string email, string resetToken)
        {
            var resetLink = $"http://localhost:5038/reset-password?token={resetToken}";
            var subject = "ŞİFRE SIFIRLAMA TALEBİ";
            var body = $"Şifre sıfırlama linkiniz: <a href='{resetLink}'>Buraya Tıklayın</a>";

            var result = YandexSmtp(email, subject, body);
            return result;
        }

        private static IResult YandexSmtp(string toEmail, string subject, string body)
        {
            var fromEmail = "duraliasan@yandex.com";
            var appPassword = "flvnducroqrwnwca";


            var smtpClient = new SmtpClient()
            {
                Port = 587, // SSL: 465, TLS: 587, Eski:25
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true,
                Host = "smtp.yandex.com",
                UseDefaultCredentials = false,
                Timeout = 50000,

            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
                return new SuccessResult("Mail kutunuzu kontrol edin.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("E-mail gönderilemedi.");
            }
        }
    }
}