using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Category;

public class CategoryDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(50, ErrorMessage = "Category name cannot be longer than 50 characters")]
    public string CategoryName { get; set; }
    [MaxLength(200, ErrorMessage = "Category description cannot be longer than 200 characters")]
    public string Description { get; set; }
}
