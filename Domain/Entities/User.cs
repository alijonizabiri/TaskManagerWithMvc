using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; }
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    //public string Image { get; set; }
    public virtual List<Task> Tasks { get; set; }
}
