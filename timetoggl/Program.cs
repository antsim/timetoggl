using PowerArgs;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using TimeToggl.Actions;
using TimeToggl.CommandLine;

namespace timetoggl
{
    class TimeTogglConsoleApp
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IAuthenticateActions, AuthenticateActions>()
                .AddSingleton<IWhenCanILeaveAction, WhenCanILeaveAction>()
                .AddTransient<TimeTogglConsoleApp>()
                .BuildServiceProvider();

            //configure console logging
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("timetoggl.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<TimeTogglConsoleApp>();
            logger.LogInformation("Example log message");
            serviceProvider.GetService<TimeTogglConsoleApp>().Run(args);
        }

        public void Run(string[] args)
        {
            Args.InvokeAction<ArgumentsApp>(args);
            Console.ReadKey();
        }
    }
}
