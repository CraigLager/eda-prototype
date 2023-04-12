using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.UserData
{
    public class UserData
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public bool IsEncrypted { get; set; }

        public UserData ToEncrypted(Guid id, Encryption.IEncryptor encryptor)
        {
            return new UserData()
            {
                FirstName = encryptor.Encrypt(id, this.FirstName),
                LastName = encryptor.Encrypt(id, this.LastName),
                IsEncrypted = true
            };
        }

        public UserData DeEncrypt(Guid id, Encryption.IEncryptor encryptor)
        {
            return new UserData()
            {
                FirstName = encryptor.DeEncrypt(id, this.FirstName),
                LastName = encryptor.DeEncrypt(id, this.LastName),
                IsEncrypted = false
            };

        }
    }
}
