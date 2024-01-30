using Domain.Filters.User;
using Domain.Models.User;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UserController(IUserService service) : Controller
{
    [HttpGet]
    public async Task<ActionResult> Index(UserFilter filter)
    {
        var users = await service.GetUsers(filter);
        return View(users);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View(new UserDto());
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.AddUser(userDto);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        var user = await service.GetUserById(id);
        var userDto = new UserDto()
        {
            Id = user.Data.Id,
            FirstName = user.Data.FirstName,
            LastName = user.Data.LastName,
            PhoneNumber = user.Data.PhoneNumber
        };
        return View(userDto);
    }
    
    [HttpPost]
    public async Task<ActionResult> Edit(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.UpdateUser(userDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await service.DeleteUser(id);
        if (result.HttpStatusCode != 200)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
