using Microsoft.AspNetCore.Http;

namespace project_fob.Controllers
{
    public interface ICookies
    {
        IRequestCookieCollection Request { get; }
        IResponseCookies Response { get; }
    }
}
