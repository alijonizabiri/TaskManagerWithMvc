using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Domain.Models.User;

public class UserDto
{
    public int Id { get; set; }
    [MaxLength(50, ErrorMessage = "First name can not be greater than 50 characters")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    [MaxLength(50, ErrorMessage = "LAst name can not be greater than 50 characters")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; }
    //public IFormFile Image { get; set; }
}
