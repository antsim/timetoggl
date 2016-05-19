using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.Helpers;
using TimeToggl.Settings;

namespace TimeToggl.CommandLine
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class AppArguments
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Tells you when you can leave home")]
        public void Vcil()
        {

        }

        [ArgActionMethod, ArgDescription("Authenticate")]
        public void Auth()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            SecureString password = ConsoleHelper.GetConsoleSecurePassword();

            // TODO
            // Call /me endpoint and store the API token to settings file
        }

        [ArgActionMethod, ArgDescription("Set API token")]
        public void set_token()
        {
            Console.Write("API Token: ");
            string token = Console.ReadLine();

            SettingsManager sm = new SettingsManager();
            sm.AddUpdateAppSettings("API_token", token);
        }
    }
}
