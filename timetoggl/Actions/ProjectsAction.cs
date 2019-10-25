using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.CommandLine;
using TimeToggl.Model;

namespace TimeToggl.Actions
{
    class ProjectsAction : IProjectAction
    {
        private ProjectsArgs _args;
        public ProjectsAction(ProjectsArgs args)
        {
            _args = args;
        }

        public string GetAll(ClArguments arguments)
        {
            if (Authentication.UserAuth == null)
            {
                // TODO: Check vcil example
            }

            var sb = new StringBuilder();

            var endpoint = string.Format(Endpoints.GET.ClientProjects, _args.ClientId);

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.GetAsync(endpoint);

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            if (responseJson.Equals("null"))
            {
                sb.AppendLine($"No projects found for client {_args.ClientId}");
                return sb.ToString();
            }

            var projects = new List<Project>();
            JArray entries = JArray.Parse(responseJson);
            foreach (var e in entries)
            {
                var active = bool.Parse(e["active"].ToString());
                if (!active) continue;

                var id = (int)e["id"];
                var name = e["name"].ToString();

                projects.Add(new Project { Id = id, Name = name });
                sb.AppendLine($"Project {id}\t{name}");
            }

            var projectJson = JsonConvert.SerializeObject(projects);
            File.WriteAllText("projects.json", projectJson);

            return sb.ToString();
        }
    }

    public class ProjectsArgs
    {
        [ArgRequired, ArgDescription("ClientId ID"), ArgPosition(1)]
        public int ClientId { get; set; }
    }
}
