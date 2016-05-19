using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.CommandLine;

namespace timetoggl
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<AppArguments>(args);
        }
    }
}
