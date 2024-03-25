using System.Net.Mail;
using System.Net;

namespace Utilites
{
    public static class EmailUtilities
    {
        public static void SendEmail(string subject, string body)
        {
            string smtpServer = "smtp.mailgun.org";
            int port = 587;
            string userName = "postmaster@sandbox486de8fe9c9b4c8f9cc88dac6ae5a00f.mailgun.org";
            string password = "7e88b4aa14af895fed8947413b508b2d-309b0ef4-1c6d8b12";
            try
            {
                using var client = new SmtpClient(smtpServer, port);

                client.Credentials = new NetworkCredential(userName, password);

                var message = new MailMessage("ITI Team 3 <ClinicProject@gmail.com>", "maxcode545@gmail.com", subject, body);

                client.Send(message);
                Console.WriteLine(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log error)
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine(HttpStatusCode.InternalServerError);
            }
        }
        public static string GenerateKeypass(int size)
        {
            Random rand = new Random();

            string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }
}
