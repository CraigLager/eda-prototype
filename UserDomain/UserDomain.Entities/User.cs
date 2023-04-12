using UserDomain.Common.Encryption;

namespace UserDomain.Entities
{
    public class User
    {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public Guid Id { get; private set; }

        public User(Guid id)
        {
            this.Id = id;
        }

        public User(Guid id,  Common.Events.IUserEvent @event, Common.Encryption.IEncryptor encryptor) : this(id)
        {
            this.Process(@event, encryptor);
        }

        public User(Guid id, IEnumerable<Common.Events.IUserEvent> @event, Common.Encryption.IEncryptor encryptor) : this(id)
        {
            foreach (var e in @event)
            {
                this.Process(e, encryptor);
            }
        }

        internal bool Process(Common.Events.IUserEvent @event, IEncryptor encryptor)
        {
            if(@event == null)
            {
                return false;
            }

            if (@event.UserData != null)
            {
                var key = encryptor.GetKey(this.Id);
                var data = @event.UserData.ToDecrypted(key, encryptor);
                this.FirstName = data.FirstName ?? this.FirstName;
                this.LastName = data.LastName ?? this.LastName;
            }

            return true;
        }
    }
}