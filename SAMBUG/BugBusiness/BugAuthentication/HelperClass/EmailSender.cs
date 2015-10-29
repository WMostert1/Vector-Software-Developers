using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BugCentral.HelperClass
{
    public class EmailSender
    {
        private static String From = "do.not.reply.sambug.vsd@gmail.com";
        private static String FromPassword = "SambugVSD4321";
        public String Result { get; set; }
        public MailAddress fromAddress { get; set; }
        public MailAddress toAddress { get; set; }
        public string fromPassword { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public EmailSender(String To)
        {

            fromAddress = new MailAddress(From, "Admin");
            toAddress = new MailAddress(To, "User");
            fromPassword = FromPassword; 
        } 

        public void setEmail(String Subject , String Body){
            subject = Subject;
            body = Body;
        }

        public Boolean sendEmail(){

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
                
       
                try {
                    message.IsBodyHtml = true;
                    smtp.Send(message); }
                catch(Exception)
                {
                    return false;
                }
            return true;
        }
    }
}