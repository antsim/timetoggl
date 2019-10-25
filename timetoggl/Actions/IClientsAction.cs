using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    public interface IClientsAction
    {
        string Get(ClArguments arguments);
    }
}