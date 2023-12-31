﻿using BlazorServeCrud.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorServeCrud.Models
{
    public class DTask
    {
        public DTask()
        {
            TaskComment = new List<TaskComment>();
        }
        public int Id { get; set; }
        [Required]
        public string? Tasks { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; } = DateTime.Today;
        public DTaskStatus TaskStatus { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [NotMapped]
        public int TaskStatusId { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public string Comment { get; set; }
        public bool IsAgreeWithDueDate { get; set; } = true;
        public string Reason { get; set; }
        public bool IsExpertHelpRequired { get; set; }
        [InverseProperty("User")]
        public int ExpertId { get; set; }
        public virtual User User { get; set; }
        public ICollection<TaskComment> TaskComment { get; set; }
    }
}
