using Domain.Filters.Task;
using Domain.Models.Task;
using Domain.Wrappers;

namespace Infrastructure.Services.TaskServices;

public interface ITaskService
{
    Task<PagedResponse<List<TaskDto>>> GetTasks(TaskFilter filter);
    Task<Response<TaskDto>> GetTaskById(int id);
    Task<Response<TaskDto>> AddTask(TaskDto userDto);
    Task<Response<TaskDto>> UpdateTask(TaskDto userDto);
    Task<Response<string>> DeleteTask(int id);
}
