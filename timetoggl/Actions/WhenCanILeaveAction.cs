using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    public class WhenCanILeaveAction : IWhenCanILeaveAction
    {
        private IAuthenticateActions _authenticateActions;

        public WhenCanILeaveAction(IAuthenticateActions authenticateActions)
        {
            _authenticateActions = authenticateActions;
        }

        public string WhenCanILeave()
        {
            return WhenCanILeave(null);
        }

        public string WhenCanILeave(ClArguments arguments)
        {
            if (Authentication.UserAuth == null)
            {
                _authenticateActions.Authenticate(null);
            }

            var sb = new StringBuilder();

            var startDate = Uri.EscapeDataString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz"));
            var endDate = Uri.EscapeDataString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz"));

            var endpoint = Endpoints.GET.TimeEntriesBetween;
            endpoint = $@"{endpoint}?start_date={startDate}&end_date={endDate}";

            using (var client =
                HttpClientFactory.GetClient(Authentication.UserAuth?.UserName, Authentication.UserAuth?.Password))
            {
                var response = client.GetAsync(endpoint);

                var responseJson = (response.Result.Content.ReadAsStringAsync().Result);
                if (responseJson.Equals("null"))
                {
                    return "ERROR";
                }

                var entries = JArray.Parse(responseJson);
                var totalDurationInSeconds = 0;

                foreach (var e in entries)
                {
                    var d = int.Parse(e["duration"].ToString());

                    if (d < 0)
                    {
                        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        var now = Convert.ToInt32(DateTime.UtcNow.Subtract(epoch).TotalSeconds);
                        d = now + d;
                    }

                    totalDurationInSeconds += d;
                }

                var officeDay = Convert.ToInt32(ConfigurationManager.AppSettings["OfficeDayDurationInSeconds"]);
                var timeLeft = officeDay - totalDurationInSeconds;

                if (timeLeft < 0)
                {
                    var overtime = TimeSpan.FromSeconds(Math.Abs(timeLeft));
                    sb.AppendLine($"You are on overtime of {overtime:hh\\:mm\\:ss}");
                }
                else
                {
                    var timeToLeave = DateTime.Now.AddSeconds(timeLeft);
                    var stillToGo = TimeSpan.FromSeconds(timeLeft);

                    sb.AppendLine($"You still have {stillToGo:hh\\:mm\\:ss} to go");
                    sb.AppendLine($"You can leave work at {timeToLeave.ToShortTimeString()}");
                }
            }

            return sb.ToString();
        }
    }
}
