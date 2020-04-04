using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Hcb.Insights
{
    public class Email
    {
        public static void Send(string fromEmail, string toEmail, string body )
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {   //Can only send email from within WinHost.
                return;
            }
            MailMessage mail = new MailMessage
            {
                //set the addresses 
                From = new MailAddress(fromEmail)
            };
            mail.To.Add(toEmail);

            //set the content 
            mail.Subject = "Client Inquiry";
            mail.Body = body;
            //send the message 
            SmtpClient smtp = new SmtpClient("mail.homecarebees.com");
            //            SmtpClient smtp = new SmtpClient("m06.internetmailserver.net");

            //Get the encrypted password from configuratio.
            IConfigurationSection config = Startup.Configuration.GetSection("hcb");
            string psw = config.GetValue<string>("emailPassword");

            //Decrypt the password.
            psw = Crypto.Decrypt(psw);

            //reverse the password
            char[] charArray = psw.ToCharArray();
            Array.Reverse(charArray);
            psw = new string(charArray);
            
            //Drop first and last two characters.
            psw = psw.Substring(1);
            psw = psw.Substring(0, psw.Length - 2);

            //Now you can use the pasword.
            NetworkCredential Credentials = new NetworkCredential("jean.minnaar@homecarebees.com", psw);
            smtp.Credentials = Credentials;
            smtp.Send(mail);

        }
    }
}
