using System;
using project_fob.Controllers;
using Xunit;
using FluentAssertions;

namespace unit_tests
{
    public class QrCodeUrlBuilderests
    {
        [Theory]
        [InlineData("meeting-id", "http://localhost:53612/Host/QrCode", "123456", "http://localhost:53612/Home/meetingPageUser?meetingId=meeting-id&password=123456")]
        [InlineData("meeting-id", "http://localhost:53612/Host/QrCode", "", "http://localhost:53612/Home/meetingPageUser?meetingId=meeting-id&password=")]
        [InlineData("meeting-id", "http://localhost:53612/myprojects/personal/csharp/Host/QrCode", "123456", "http://localhost:53612/myprojects/personal/csharp/Home/meetingPageUser?meetingId=meeting-id&password=123456")]
        [InlineData("meeting-id", "http://localhost/myprojects/personal/csharp/Host/QrCode", "123456", "http://localhost/myprojects/personal/csharp/Home/meetingPageUser?meetingId=meeting-id&password=123456")]
        public void CreateUrlTests(string meetingId, string baseUrl, string roomPassword, string expected)
        {
            var meetingUrl = QrCodeUrlBuilder.BuildUrl(meetingId, baseUrl, roomPassword);

            meetingUrl.Should().Be(expected);
        }
    }
}
