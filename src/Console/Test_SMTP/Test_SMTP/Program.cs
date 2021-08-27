using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Test_SMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello SMTP Test!");
            Console.WriteLine("Enter an email address.");
            var emailTo = Console.ReadLine();
            if (string.IsNullOrEmpty(emailTo)) throw new ArgumentException("Email to is required.");
            SendEmail(emailTo);
            Console.WriteLine($"Test email sent to {emailTo}");
            Console.Read();
        }

        static void SendEmail(string email_to)
        {


            try
            {
                var variableTarget = EnvironmentVariableTarget.User;
                var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER_1", variableTarget);
                var smtpServerUserName = Environment.GetEnvironmentVariable("SMTP_SERVER_1_UserName", variableTarget);
                var smtpServerPassword = Environment.GetEnvironmentVariable("SMTP_SERVER_1_Password", variableTarget);
                var smtpServerEmailTo = email_to;
                var id = DateTime.Now.Ticks.ToString();

                using (var mySmtpClient = new SmtpClient(smtpServer))
                {
                    // set smtp-client with basicAuthentication
                    mySmtpClient.UseDefaultCredentials = false;
                    NetworkCredential basicAuthenticationInfo = new
                    NetworkCredential(smtpServerUserName, smtpServerPassword);
                    mySmtpClient.Credentials = basicAuthenticationInfo;                
                    mySmtpClient.EnableSsl = true;

                    // add from,to mailaddresses
                    MailAddress from = new MailAddress(smtpServerUserName, "TestFromName");
                    MailAddress to = new MailAddress(smtpServerEmailTo, "TestToName");
                    MailMessage myMail = new MailMessage(from, to);

                    // add ReplyTo
                    MailAddress replyTo = new MailAddress(smtpServerUserName);
                    myMail.ReplyToList.Add(replyTo);

                    // set subject and encoding
                    myMail.Subject = $"Test message - {id}";
                    myMail.SubjectEncoding = Encoding.UTF8;

                    // set body-message and encoding
                    myMail.Body = $"<b>Test Mail</b><br>using <b>HTML</b> - {id}.";
                    myMail.BodyEncoding = Encoding.UTF8;
                    // text or html
                    myMail.IsBodyHtml = true;

                    mySmtpClient.Send(myMail);
                }      
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
