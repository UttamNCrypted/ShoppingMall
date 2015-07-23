using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShoppingMall.Models
{
    public class GeneralUtility
    {
        System.Net.Mail.MailMessage newMail = new System.Net.Mail.MailMessage();

        public void SendEmail(string emailTo, string subject, string msgBody)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                 //setup Smtp authentication
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("buntybunty2890@gmail.com", "bunty@123456");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("buntybunty2890@gmail.com");
                msg.To.Add(new MailAddress(emailTo));

                msg.Subject = "This is a test Email subject";
                msg.IsBodyHtml = true;
                msg.Body = msgBody;

                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}