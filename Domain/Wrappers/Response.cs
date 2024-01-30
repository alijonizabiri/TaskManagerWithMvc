using System.Net;

namespace Domain.Wrappers;

public class Response<T>
{
    public int HttpStatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
    public T Data { get; set; }

    public Response(T data)
    {
        HttpStatusCode = 200;
        ErrorMessages = new List<string>();
        Data = data;
    }

    public Response(HttpStatusCode statusCode, string message)
    {
        HttpStatusCode = (int)statusCode;
        ErrorMessages = new List<string>() { message };
    }
    
    public Response(HttpStatusCode statusCode, string message, T data)
    {
        HttpStatusCode = (int)statusCode;
        ErrorMessages = new List<string>() { message };
        Data = data;
    }
}
