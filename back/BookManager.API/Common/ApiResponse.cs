namespace BookManager.API.Common;

public class ApiResponse<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? Message { get; private set; }
    public IEnumerable<string> Errors { get; private set; } = [];

    private ApiResponse() { }

    public static ApiResponse<T> Ok(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string error)
        => new() { Success = false, Errors = [error] };

    public static ApiResponse<T> Fail(IEnumerable<string> errors)
        => new() { Success = false, Errors = errors };
}

public class ApiResponse
{
    public bool Success { get; private set; }
    public string? Message { get; private set; }
    public IEnumerable<string> Errors { get; private set; } = [];

    private ApiResponse() { }

    public static ApiResponse Ok(string? message = null)
        => new() { Success = true, Message = message };

    public static ApiResponse Fail(string error)
        => new() { Success = false, Errors = [error] };
}
