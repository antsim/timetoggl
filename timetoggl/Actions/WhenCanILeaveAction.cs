using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using TimeToggl.API;
using TimeToggl.Client;

namespace TimeToggl.Actions
{
    public class WhenCanILeaveAction : BaseAction, IAction
    {
        public void Run()
        {
            if (Authentication.UserAuth == null)
            {
                IAction action = new AuthenticateAction();
                action.Run();
            }

            var startDate = Uri.EscapeDataString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz"));
            var endDate = Uri.EscapeDataString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz"));

            // Validate token by calling me endpoint
            string endpoint = Endpoints.TimeEntriesBetween;
            endpoint = $@"{endpoint}?start_date={startDate}&end_date={endDate}";

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.GetAsync(endpoint);

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);
            if (string.IsNullOrEmpty(responseJson))
            {
                return;
            }

            JArray entries = JArray.Parse(responseJson);
            int totalDurationInSeconds = 0;

            foreach (var e in entries)
            {
                int d = int.Parse(e["duration"].ToString());

                if (d < 0)
                {
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    int now = Convert.ToInt32(DateTime.UtcNow.Subtract(epoch).TotalSeconds);
                    d = now + d;
                }

                totalDurationInSeconds += d;
            }

            int officeDay = Convert.ToInt32(ConfigurationManager.AppSettings["OfficeDayDurationInSeconds"].ToString());
            int timeLeft = officeDay - totalDurationInSeconds;

            if (timeLeft < 0)
            {
                TimeSpan overtime = TimeSpan.FromSeconds(Math.Abs(timeLeft));
                Output.Add($"You are on overtime of {overtime.ToString(@"hh\:mm\:ss")}");
            }
            else
            {
                DateTime timeToLeave = DateTime.Now.AddSeconds(timeLeft);
                TimeSpan stillToGo = TimeSpan.FromSeconds(timeLeft);

                Output.Add($"You still have {stillToGo.ToString(@"hh\:mm\:ss")} to go");
                Output.Add($"You can leave work at {timeToLeave.ToShortTimeString()}");
            }
        }
    }
}
