using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using project_fob.Controllers;
using FluentAssertions;

namespace unit_tests
{
    public class IDGeneratorsTest
    {
        [Fact]
        public void GenerateGuidId()
        {
            string guid = IDGenerators.GenerateId();
            Assert.NotEmpty(guid);
        }

        [Fact]
        public void GenerateMeetingId()
        {
            string meetingId = IDGenerators.GenerateMeetingId();
            Assert.NotEmpty(meetingId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(7)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(2)]
        public void GenerateUnambiguousMeetingIdByLength(int length)
        {
            string meetingId = IDGenerators.GenerateUnambiguousMeetingIdByLength(length);
            meetingId.Length.Should().Be(length);
        }

        


    }
}
