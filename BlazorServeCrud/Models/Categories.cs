using System.ComponentModel.DataAnnotations;

namespace BlazorServeCrud.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Isactive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
