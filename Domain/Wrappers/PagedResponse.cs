using System.Net;

namespace Domain.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; } // 2
    public int PageSize { get; set; } // 12
    public int TotalPages { get; set; } // 30
    public int TotalRecords { get; set; } // 350

    public PagedResponse(T data, int totalRecords, int pageNumber, int pageSize) : base(data)
    {
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(((double)totalRecords / pageSize));
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public PagedResponse(HttpStatusCode code, string error) : base(code, error)
    {
    }
    
}
