using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Common.Encryption;
using UserDomain.Common.Events;
using UserDomain.Common.Repositories;

namespace UserDomain.Entities
{
    public class UserService
    {
        private IMessageBus _messageBus;
        private IRepository<User> _repo;
        private IEncryptor _encryptor;

        public UserService(Common.Events.IMessageBus messageBus, Common.Repositories.IRepository<User> repo, Common.Encryption.IEncryptor encryptor)
        {
            this._messageBus = messageBus;
            this._repo = repo;
            this._encryptor = encryptor;
        }

        public User Create(Common.UserData.UserData userData)
        {
            var user = new User(Guid.NewGuid()) { };
            user.FirstName = userData.FirstName ?? user.FirstName;
            user.LastName = userData.LastName ?? user.LastName;

            _messageBus.Publish(new Common.Events.UserEvent() { UserData = userData.ToEncrypted(user.Id, this._encryptor), Id = user.Id, Message = "CREATE" });
            return user;
        }

        public User Update(Guid id, Common.UserData.UserData userData)
        {
            var user = _repo.GetById(id);
            user.FirstName = userData.FirstName ?? user.FirstName;
            user.LastName = userData.LastName ?? user.LastName;

            _messageBus.Publish(new Common.Events.UserEvent() { UserData = userData.ToEncrypted(user.Id, this._encryptor), Id = user.Id, Message = "UPDATE" });
            return user;
        }

        public bool Delete(Guid id)
        {
            var result = _encryptor.DeleteKey(id);
            _messageBus.Publish(new Common.Events.UserEvent() {Id = id, Message = "DELETE" });
            return result;
        }

    }
}
