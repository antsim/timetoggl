using TimeToggl.API;
using TimeToggl.Client;
using TimeToggl.CommandLine;
using TimeToggl.Helpers;
using TimeToggl.Security;

namespace TimeToggl.Actions
{
    public class AuthenticateActions : IAuthenticateActions
    {
        public string Authenticate(ClArguments arguments)
        {

            var cm = new CredentialsManager();
            var up = cm.GetCredentials();

            if (up != null)
            {
                Authentication.UserAuth = up;
                return $"Authenticated: {Authentication.UserAuth.UserName}";
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

                    return "Authentication successful";
                }
                else
                {
                    return "Authentication failed. Check your username and password!";
                }
            }
        }
    }
}
