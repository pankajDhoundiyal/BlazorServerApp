using BlazorServeCrud.Models;
using System.Text;

namespace BlazorServeCrud.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string subject, StringBuilder Body, List<string> To, List<string> Cc, List<string> Bcc);
    }
}
