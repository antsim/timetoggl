using TimeToggl.CommandLine;

namespace TimeToggl.Actions
{
    public interface IWhenCanILeaveAction
    {
        string WhenCanILeave();
        string WhenCanILeave(ClArguments arguments);
    }
}