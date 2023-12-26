using BlazorServeCrud.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorServeCrud.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly DatabaseContext _ctx;
        public CategoryService(DatabaseContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> AddUpdateCategoryAsync(Categories categories)
        {
            try
            {
                if(categories.Id> 0)
                {
                    _ctx.Categories.Update(categories);
                }
                else
                {
                    _ctx.Categories.Add(categories);
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _ctx.Categories.FindAsync(id);
                if (category == null)
                    return false;
                _ctx.Categories.Remove(category);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Categories>> GetAllCategoryAsync()
        {
            try
            {
                List<Categories> categories = await _ctx.Categories.OrderByDescending(m => m.Id).ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Categories> GetAllCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _ctx.Categories.FindAsync(id);
                if (category == null)
                    return new Categories();  
                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
