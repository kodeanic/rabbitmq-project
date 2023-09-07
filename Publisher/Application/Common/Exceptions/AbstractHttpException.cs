using System.Net;

namespace Application.Common.Exceptions;

public abstract class AbstractHttpException : Exception
{
    public readonly int StatusCode;

    public new readonly string? Message;

    protected AbstractHttpException(HttpStatusCode httpCode, string? message)
    {
        StatusCode = (int)httpCode;
        Message = message;
    }
}