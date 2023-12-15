using BlazorServeCrud.Models;
using System.Text;

namespace BlazorServeCrud.Services
{
    public interface IUserService
    {
        Task<User> LoginAsync(LoginUser user);
        Task<bool> RegisterUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetAllExpertsAsync();
        Task<User> FindByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> SendEmail(string subject, StringBuilder Body, List<string> To, List<string> Cc, List<string> Bcc);
    }
}
