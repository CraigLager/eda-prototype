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

        public UserData ToEncrypted(Guid encryptionKey, Encryption.IEncryptor encryptor)
        {
            if(this.IsEncrypted)
            {
                return this;
            }

            return new UserData()
            {
                FirstName = encryptor.Encrypt(encryptionKey, this.FirstName),
                LastName = encryptor.Encrypt(encryptionKey, this.LastName),
                IsEncrypted = true
            };
        }

        public UserData ToDecrypted(Guid encryptionKey, Encryption.IEncryptor encryptor)
        {
            if (!this.IsEncrypted)
            {
                return this;
            }

            return new UserData()
            {
                FirstName = encryptor.Decrypt(encryptionKey, this.FirstName),
                LastName = encryptor.Decrypt(encryptionKey, this.LastName),
                IsEncrypted = false
            };

        }
    }
}
