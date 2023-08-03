namespace Application.Wrappers;

public class Response<T>
{
    public Response()
    {
    }

    public Response(T data, string? message = null)
    {
        Data = data;
        Succeeded = true;
        Message = message;
    }

    public Response(string? message)
    {
        Succeeded = false;
        Message = message;
    }

    public T Data { get; set; } = default!;
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = null!;
}