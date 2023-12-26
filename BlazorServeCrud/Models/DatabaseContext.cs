using Microsoft.EntityFrameworkCore;

namespace BlazorServeCrud.Models
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) 
        { 
        }
        public DbSet<Person> Person { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DTask> Task { get; set; }
        public DbSet<TaskComment> TaskComment { get; set; }
        public DbSet<Categories> Categories { get; set; }
    }
}
