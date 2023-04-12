using System.Security.Cryptography;
using System.Text;

namespace UserDomain.Encryption
{
    /// <summary>
    /// This class is trash. It "encrypts" by appending the key top the value and reversing the payload
    /// The only thing that this is useful for is garbling keys in a viewable way
    /// </summary>
    public class Encyptor : Common.Encryption.IEncryptor
    {
        private static Dictionary<Guid, Guid> _keys = new Dictionary<Guid, Guid>();

        public string Encrypt(Guid key, string value)
        {
            if (!_keys.ContainsValue(key))
            {
                return value;
            }
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return key.ToString() + new string(charArray);
        }

        public string Decrypt(Guid key, string value)
        {
            if (!_keys.ContainsValue(key))
            {
                return value;
            }

            if(!value.Contains(key.ToString()))
            {
                return value;
            }

            value = value.Replace(key.ToString(), "");
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);

        }

        public bool DeleteKey(Guid entityId)
        {
            return _keys.Remove(entityId);
        }

        public Guid GetKey(Guid entityId)
        {
            if (!_keys.ContainsKey(entityId))
            {
                _keys.Add(entityId, Guid.NewGuid());
            }

            return _keys[entityId];
        }
    }
}