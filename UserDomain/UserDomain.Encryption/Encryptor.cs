using System.Security.Cryptography;
using System.Text;

namespace UserDomain.Encryption
{
    public class Encyptor : Common.Encryption.IEncryptor
    {
        private static Dictionary<Guid, string> _keys = new Dictionary<Guid, string>();

        public string Encrypt(Guid entityId, string value)
        {
            if (!_keys.ContainsKey(entityId))
            {
                _keys.Add(entityId, Guid.NewGuid().ToString());
            }

            var result = _keys[entityId] + value;
            return result;
        }

        public string DeEncrypt(Guid entityId, string value)
        {
            if (!_keys.ContainsKey(entityId))
            {
                return value;
            }

            return value.Replace(_keys[entityId], "");
        }

        public bool DeleteKey(Guid entityId)
        {
            return _keys.Remove(entityId);
        }
    }
}