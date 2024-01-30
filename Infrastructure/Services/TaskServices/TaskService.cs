using System.Net;
using Domain.Entities;
using Domain.Filters.Task;
using Domain.Models.Task;
using Domain.Wrappers;
using Infrastructure.Persistence.Context;
using Infrastructure.Services.TaskServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = Domain.Entities.Task;

namespace Infrastructure.Services.TaskServices;

public class TaskService(ApplicationDbContext context, ILogger<TaskService> logger) : ITaskService
{
    public async Task<PagedResponse<List<TaskDto>>> GetTasks(TaskFilter filter)
    {
        var validFilter = new TaskFilter(filter.PageNumber, filter.PageSize);
        
        var query = context.Tasks.AsNoTracking().AsQueryable();

        if (filter.Title != null)
        {
            query = query.Where(t => t.Title.ToLower().Trim().Contains(filter.Title.ToLower().Trim()));
        }
        
        if (filter.UserName != null)
        {
            query = query.Where(t => string
                    .Concat(t.Assignee.FirstName, t.Assignee.LastName).ToLower().Trim()
                    .Contains(filter.UserName.ToLower().Trim()));
        }
        
        if (filter.CategoryName != null)
        {
            query = query.Where(t => t.Category.CategoryName.ToLower().Trim()
                .Contains(filter.CategoryName.ToLower().Trim()));
        }
        
        if (filter.TaskPriority != null)
        {
            query = query.Where(t => t.TaskPriority == filter.TaskPriority);
        }
        
        if (filter.IsComleted != null)
        {
            query = query.Where(t => t.IsComleted == filter.IsComleted);
        }
        
        if (filter.CreatedAt != null)
        {
            query = query.Where(t => 
                t.CreatedAt.Month == filter.CreatedAt.Value.Month && 
                t.CreatedAt.Day == filter.CreatedAt.Value.Day && 
                t.CreatedAt.Year == filter.CreatedAt.Value.Year);
        }
        
        var data = await (
            from task in query
            select new TaskDto()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UserId = task.UserId,
                UserName = string.Concat(task.Assignee.FirstName, " " ,task.Assignee.LastName),
                CategoryId = task.CategoryId,
                CategoryName = task.Category.CategoryName,
                CreatedAt = task.CreatedAt,
                TaskPriority = task.TaskPriority,
                IsComleted = task.IsComleted
                
            })
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        var totalRecords = data.Count;
        return new PagedResponse<List<TaskDto>>(data, totalRecords, validFilter.PageNumber, validFilter.PageSize);
    }

    public async Task<Response<TaskDto>> GetTaskById(int id)
    {
        var task = await context.Tasks
            .Include(task => task.Assignee)
            .Include(task => task.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (task == null)
            return new Response<TaskDto>(HttpStatusCode.BadRequest, "Task not found");
        
        return new Response<TaskDto>(new TaskDto()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            UserId = task.UserId,
            UserName = string.Concat(task.Assignee.FirstName, " " ,task.Assignee.FirstName),
            CategoryId = task.CategoryId,
            CategoryName = task.Category.CategoryName,
            CreatedAt = task.CreatedAt,
            TaskPriority = task.TaskPriority,
            IsComleted = task.IsComleted
        });
    }

    public async Task<Response<TaskDto>> AddTask(TaskDto taskDto)
    {
        try
        {
            var task = new Task()
            {
                Id = taskDto.Id,
                Title = taskDto.Title,
                Description = taskDto.Description,
                UserId = taskDto.UserId,
                CategoryId = taskDto.CategoryId,
                CreatedAt = DateTimeOffset.UtcNow,
                TaskPriority = taskDto.TaskPriority,
                IsComleted = taskDto.IsComleted
            };

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            return new Response<TaskDto>(taskDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<TaskDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<TaskDto>> UpdateTask(TaskDto taskDto)
    {
        try
        {
            var task = await context.Tasks.FirstOrDefaultAsync(c => c.Id == taskDto.Id);

            if (task == null)
                return new Response<TaskDto>(HttpStatusCode.BadRequest, "Task not found");
            
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.UserId = taskDto.UserId;
            task.CategoryId = taskDto.CategoryId;
            task.TaskPriority = taskDto.TaskPriority;
            task.IsComleted = taskDto.IsComleted;

            await context.SaveChangesAsync();

            return new Response<TaskDto>(taskDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<TaskDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<string>> DeleteTask(int id)
    {
        try
        {
            var task = await context.Tasks.FirstOrDefaultAsync(c => c.Id == id);

            if (task == null)
                return new Response<string>(HttpStatusCode.BadRequest, "Task not found");

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return new Response<string>("Task deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }
}
