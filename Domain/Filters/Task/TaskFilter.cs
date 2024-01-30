using Domain.Enums;

namespace Domain.Filters.Task;

public class TaskFilter : PaginationFilter
{
    public string? Title { get; set; } = null;
    public string? UserName { get; set; } = null;
    public string? CategoryName { get; set; } = null;
    public TaskPriority? TaskPriority { get; set; } = null;
    public bool? IsComleted { get; set; } = null;
    public DateTimeOffset? CreatedAt { get; set; } = null;
    
    public TaskFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public TaskFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }
}
