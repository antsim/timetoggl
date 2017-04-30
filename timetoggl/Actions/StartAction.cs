using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;

namespace TimeToggl.Actions
{
    class StartAction : BaseAction, IAction
    {
        private StartArgs _args;

        public StartAction(StartArgs args)
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

            dynamic content = new ExpandoObject();
            content.time_entry = new ExpandoObject();
            content.time_entry.description = _args.Description;
            content.time_entry.pid = _args.ProjectId;
            content.time_entry.created_with = "TimeToggle";
            
            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);

            var response = client.PostAsync(Endpoints.POST.Start, new StringContent(JsonConvert.SerializeObject(content)));
            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            var json = JObject.Parse(responseJson);
            var id = (int)json["data"]["id"];

            Output.Add($"Time entry started with ID {id}");
        }
    }

    public class StartArgs
    {
        [ArgRequired, ArgDescription("Description of the time entry"), ArgPosition(1)]
        public string Description { get; set; }

        [ArgRequired, ArgDescription("ID of the project"), ArgPosition(2)]
        public int ProjectId { get; set; }
    }
}
