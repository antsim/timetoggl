using System;
using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    public interface IAuthenticateActions
    {
        string Authenticate(ClArguments arguments);
    }
}