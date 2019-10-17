using PowerArgs;
using System;
using TimeToggl.CommandLine;

namespace timetoggl
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<ArgumentsApp>(args);
            Console.ReadKey();
        }
    }
}
