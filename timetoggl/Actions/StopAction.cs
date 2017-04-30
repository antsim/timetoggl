using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;

namespace TimeToggl.Actions
{
    class StopAction : BaseAction, IAction
    {
        private StopArgs _args;

        public StopAction(StopArgs args)
        {
            _args = args;
        }

        public void Run()
        {
            if (Authentication.UserAuth == null)
            {
                IAction action = new AuthenticateAction();
                action.Run();
            }

            string endpoint = string.Format(Endpoints.PUT.Stop, _args.TimeEntryId);

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.PutAsync(endpoint, new StringContent(""));

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);
            if (responseJson.Equals("null"))
            {
                return;
            }

            JObject json = JObject.Parse(responseJson);
            var duration = (int)json["data"]["duration"];
            var description = json["data"]["description"].ToString();

            var ts = TimeSpan.FromSeconds(duration);
            Output.Add($"Stopped time entry with description \"{description}\" and duration of {ts.ToString(@"hh\:mm\:ss")}");
        }
    }

    public class StopArgs
    {
        [ArgRequired, ArgDescription("Time entry ID"), ArgPosition(1)]
        public int TimeEntryId { get; set; }
    }
}
