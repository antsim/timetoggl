using CredentialManagement;
using System.Security;
using TimeToggl.Extensions;

namespace TimeToggl.Security
{
    public class UserPass
    {
        public UserPass()
        {
        }

        public UserPass(Credential c)
        {
            UserName = c.Username;
            Password = c.Password.ToSecureString();
        }
        public string UserName { get; set; }
        public SecureString Password { get; set; }
    }
}
