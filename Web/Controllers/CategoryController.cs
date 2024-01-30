using Domain.Entities;
using Domain.Filters.Category;
using Domain.Models.Category;
using Infrastructure.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class CategoryController(ICategoryService service) : Controller
{
    [HttpGet]
    public async Task<ActionResult> Index(CategoryFilter filter)
    {
        var categories = await service.GetCategories(filter);
        return View(categories);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View(new CategoryDto());
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.AddCategory(categoryDto);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        var category = await service.GetCategoryById(id);
        var categoryDto = new CategoryDto()
        {
            Id = category.Data.Id,
            CategoryName = category.Data.CategoryName,
            Description = category.Data.Description,
        };
        return View(categoryDto);
    }
    
    [HttpPost]
    public async Task<ActionResult> Edit(CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        await service.UpdateCategory(categoryDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await service.DeleteCategory(id);
        if (result.HttpStatusCode != 200)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
