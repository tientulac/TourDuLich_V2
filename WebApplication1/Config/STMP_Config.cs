using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApplication1.Config
{
    public class STMP_Config
    {
        public static bool SendEmail(String to_email, String subject, String body, String password, String from_email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            mail.From = new MailAddress(from_email);
            mail.To.Add(to_email);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(from_email, password);

            try
            {
                // Cần cho phép các ứng dụng kém an toàn truy cập vào tài khoản email
                // Cho phép tại: https://myaccount.google.com/lesssecureapps
                SmtpServer.Send(mail);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}