using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeToggl.Extensions;
namespace TimeToggl.Security
{
    public class CredentialsManager
    {

        public bool SetCredentials(UserPass up)
        {
            var cm = new Credential { Target = nameof(TimeToggl), PersistanceType = PersistanceType.Enterprise, Username = up.UserName, Password = up.Password.ToNonSecureString() };
            return cm.Save();
        }

        public void RemoveCredentials()
        {
            var cm = new Credential { Target = nameof(TimeToggl) };
            cm.Delete();
        }

        public UserPass GetCredentials()
        {
            var cm = new Credential { Target = nameof(TimeToggl) };
            if (!cm.Exists())
                return null;

            cm.Load();
            var up = new UserPass(cm);
            return up;
        }
    }
}
