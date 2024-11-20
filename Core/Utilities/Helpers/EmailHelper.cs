using Core.Utilities.Results;

namespace Core.Utilities.Helpers
{
    public static class EmailHelper
    {
        public static IResult SendPasswordResetEmail(string email, string resetToken)
        {
            var resetLink = $"http://localhost:5038/reset-password?token={resetToken}";
            return new SuccessResult(resetLink);
            var subject = "Şifre Sıfırlama Talebi";
            var body = $"Şifre sıfırlama linkiniz: <a href='{resetLink}'>Buraya Tıklayın</a>";

            //SendEmail(email, subject, body);
        }

        //public static void SendEmail(string toEmail, string subject, string body)
        //{
        //    try
        //    {
        //        // Gmail servisi alınıyor
        //        var service = GoogleOAuthHelper.GetGmailService();

        //        // E-posta mesajı hazırlanıyor
        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("duraliasan06ank@gmail.com"),
        //            Subject = subject,
        //            Body = body,
        //            IsBodyHtml = true
        //        };
        //        mailMessage.To.Add(toEmail);

        //        // E-posta içeriğini base64 encoding formatında Gmail API'ye gönderilecek şekilde hazırlıyoruz
        //        var message = new Message
        //        {
        //            Raw = EncodeMessage(mailMessage)
        //        };

        //        // E-posta gönderimi
        //        var request = service.Users.Messages.Send(message, "me");
        //        var result = request.Execute();

        //        Console.WriteLine($"E-posta başarıyla gönderildi. Mesaj ID: {result.Id}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"E-posta gönderilemedi: {ex.Message}");
        //    }
        //}

        //// MailMessage'i base64 encoding formatına dönüştürme
        //private static string EncodeMessage(MailMessage mailMessage)
        //{
        //    var emailContent = new StringBuilder();

        //    // Başlıklar
        //    emailContent.AppendLine("From: " + mailMessage.From.Address);
        //    emailContent.AppendLine("To: " + string.Join(",", mailMessage.To));
        //    emailContent.AppendLine("Subject: " + mailMessage.Subject);
        //    emailContent.AppendLine("MIME-Version: 1.0");
        //    emailContent.AppendLine("Content-Type: text/html; charset=UTF-8");
        //    emailContent.AppendLine();

        //    // Gövde
        //    emailContent.AppendLine(mailMessage.Body);

        //    // E-posta içeriğini Base64'e encode et
        //    byte[] byteArray = Encoding.UTF8.GetBytes(emailContent.ToString());
        //    return Convert.ToBase64String(byteArray)
        //        .Replace('+', '-')
        //        .Replace('/', '_')
        //        .Replace("=", "");
        //}
    }
}