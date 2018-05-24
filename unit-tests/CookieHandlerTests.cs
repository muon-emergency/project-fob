using project_fob.Controllers;
using Xunit;

namespace unit_tests
{
    public partial class CookieHandlerTests
    {
        [Fact]
        public void dfsdf()
        {
            var testCookies = new MockCookies();

            CookieHandler.CreateOrUpdateCookies(testCookies);

            testCookies.MockResponse.Verify(x => x.Append("ID", Moq.It.IsAny<string>()), Moq.Times.Once);
        }
    }
}
