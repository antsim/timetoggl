using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeToggl.API
{
    public static class Endpoints
    {
        public static class GET
        {
            public const string Me = @"https://www.toggl.com/api/v8/me";
            public const string TimeEntriesBetween = @"https://www.toggl.com/api/v8/time_entries";
            public const string Clients = @"https://www.toggl.com/api/v8/clients";
            public const string ClientProjects = @"https://www.toggl.com/api/v8/clients/{0}/projects";
        }
        
        public static class POST
        {
            public const string Start = @"https://www.toggl.com/api/v8/time_entries/start";
        }

        public static class PUT
        {
            public const string Stop = @"https://www.toggl.com/api/v8/time_entries/{0}/stop";
        }
    }
}
