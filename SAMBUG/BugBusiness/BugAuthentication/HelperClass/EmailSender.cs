using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BugCentral.HelperClass
{
    class EmailSender
    {
        public String Result { get; set; }
        public MailAddress fromAddress { get; set; }
        public MailAddress toAddress { get; set; }
        public string fromPassword { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public EmailSender(String from,String Password, String To)
        {

            fromAddress = new MailAddress(from, "Admin");
            toAddress = new MailAddress(To, "User");
            fromPassword = Password; 
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
            
                try { smtp.Send(message); }
                catch(Exception e)
                {
                    return false;
                }
            return true;
        }
    }
}