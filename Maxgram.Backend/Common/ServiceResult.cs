namespace Maxgram.Backend.Common;

public class ServiceResult<T>
{
    public bool Success => Error == ErrorCode.None;
    public ErrorCode Error { get; init; } = ErrorCode.None;
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static ServiceResult<T> Ok(T data) => new() { Data = data };
    public static ServiceResult<T> Fail(ErrorCode code, string message) =>
        new() { Error = code, Message = message };
}
