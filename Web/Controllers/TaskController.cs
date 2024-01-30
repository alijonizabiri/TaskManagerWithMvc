using Domain.Enums;
using Domain.Filters.Category;
using Domain.Filters.Task;
using Domain.Filters.User;
using Domain.Models.Task;
using Infrastructure.Services.CategoryServices;
using Infrastructure.Services.TaskServices;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

public class TaskController(ITaskService service, ICategoryService categoryService, IUserService userService) : Controller
{
    [HttpGet]
    public async Task<ActionResult> Index(TaskFilter filter)
    {
        var tasks = await service.GetTasks(filter);
        return View(tasks);
    }

    [HttpGet]
    public ActionResult Create()
    {
        var categories = categoryService.GetCategories(new CategoryFilter()).Result.Data;
        var users = userService.GetUsers(new UserFilter()).Result.Data;
        var priorities = Enum.GetNames(typeof(TaskPriority)).ToList();
        ViewBag.Categories = categories;
        ViewBag.Users = users;
        ViewBag.Priorities = priorities;
        return View(new TaskDto());
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(TaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.AddTask(taskDto);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        var task = await service.GetTaskById(id);
        var taskDto = new TaskDto()
        {
            Id = task.Data.Id,
            Title = task.Data.Title,
            Description = task.Data.Description,
            CategoryId = task.Data.CategoryId,
            CategoryName = task.Data.CategoryName,
            UserId = task.Data.UserId,
            UserName = task.Data.UserName,
            TaskPriority = task.Data.TaskPriority,
            IsComleted = task.Data.IsComleted,
        };
        var categories = categoryService.GetCategories(new CategoryFilter()).Result.Data;
        var users = userService.GetUsers(new UserFilter()).Result.Data;
        var priorities = Enum.GetNames(typeof(TaskPriority)).ToList();
        ViewBag.Categories = categories;
        ViewBag.Users = users;
        ViewBag.Priorities = priorities;
        return View(taskDto);
    }
    
    [HttpPost]
    public async Task<ActionResult> Edit(TaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.UpdateTask(taskDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await service.DeleteTask(id);
        if (result.HttpStatusCode != 200)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
