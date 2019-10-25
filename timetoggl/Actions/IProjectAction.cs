using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    public interface IProjectAction
    {
        string GetAll(ClArguments arguments);
    }
}