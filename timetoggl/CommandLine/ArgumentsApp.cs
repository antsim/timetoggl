using PowerArgs;
using System;
using System.Runtime.InteropServices;
using TimeToggl.Actions;
using TimeToggl.Settings;

namespace TimeToggl.CommandLine
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ArgumentsApp
    {
        private IAuthenticateActions _authenticateActions;

        public ArgumentsApp()
        {
            
        }

        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Tells you when you can leave home")]
        public void Vcil()
        {
            RunAction(_authenticateActions.Authenticate, null);
        }

        [ArgActionMethod, ArgDescription("Authenticate (username/password)")]
        public void AuthUsername()
        {
            
        }

        [ArgActionMethod, ArgDescription("Starts a new time entry")]
        public void Start(StartArgs args)
        {
            
        }

        [ArgActionMethod, ArgDescription("Stops a current time entry")]
        public void Stop(StopArgs args)
        {
            
        }

        [ArgActionMethod, ArgDescription("Gets a list of user clients")]
        public void Clients()
        {
            
        }

        [ArgActionMethod, ArgDescription("Gets a list of client projects")]
        public void Projects(ProjectsArgs args)
        {
            
        }

        private void RunAction(Func<ClArguments, string> actionMethod, ClArguments arguments)
        {
            actionMethod.Invoke(arguments).ToConsoleString();
        }
    }
}
