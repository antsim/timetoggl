using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security;
using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.Helpers;
using TimeToggl.Security;
using TimeToggl.Extensions;

namespace TimeToggl.Actions
{
    public class AuthenticateAction : BaseAction, IAction
    {
        public void Run()
        {
            CredentialsManager cm = new CredentialsManager();
            var up = cm.GetCredentials();

            if (up != null)
            {
                Authentication.UserAuth = up;
                return;
            }
            
            string username = ConsoleHelper.GetConsoleInput("Username");
            SecureString password = ConsoleHelper.GetConsoleSecureInput("Password");

            var client = HttpClientFactory.GetClient(username, password);
            var response = client.GetAsync(Endpoints.GET.Me);
            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            if (!string.IsNullOrEmpty(responseJson))
            {
                cm.SetCredentials(new UserPass() { UserName = username, Password = password });

                Output.Add("Authentication successful");
            }
            else
            {
                Output.Add("Authentication failed. Check your username and password!");
            }
        }
    }
}
