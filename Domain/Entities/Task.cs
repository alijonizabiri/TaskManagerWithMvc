using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Task
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Task title is required")]
    [MaxLength(50, ErrorMessage = "Task title cannot be longer than 50 characters")]
    public string Title { get; set; }

    [MaxLength(200, ErrorMessage = "Task title cannot be longer than 200 characters")]
    public string? Description { get; set; } = null;
    public int UserId { get; set; }
    public virtual User Assignee { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public bool IsComleted { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
