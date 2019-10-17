using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.Helpers;
using TimeToggl.Security;

namespace TimeToggl.Actions
{
    public class AuthenticateAction : BaseAction, IAction
    {
        public void Run()
        {
            var cm = new CredentialsManager();
            var up = cm.GetCredentials();

            if (up != null)
            {
                Authentication.UserAuth = up;
                return;
            }
            
            var username = ConsoleHelper.GetConsoleInput("Username");
            var password = ConsoleHelper.GetConsoleSecureInput("Password");

            using (var client = HttpClientFactory.GetClient(username, password))
            {
                var response = client.GetAsync(Endpoints.GET.Me);
                var responseJson = (response.Result.Content.ReadAsStringAsync().Result);

                if (!string.IsNullOrEmpty(responseJson))
                {
                    var newUp = new UserPass { UserName = username, Password = password };

                    cm.SetCredentials(newUp);
                    Authentication.UserAuth = newUp;

                    Output.Add("Authentication successful");
                }
                else
                {
                    Output.Add("Authentication failed. Check your username and password!");
                }
            }
        }
    }
}
