using System;
using System.Net;
using System.Net.Mail;
using MyMLAppML.Model;
using PLplot;

namespace myMLApp
{
    class Program
    {
        enum MyEnum
        {
            Normal,
            MisAllignment
        }
        static void Main(string[] args)
        {
            // Add input data
            var input = new ModelInput();
            //input.Vibxaccg = 0.210577715F;
            //input.Vibyaccg = -0.021362957F;
            //input.Chasiaccg = 0.088503677F;

            input.Vibxaccg = 0.125125889F;
            input.Vibyaccg = -0.143436994F;
            input.Chasiaccg = -0.100711081F;


            // Load model and predict output of sample data
            ModelOutput result = ConsumeModel.Predict(input);
            Console.WriteLine($"Input: \nVibX: {input.Vibxaccg} \nVibY: {input.Vibyaccg} \nChasi: {input.Chasiaccg} \nResult: {result.Prediction}");

        }


        private static void SentEmail()
        {
            var fromAddress = new MailAddress("amitknit09@gmail.com", "Amit Kanaujia");
            var toAddress = new MailAddress("vidya.marathe@honeywell", "vidya");
            const string fromPassword = "";
            const string subject = "Alert";
            const string body = "There is some error detected in pump.";

            var smtp = new SmtpClient
            {
                Host = "74.125.142.108",
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
            {
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }
    }
}