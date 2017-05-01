using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.Extensions;
using TimeToggl.Model;

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

            var projectId = _args.ProjectId;
            if (!string.IsNullOrWhiteSpace(_args.ProjectName))
            {
                if (!File.Exists("projects.json"))
                {
                    Output.Add("Projects cache did not exist. You can only use project name search after retrieving projects list.");
                    return;
                }

                var projects = JsonConvert.DeserializeObject<List<Project>>(File.ReadAllText("projects.json"));
                var project = projects.FirstOrDefault(p => p.Name.Contains(_args.ProjectName, StringComparison.OrdinalIgnoreCase));

                if (project != null)
                {
                    projectId = project.Id;
                    Output.Add($"Matched project name to project: {project.Name}");
                }
                else
                {
                    Output.Add("Given project name did not match any of the cached projects");
                    return;
                }
            }

            dynamic content = new ExpandoObject();
            content.time_entry = new ExpandoObject();
            content.time_entry.description = _args.Description;
            content.time_entry.pid = projectId;
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

        [ArgDescription("ID of the project"), ArgShortcut("-pid")]
        public int ProjectId { get; set; }

        [ArgDescription("Name of the project"), ArgShortcut("-p")]
        public string ProjectName { get; set; }
    }
}
