using BlazorServeCrud.Enum;
using BlazorServeCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorServeCrud.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _ctx;
		public UserService(DatabaseContext databaseContext)
		{
			_ctx= databaseContext;
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
                if (data != null) { return data; }
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
                    _ctx.User.Update(user);
                else
                {
                    var data = await _ctx.User.FirstOrDefaultAsync(_ => _.Email == user.Email);
                    if (data != null) { return false; }
                    user.Role = Enum.Role.User;
                    _ctx.User.Add(user);
                }
                _ctx.SaveChanges();
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
                var data = await _ctx.User.Where(_=>_.Role != Role.Admin).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> FindByIdAsync(int id)
        {
            return await _ctx.User.FindAsync(id);
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
    }
}
