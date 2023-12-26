using BlazorServeCrud.Enum;
using BlazorServeCrud.Models;
using BlazorServeCrud.Pages.Admin;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;

namespace BlazorServeCrud.Services
{
    public class TaskService : ITaskService
    {
        private readonly DatabaseContext _ctx;
        private readonly IConfiguration configuration;
        public TaskService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _ctx = databaseContext;
            this.configuration = configuration;
        }
        public async Task<bool> AddUpdateTaskAsync(DTask task)
        {
            try
            {
                var user = await _ctx.User.FirstOrDefaultAsync(m => m.Id == task.UserId);
                if (task.Id > 0)
                    _ctx.Task.Update(task);
                else
                {
                    task.TaskStatus = DTaskStatus.Active;
                    task.Remarks = task.Remarks == null ? "" : task.Remarks;
                    task.Reason = task.Reason == null ? "" : task.Reason;
                    _ctx.Task.Add(task);
                }
                _ctx.SaveChanges();
                List<string> emails = new List<string>();
                emails.Add(user.Email);
                string baseUrl = configuration.GetValue<string>("BaseUrl");
                StringBuilder body = new StringBuilder();
                body.AppendFormat("<h1>Task Assignment</h1>");
                body.AppendFormat("Dear {0},", user.FirstName + " " + user.LastName);
                body.AppendFormat("<br />");
                body.AppendFormat("<p>You have been assigned a Task by the Admin, please click on the below link to login to portal to view the Task Details!</p>");
                body.AppendFormat("TaskName - {0}", task.Tasks);
                body.AppendFormat("<br />");
                body.AppendFormat("<br />");
                body.AppendFormat("<a href=" + baseUrl + "> Click here to Login</a>");
                body.AppendFormat("<br />");
                body.AppendFormat("<br />");
                body.AppendFormat("Warm Regards,");
                body.AppendFormat("<br />");
                body.AppendFormat("Admin");
                // send email
                await SendEmail("Task Assignment", body, emails, null, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddUpdateUserTaskAsync(DTask task)
        {
            bool sendEmailtoExpert = false;
            bool sendEmailtoAdmin = false;
            try
            {
                if (task.Id > 0)
                {
                    task.TaskStatus = (DTaskStatus)task.TaskStatusId;
                    if (task.IsExpertHelpRequired)
                    {
                        sendEmailtoExpert = true;
                    }
                    if (!task.IsAgreeWithDueDate)
                    {
                        sendEmailtoAdmin = true;
                    }
                    if (!task.IsExpertHelpRequired)
                    {
                        task.ExpertId = 0;
                    }
                    if (task.IsAgreeWithDueDate)
                    {
                        task.Reason = "";
                    }
                    _ctx.Task.Update(task);
                    //if(task.Comment!=null)
                    //{
                    //    TaskComment taskComment = new TaskComment();
                    //    taskComment.TaskId = task.Id;
                    //    taskComment.Comment = task.Comment;
                    //    taskComment.UserId = task.UserId;
                    //    _ctx.TaskComment.Add(taskComment);
                    //}
                }
                else
                {
                    task.TaskStatus = DTaskStatus.Active;
                    _ctx.Task.Add(task);
                }
                _ctx.SaveChanges();
                if(sendEmailtoExpert)
                {
                    // send email to expert
                    var usr = await _ctx.User.Where(m => m.Id == task.ExpertId).FirstOrDefaultAsync();
                    var tsk = await _ctx.User.Where(m => m.Id == task.UserId).FirstOrDefaultAsync();
                    List<string> emails = new List<string>();
                    emails.Add(usr.Email);
                    string baseUrl = configuration.GetValue<string>("BaseUrl");
                    StringBuilder body = new StringBuilder();
                    body.AppendFormat("<h1>Assistance Required on Task</h1>");
                    body.AppendFormat("Dear {0},", usr.FirstName + " " + usr.LastName);
                    body.AppendFormat("<br />");
                    body.AppendFormat("<p>You have been requested by the user '"+ tsk.Email+"' to help him on the Task Assigned by the Admin, please click on the below link to login to portal to view the Task Details!</p>");
                    body.AppendFormat("TaskName - {0}", task.Tasks);
                    body.AppendFormat("<br />");
                    body.AppendFormat("<br />");
                    body.AppendFormat("<a href=" + baseUrl + "> Click here to Login</a>");
                    body.AppendFormat("<br />");
                    body.AppendFormat("<br />");
                    body.AppendFormat("Warm Regards,");
                    body.AppendFormat("<br />");
                    //body.AppendFormat("Admin");
                    // send email
                    await SendEmail("Assistance Required on Task", body, emails, null, null);
                }
                if (sendEmailtoAdmin)
                {
                    // send email to Admin
                    var usr = await _ctx.User.Where(m => m.Role == Role.Admin).FirstOrDefaultAsync();
                    List<string> emails = new List<string>();
                    emails.Add(usr.Email);
                    string baseUrl = configuration.GetValue<string>("BaseUrl");
                    StringBuilder body = new StringBuilder();
                    body.AppendFormat("<h1>DueDate change Request</h1>");
                    body.AppendFormat("Dear {0},", usr.FirstName + " " + usr.LastName);
                    body.AppendFormat("<br />");
                    body.AppendFormat("<p>You have been requested to change the Task Duedate, please click on the below link to login to portal to view the Task Details!</p>");
                    body.AppendFormat("TaskName - {0}", task.Tasks);
                    body.AppendFormat("<br />");
                    body.AppendFormat("Reason mentioned - {0}", task.Reason);
                    body.AppendFormat("<br />");
                    body.AppendFormat("<br />");
                    body.AppendFormat("<a href=" + baseUrl + "> Click here to Login</a>");
                    body.AppendFormat("<br />");
                    body.AppendFormat("<br />");
                    body.AppendFormat("Warm Regards,");
                    body.AppendFormat("<br />");
                    // send email
                    await SendEmail("DueDate change Request", body, emails, null, null);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<DTask>> GetAllTaskAsync()
        {
            try
            {
                List<DTask> tasks = await _ctx.Task.OrderByDescending(m => m.Id)
                                     .Include(m => m.User).ToListAsync();
                return tasks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<DTask> GetTaskByIdAsync(int id)
        {
            DTask task = await _ctx.Task.FirstOrDefaultAsync(_ => _.Id == id);
            task.TaskComment = await _ctx.TaskComment.Where(_ => _.TaskId == task.Id).ToListAsync();
            task.TaskStatusId = (int)task.TaskStatus;
            return task;
        }
        public async Task<List<DTask>> GetUsersTaskAsync(string username)
        {
            var user = await _ctx.User.FirstOrDefaultAsync(_ => _.Email == username);
            if (user != null)
            {
                return await _ctx.Task.Where(_ => _.UserId == user.Id).OrderByDescending(m => m.Id).ToListAsync();
            }
            else
                return null;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _ctx.Task.FindAsync(id);
                if (task == null)
                    return false;
                _ctx.Task.Remove(task);
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
