namespace Domain.Filters.User;

public class UserFilter : PaginationFilter
{
    public string? Name { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    
    public UserFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    
    public UserFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }
}
