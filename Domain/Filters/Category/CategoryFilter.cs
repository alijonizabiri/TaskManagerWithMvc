namespace Domain.Filters.Category;

public class CategoryFilter : PaginationFilter
{
    public string? Name { get; set; } = null;
    public CategoryFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    
    public CategoryFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }   
}
