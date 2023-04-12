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

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userData">Seed data</param>
        /// <returns>Created user</returns>
        public User Create(Common.UserData.UserData userData)
        {
            var user = new User(Guid.NewGuid()) { };
            return Apply(user, "CREATE", userData);
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">Users id</param>
        /// <param name="userData">New data</param>
        /// <returns>Updated user</returns>
        public User Update(Guid id, Common.UserData.UserData userData)
        {
            var user = _repo.GetById(id);
            return Apply(user, "UPDATE", userData);
        }

        /// <summary>
        /// Apply new user data to a user entity
        /// </summary>
        /// <param name="user">User to apply data to</param>
        /// <param name="message">Message for the event to be raised</param>
        /// <param name="userData">User data to apply</param>
        /// <returns></returns>
        private User Apply(User user, string message, Common.UserData.UserData userData)
        {
            var key = _encryptor.GetKey(user.Id);
            var @event = new Common.Events.UserEvent() { UserData = userData.ToEncrypted(key, this._encryptor), Id = user.Id, Message = message };
            user.Process(@event, this._encryptor);
            _messageBus.Publish(@event);

            return user;
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            var result = _encryptor.DeleteKey(id);
            _messageBus.Publish(new Common.Events.UserEvent() {Id = id, Message = "DELETE" });
            return result;
        }

    }
}
