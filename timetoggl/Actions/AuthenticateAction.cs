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
    public class AuthenticateAction : IAction
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
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            SecureString password = ConsoleHelper.GetConsoleSecurePassword();

            var client = HttpClientFactory.GetClient(username, password);
            var response = client.GetAsync(Endpoints.Me);
            string responseJson = (response.Result.Content.ReadAsStringAsync().Result);

            if (!string.IsNullOrEmpty(responseJson))
            {
                cm.SetCredentials(new UserPass() { UserName = username, Password = password });

                Console.WriteLine("Authentication successful");
            }
            else
            {
                Console.WriteLine("Authentication failed. Check your username and password!");
            }
        }
    }
}
