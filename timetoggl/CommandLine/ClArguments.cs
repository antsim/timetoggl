using System.Collections.Generic;

namespace TimeToggl.CommandLine
{
    public class ClArguments
    {
        public ClArguments()
        {
            Arguments = new List<string>();
        }

        public List<string> Arguments { get; set; }
    }
}