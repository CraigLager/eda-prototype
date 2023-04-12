using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Common.Encryption;

namespace UserDomain.Entities
{
    public class UserRepository : Common.Repositories.IRepository<User>
    {
        private Common.Events.IMessageBus _messageBus;
        private IEncryptor _encryptor;

        public UserRepository(Common.Events.IMessageBus messageBus, Common.Encryption.IEncryptor encryptor)
        {
            this._messageBus = messageBus;
            this._encryptor = encryptor;
        }

        public User GetById(Guid id)
        {
            var events = _messageBus.Get<Common.Events.UserEvent>(new Common.Events.EventGetOptions() { Id = id });
            return new User(id, events, _encryptor);
        }

        public User GetById(Guid id, DateTime atDate)
        {
            var events = _messageBus.Get<Common.Events.UserEvent>(new Common.Events.EventGetOptions() { Id = id, EventsUntil = atDate });
            return new User(id, events, _encryptor);
        }

        public User GetById(Guid id, int version)
        {
            var events = _messageBus.Get<Common.Events.UserEvent>(new Common.Events.EventGetOptions() { Id = id, VersionUntil = version });
            return new User(id, events, _encryptor);
        }
    }
}
