using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    class ClientsAction : IClientsAction
    {
        public string Get(ClArguments arguments)
        {
            if (Authentication.UserAuth == null)
            {
                // TODO: Check vcil
            }

            var sb = new StringBuilder();

            var endpoint = Endpoints.GET.Clients;

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.GetAsync(endpoint);

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            JArray entries = JArray.Parse(responseJson);

            foreach (var e in entries)
            {
                int id = int.Parse(e["id"].ToString());
                string name = e["name"].ToString();

                sb.AppendLine($"Client {id}\t{name}");
            }

            return sb.ToString();
        }
    }
}
