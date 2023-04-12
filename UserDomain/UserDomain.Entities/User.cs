using UserDomain.Common.Encryption;

namespace UserDomain.Entities
{
    public class User
    {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public Guid Id { get; private set; }

        private IEncryptor _encryptor;

        public User(Guid id)
        {
            this.Id = id;
        }

        private User(Guid id, Common.Encryption.IEncryptor encryptor)
        {
            this.Id = id;
            this._encryptor = encryptor;
        }


        public User(Guid id,  Common.Events.IUserEvent @event, Common.Encryption.IEncryptor encryptor) : this(id, encryptor)
        {
            this.Process(@event);
        }

        public User(Guid id, IEnumerable<Common.Events.IUserEvent> @event, Common.Encryption.IEncryptor encryptor) : this(id, encryptor)
        {
            this.Process(@event);
        }

        private bool Process(Common.Events.IUserEvent @event)
        {
            if(@event == null)
            {
                return false;
            }

            if (@event.UserData != null)
            {
                var data = @event.UserData.IsEncrypted ? @event.UserData.DeEncrypt(@event.Id, this._encryptor) : @event.UserData;
                this.FirstName = data.FirstName ?? this.FirstName;
                this.LastName = data.LastName ?? this.LastName;
            }

            return true;
        }

        private bool Process(IEnumerable<Common.Events.IUserEvent> events)
        {
            foreach (var e in events)
            {
                Process(e);
            }

            return true;
        }
    }
}