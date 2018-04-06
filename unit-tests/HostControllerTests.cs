using System;
using project_fob.Controllers;
using Xunit;
using FluentAssertions;

namespace unit_tests
{
    public class HostControllerTests
    {
        [Fact]
        public void CreateUrlTests()
        {
            string meetingId = "meeting-id";
            const string baseUrl = "http://localhost:53612/Host/QrCode";
            const string roomPassword = "123456";

            var meetingUrl = HostController.CreateUrlForTesting(meetingId, baseUrl, roomPassword);

            meetingUrl.Should().Be("http://localhost:53612/Home/meetingPageUser?meetingId=meeting-id&password=123456");
        }

        [Fact]
        public void CreateUrlFromLongUrlTests()
        {
            string meetingId = "meeting-id";
            const string baseUrl = "http://localhost:53612/myprojects/personal/csharp/Host/QrCode";
            const string roomPassword = "123456";

            var meetingUrl = HostController.CreateUrlForTesting(meetingId, baseUrl, roomPassword);

            meetingUrl.Should().Be("http://localhost:53612/myprojects/personal/csharp/Home/meetingPageUser?meetingId=meeting-id&password=123456");
        }

    }
}
