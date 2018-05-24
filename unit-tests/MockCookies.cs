using System;
using Microsoft.AspNetCore.Http;
using project_fob.Controllers;
using Moq;

namespace unit_tests
{
    public class MockCookies : ICookies
    {
        public MockCookies()
        {
            MockRequest = new Mock<IRequestCookieCollection>();
            MockResponse = new Mock<IResponseCookies>();
        }

        public Mock<IRequestCookieCollection> MockRequest { get; set; }
        public Mock<IResponseCookies> MockResponse { get; set; }

        public IRequestCookieCollection Request => MockRequest.Object;

        public IResponseCookies Response => MockResponse.Object;
    }
}

