using BlazorServeCrud.Models;

namespace BlazorServeCrud.Services
{
    public interface ITaskService
    {
        Task<bool> AddUpdateTaskAsync(DTask task);
        Task<bool> AddUpdateUserTaskAsync(DTask task);
        Task<List<DTask>> GetAllTaskAsync();
        Task<DTask> GetTaskByIdAsync(int id);
        Task<List<DTask>> GetUsersTaskAsync(string username);
        Task<bool> DeleteTaskAsync(int id);
    }
}
