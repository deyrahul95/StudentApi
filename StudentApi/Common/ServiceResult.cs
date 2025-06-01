using System.Net;

namespace StudentApi.Common;

public class ServiceResult(HttpStatusCode statusCode, string message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
}

public class ServiceResult<T>(
    HttpStatusCode statusCode,
    string message,
    T? data = default) : ServiceResult(statusCode: statusCode, message: message)
{
    public T? Data { get; set; } = data;
}