using PowerArgs;
using System;
using TimeToggl.Actions;
using TimeToggl.Settings;

namespace TimeToggl.CommandLine
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ArgumentsApp
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Tells you when you can leave home")]
        public void Vcil()
        {
            IAction action = new WhenCanILeaveAction();
            action.Run();
            action.PrintOutput();
        }

        [ArgActionMethod, ArgDescription("Authenticate (username/password)")]
        public void AuthUsername()
        {
            IAction action = new AuthenticateAction();
            action.Run();
            action.PrintOutput();
        }
    }
}
