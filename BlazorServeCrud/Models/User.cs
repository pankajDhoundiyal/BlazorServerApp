using BlazorServeCrud.Enum;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace BlazorServeCrud.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNo { get; set; }
        public Role Role { get; set; }
        [NotMapped]
        public int RoleId { get; set; }
        public int ExpertCategoryId { get; set; }
        [NotMapped]
        public string ExpertFulllName => $"{FirstName} {LastName} - {(ExpertCategory)ExpertCategoryId}";
        public ICollection<DTask> Task { get; } = new List<DTask>();
    }
    public class LoginUser
    {
        public int Id { get; set; }
        
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        
    }
}
