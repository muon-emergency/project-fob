using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_fob.Controllers
{
    public static class IDGenerators
    {
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GenerateMeetingId()
        {
            return GenerateUnambiguousMeetingIdByLength(6);
        }

        public static string GenerateUnambiguousMeetingIdByLength(int length)
        {
            Random random = new Random();
            const string chars = "367CDFGHJKMNPRTWX";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
