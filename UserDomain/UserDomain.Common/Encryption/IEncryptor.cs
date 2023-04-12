using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Encryption
{
    public interface IEncryptor
    {
        string DeEncrypt(Guid entityId, string value);
        bool DeleteKey(Guid entityId);
        string Encrypt(Guid entityId, string value);
    }
}
