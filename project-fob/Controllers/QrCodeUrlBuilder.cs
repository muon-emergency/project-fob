using System.Text;

namespace project_fob.Controllers
{
    public class QrCodeUrlBuilder
    {
        public static string BuildUrl(string meetingIdString, string currentUrl, string roomPassword)
        {
            string[] split = currentUrl.Split('/');

            // This variable is really confusing so I'll explain.
            // Baseurl doesn't work because the url can be a long string which I could not use to correctly find the required parameters to enter to the meeting.
            // In case we are running the project on a link like : test.co.uk/project/seesharp/fob/
            // the url request will return something like this: test.co.uk/project/seesharp/fob/index/hostpage. Because of that I have to modify the
            // URL so it grabs the correct url (hopefully).
            // The url editing also needed because of the controll handling it'd generate a wrong url for the user which would render the QRCode useless.

            StringBuilder currentUrlLocationBase = new StringBuilder();

            for (int i = 0; i < split.Length - 2; i++)
            {
                currentUrlLocationBase.Append(split[i] + "/");
            }

            //The string (url) we generate.
            return currentUrlLocationBase.ToString() + "Home/meetingPageUser?meetingId=" + meetingIdString + "&password=" + roomPassword;
        }
    }
}
