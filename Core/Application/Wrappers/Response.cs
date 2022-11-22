namespace Application.Wrappers;

public class Response<T>
{
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }


    public Response()
    {
    }

    public Response(T data, string message = null)
    {
        Data = data;
        Succeeded = true;
        Message = message;
    }

    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }
}