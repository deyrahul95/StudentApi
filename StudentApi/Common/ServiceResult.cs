using System.Net;

namespace StudentApi.Common;

/// <summary>
/// Represents the outcome of a service operation including status code and message
/// </summary>
public class ServiceResult(HttpStatusCode statusCode, string message)
{
    /// <summary>
    /// Gets or sets the HTTP status code of the operation result
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = statusCode;

    /// <summary>
    /// Gets or sets a message describing the result of the operation
    /// </summary>
    public string Message { get; set; } = message;
}

/// <summary>
/// Represents the outcome of a service operation including status code, message, and data
/// Inherits from <see cref="ServiceResult"/>
/// </summary>
/// <typeparam name="T">The type of the data returned in the result</typeparam>
public class ServiceResult<T>(
    HttpStatusCode statusCode,
    string message,
    T? data = default) : ServiceResult(statusCode: statusCode, message: message)
{
    /// <summary>
    /// Gets or sets the data returned by the operation
    /// </summary>
    public T? Data { get; set; } = data;
}
