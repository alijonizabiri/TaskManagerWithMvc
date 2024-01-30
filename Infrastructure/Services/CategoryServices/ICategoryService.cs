using Domain.Filters.Category;
using Domain.Models.Category;
using Domain.Wrappers;

namespace Infrastructure.Services.CategoryServices;

public interface ICategoryService
{
    Task<PagedResponse<List<CategoryDto>>> GetCategories(CategoryFilter filter);
    Task<Response<CategoryDto>> GetCategoryById(int id);
    Task<Response<CategoryDto>> AddCategory(CategoryDto userDto);
    Task<Response<CategoryDto>> UpdateCategory(CategoryDto userDto);
    Task<Response<string>> DeleteCategory(int id);
}
