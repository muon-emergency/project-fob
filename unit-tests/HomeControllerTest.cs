using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using project_fob.Controllers;
using Xunit;

namespace unit_tests
{
    public partial class HomeControllerTest
    {
        [Theory]
        [InlineData("hey", "hey", false)]
        [InlineData("", "", false)]
        [InlineData("", null, false)]
        [InlineData("asd", "obvious", true)]
        [InlineData(null, "Hello there", true)]
        public void CreateMeetingPasswordCheck(string attendeePassword, string hostPassword, bool expected)
        {
            var Result = HomeController.CheckPasswordsAreCorrectForHosting(attendeePassword, hostPassword);

            Result.Should().Be(expected);
        }
    }
}
