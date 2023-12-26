using BlazorServeCrud.Enum;
using BlazorServeCrud.Models;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System.Collections.Specialized;
using System.Text;

namespace BlazorServeCrud.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _ctx;
        private readonly IConfiguration configuration;
        public UserService(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_ctx= databaseContext;
            this.configuration = configuration;
        }
        public async Task<User> LoginAsync(LoginUser user)
        {
			try
			{
                if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                {
                    return null;
                }
                var data = await _ctx.User.FirstOrDefaultAsync(_=>_.Email== user.Email && _.Password==user.Password);
                if (data != null) {                    
                    return data; 
                }
                else return null;
            }
			catch (Exception ex)
			{
				return null;
			}
        }
        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                if (user.Id > 0)
                {
                    user.Role = (Role)user.RoleId;
                    if(user.Role == Role.User)
                    {
                        user.ExpertCategoryId = 0;
                    }
                    _ctx.User.Update(user);

                }
                else
                {
                    var data = await _ctx.User.FirstOrDefaultAsync(_ => _.Email == user.Email);
                    if (data != null) { return false; }
                    user.Role = (Role)user.RoleId;
                    _ctx.User.Add(user);
                }
                _ctx.SaveChanges();
                List<string> emails = new List<string>();
                emails.Add(user.Email);
                string baseUrl = configuration.GetValue<string>("BaseUrl");
                StringBuilder body = new StringBuilder();
                body.AppendFormat("<h1>User Registration</h1>");
                body.AppendFormat("Dear {0},", user.FirstName +" "+user.LastName);
                body.AppendFormat("<br />");
                body.AppendFormat("<p>Thank you for registering with us!</p>");
                body.AppendFormat("<p>Please find the details below to login </p>");
                body.AppendFormat("UserName - {0}", user.Email);
                body.AppendFormat("<br />");
                body.AppendFormat("Password - {0}", user.Password);
                body.AppendFormat("<br />");
                body.AppendFormat("<br />");
                body.AppendFormat("<a href="+baseUrl+"> Click here to Login</a>");
                body.AppendFormat("<br />");
                body.AppendFormat("<br />");
                body.AppendFormat("Warm Regards,");
                body.AppendFormat("<br />");
                body.AppendFormat("Admin");
                // send email
                await SendEmail("User Registration", body, emails, null, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var data = await _ctx.User.FirstOrDefaultAsync(_ => _.Email == email);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                var data = await _ctx.User.Where(_=>_.Role != Role.Admin).OrderByDescending(m => m.Id).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<User>> GetAllExpertsAsync()
        {
            try
            {
                var data = await _ctx.User.Where(_ => _.Role != Role.Admin && _.Role != Role.User).ToListAsync();
                foreach (var item in data)
                {
                    var res = await _ctx.Categories.FindAsync(item.ExpertCategoryId);
                    if (res != null)
                        item.ExpertFulllName = string.Format("{0} {1} {2}", item.FirstName, item.LastName + " - ", res.CategoryName);
                }
                
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _ctx.User.FindAsync(id);
            user.RoleId = (int)user.Role;
            return user;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await FindByIdAsync(id);
                if (user == null)
                    return false;
                _ctx.User.Remove(user);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
