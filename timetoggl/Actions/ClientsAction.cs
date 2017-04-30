using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.API;
using TimeToggl.Client;

namespace TimeToggl.Actions
{
    class ClientsAction : BaseAction, IAction
    {
        public void Run()
        {
            if (Authentication.UserAuth == null)
            {
                IAction action = new AuthenticateAction();
                action.Run();
            }

            var endpoint = Endpoints.GET.Clients;

            var client = HttpClientFactory.GetClient(Authentication.UserAuth.UserName, Authentication.UserAuth.Password);
            var response = client.GetAsync(endpoint);

            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            JArray entries = JArray.Parse(responseJson);

            foreach (var e in entries)
            {
                int id = int.Parse(e["id"].ToString());
                string name = e["name"].ToString();

                Output.Add($"Client {id}\t{name}");
            }
        }
    }
}
