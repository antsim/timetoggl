using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.Extensions;

namespace TimeToggl.Actions
{
    public abstract class BaseAction
    {
        public List<string> Output { get; set; }

        public BaseAction()
        {
            Output = new List<string>();
        }

        public void PrintOutput()
        {
            Output.ToConsole();
        }
    }
}
