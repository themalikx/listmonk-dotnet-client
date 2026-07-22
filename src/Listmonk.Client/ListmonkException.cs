using System;
using System.Net;

namespace Listmonk.Client;

public sealed class ListmonkException : Exception
{
    public ListmonkException(string message, HttpStatusCode statusCode, string? responseBody = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string? ResponseBody { get; }
}
