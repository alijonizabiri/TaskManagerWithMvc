using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(50, ErrorMessage = "Category name cannot be longer than 50 characters")]
    public string CategoryName { get; set; }
    [MaxLength(200, ErrorMessage = "Category description cannot be longer than 200 characters")]
    public string Description { get; set; }
    public virtual List<Task> Tasks { get; set; }
}
