using System.Net;
using Domain.Entities;
using Domain.Filters.Category;
using Domain.Models.Category;
using Domain.Wrappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.CategoryServices;

public class CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<PagedResponse<List<CategoryDto>>> GetCategories(CategoryFilter filter)
    {
        var validFilter = new CategoryFilter(filter.PageNumber, filter.PageSize);
        
        var query = context.Categories.AsNoTracking().AsQueryable();

        if (filter.Name != null)
        {
            query = query.Where(c => c.CategoryName.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
        }
        
        var data = await (
            from category in query
            select new CategoryDto()
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
            })
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        var totalRecords = data.Count;
        return new PagedResponse<List<CategoryDto>>(data, totalRecords, validFilter.PageNumber, validFilter.PageSize);
    }

    public async Task<Response<CategoryDto>> GetCategoryById(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
            return new Response<CategoryDto>(HttpStatusCode.BadRequest, "Category not found");
        
        return new Response<CategoryDto>(new CategoryDto()
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            Description = category.Description,
        });
    }

    public async Task<Response<CategoryDto>> AddCategory(CategoryDto categoryDto)
    {
        try
        {
            var category = new Category()
            {
                Id = categoryDto.Id,
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description,
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<CategoryDto>(categoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<CategoryDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<CategoryDto>> UpdateCategory(CategoryDto categoryDto)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryDto.Id);

            if (category == null)
                return new Response<CategoryDto>(HttpStatusCode.BadRequest, "Category not found");

            category.Id = categoryDto.Id;
            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;

            await context.SaveChangesAsync();

            return new Response<CategoryDto>(categoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<CategoryDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<string>> DeleteCategory(int id)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return new Response<string>(HttpStatusCode.BadRequest, "Category not found");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<string>("Category deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }
}
