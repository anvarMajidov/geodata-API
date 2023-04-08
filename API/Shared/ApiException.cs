using System.Net;

namespace API.Shared;

public class ApiException
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
