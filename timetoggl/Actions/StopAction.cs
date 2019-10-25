using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Net.Http;
using System.Text;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    class StopAction : IStopAction
    {
        private StopArgs _args;

        public StopAction(StopArgs args)
        {
            _args = args;
        }

        public string Stop(ClArguments arguments)
        {
            if (Authentication.UserAuth == null)
            {
                // TODO: check vcil
            }

            var sb = new StringBuilder();

            string endpoint = string.Format(Endpoints.PUT.Stop, _args.TimeEntryId);

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.PutAsync(endpoint, new StringContent(""));

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);
            if (responseJson.Equals("null"))
            {
                return null;
            }

            try
            {
                JObject json = JObject.Parse(responseJson);
                var duration = (int)json["data"]["duration"];
                var description = json["data"]["description"].ToString();
                var ts = TimeSpan.FromSeconds(duration);
                sb.AppendLine($"Stopped time entry with description \"{description}\" and duration of {ts.ToString(@"hh\:mm\:ss")}");
            }
            catch (JsonReaderException)
            {
                sb.AppendLine(responseJson.Replace("\"", ""));
            }

            return sb.ToString();
        }
    }

    public class StopArgs
    {
        [ArgRequired, ArgDescription("Time entry ID"), ArgPosition(1)]
        public int TimeEntryId { get; set; }
    }
}
