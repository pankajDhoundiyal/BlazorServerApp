using BlazorServeCrud.Models;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace BlazorServeCrud.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            //this.configuration = configuration;
        }
        public async Task<bool> SendEmail(string subject, StringBuilder Body, List<string> To, List<string> Cc, List<string> Bcc)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(configuration.GetValue<string>("EmailSettings:From")));
                foreach (var toAddress in To)
                {
                    email.To.Add(MailboxAddress.Parse(toAddress));
                }
                // Add CC and BCC recipients if provided
                if (Cc != null)
                {
                    foreach (var ccAddress in Cc)
                    {
                        email.Cc.Add(MailboxAddress.Parse(ccAddress));
                    }
                }
                if (Bcc != null)
                {
                    foreach (var bccAddress in Bcc)
                    {
                        email.Bcc.Add(MailboxAddress.Parse(bccAddress));
                    }
                }
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = Body.ToString() };
                // send email
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    try
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        client.Connect(configuration.GetValue<string>("EmailSettings:Host"), configuration.GetValue<int>("EmailSettings:Port"), SecureSocketOptions.Auto);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        await client.AuthenticateAsync(configuration.GetValue<string>("EmailSettings:From"), configuration.GetValue<string>("EmailSettings:Password"));
                        await client.SendAsync((MimeKit.MimeMessage)email);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
