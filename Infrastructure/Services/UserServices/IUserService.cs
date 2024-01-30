using Domain.Entities;
using Domain.Filters.User;
using Domain.Models.User;
using Domain.Wrappers;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Services.UserServices;

public interface IUserService
{
    Task<PagedResponse<List<UserDto>>> GetUsers(UserFilter filter);
    Task<Response<UserDto>> GetUserById(int id);
    Task<Response<UserDto>> AddUser(UserDto userDto);
    Task<Response<UserDto>> UpdateUser(UserDto userDto);
    Task<Response<string>> DeleteUser(int id);
}
