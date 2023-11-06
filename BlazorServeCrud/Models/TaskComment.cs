using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorServeCrud.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        [ForeignKey("Task")]
        public int TaskId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DTask Task { get; set; } 
        
        public User User { get; set; }
    }
}
