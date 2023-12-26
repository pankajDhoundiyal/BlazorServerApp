using BlazorServeCrud.Models;

namespace BlazorServeCrud.Services
{
    public interface ICategoryService
    {
        Task<bool> AddUpdateCategoryAsync(Categories categories);
        Task<bool> DeleteCategoryAsync(int id);
        Task<List<Categories>> GetAllCategoryAsync();
        Task<Categories> GetAllCategoryByIdAsync(int id);
    }
}
