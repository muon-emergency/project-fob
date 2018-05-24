using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace project_fob.Controllers
{
    public class RealCookies : ICookies
    {
        private readonly ControllerBase controller;

        public RealCookies(ControllerBase controller)
        {
            this.controller = controller;
        }

        public IRequestCookieCollection Request => controller.Request.Cookies;

        public IResponseCookies Response => controller.Response.Cookies;
    }
}
