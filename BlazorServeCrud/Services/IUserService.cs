using BlazorServeCrud.Models;

namespace BlazorServeCrud.Services
{
    public interface IUserService
    {
        Task<User> LoginAsync(LoginUser user);
        Task<bool> RegisterUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<User> FindByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
