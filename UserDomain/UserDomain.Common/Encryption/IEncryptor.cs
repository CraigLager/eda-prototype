using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Encryption
{
    public interface IEncryptor
    {
        Guid GetKey(Guid entityId);
        string Decrypt(Guid key, string value);
        bool DeleteKey(Guid entityId);
        string Encrypt(Guid guid, string value);
    }
}
