using PowerArgs;
using System;
using TimeToggl.Actions;
using TimeToggl.Settings;

namespace TimeToggl.CommandLine
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ArgumentsApp
    {
        public ArgumentsApp()
        {

        }

        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Tells you when you can leave home")]
        public void Vcil()
        {
            RunAction(new WhenCanILeaveAction());
        }

        [ArgActionMethod, ArgDescription("Authenticate (username/password)")]
        public void AuthUsername()
        {
            RunAction(new AuthenticateAction());
        }

        [ArgActionMethod, ArgDescription("Starts a new time entry")]
        public void Start(StartArgs args)
        {
            RunAction(new StartAction(args));
        }

        [ArgActionMethod, ArgDescription("Stops a current time entry")]
        public void Stop(StopArgs args)
        {
            RunAction(new StopAction(args));
        }

        [ArgActionMethod, ArgDescription("Gets a list of user clients")]
        public void Clients()
        {
            RunAction(new ClientsAction());
        }

        [ArgActionMethod, ArgDescription("Gets a list of client projects")]
        public void Projects(ProjectsArgs args)
        {
            RunAction(new ProjectsAction(args));
        }

        private void RunAction(IAction action)
        {
            action.Run();
            action.PrintOutput();
        }
    }
}
