using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GPUScalper
{
    internal class EmailSender
    {
        public string passedSenderEmailAddressForNewCartNotifications = "";
        public string passedSenderEmailPassForNewCartNotifications = "";

        public void SendGPUAlertEmail(string url)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("tylortrub@gmail.com");
                message.To.Add(new MailAddress("tylortrub@gmail.com"));
                message.Subject = "GPU Auto-Scalper";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = $"SCALPER FOUND A GPU AND HAS ADDED IT TO THE CART AT {DateTime.Now}; Go get it!";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(passedSenderEmailAddressForNewCartNotifications, passedSenderEmailPassForNewCartNotifications);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) { Console.WriteLine($"ERROR SENDING EMAIL: {ex.Message}"); }

        }

    }



}  
