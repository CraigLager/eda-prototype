namespace UserDomain.Common.Events
{
    public interface IMessageBus
    {
        public bool Publish(IUserEvent @event);
        public List<T> Get<T>(IEventGetOptions options);
        public event EventHandler<IUserEvent> OnUserEvent;
    }
}