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
using TimeToggl.CommandLine;
using TimeToggl.Extensions;
using TimeToggl.Model;

namespace TimeToggl.Actions
{
    class StartAction : IStartAction
    {
        private StartArgs _args;

        public StartAction(StartArgs args)
        {
            _args = args;
        }

        public string Start(ClArguments arguments)
        {
            if (Authentication.UserAuth == null)
            {
                // TODO: Check vcil
            }

            var sb = new StringBuilder();

            var projectId = _args.ProjectId;
            if (!string.IsNullOrWhiteSpace(_args.ProjectName))
            {
                if (!File.Exists("projects.json"))
                {
                    sb.AppendLine("Projects cache did not exist. You can only use project name search after retrieving projects list.");
                    return sb.ToString();
                }

                var projects = JsonConvert.DeserializeObject<List<Project>>(File.ReadAllText("projects.json"));
                var project = projects.FirstOrDefault(p => p.Name.Contains(_args.ProjectName, StringComparison.OrdinalIgnoreCase));

                if (project != null)
                {
                    projectId = project.Id;
                    sb.AppendLine($"Matched project name to project: {project.Name}");
                }
                else
                {
                    sb.AppendLine("Given project name did not match any of the cached projects");
                    return sb.ToString();
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

            sb.AppendLine($"Time entry started with ID {id}");
            return sb.ToString();
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
