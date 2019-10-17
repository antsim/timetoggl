using System.Collections.Generic;
using TimeToggl.Extensions;

namespace TimeToggl.Actions
{
    public abstract class BaseAction
    {
        public List<string> Output { get; set; }

        protected BaseAction()
        {
            Output = new List<string>();
        }

        public void PrintOutput()
        {
            Output.ToConsole();
        }
    }
}
