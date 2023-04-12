using UserDomain.Common.Events;

namespace UserDomain.MessageBus
{
    public class MessageBus : Common.Events.IMessageBus
    {
        private List<IUserEvent> _messageBus = new List<IUserEvent>();

        /// <summary>
        /// Builds a query against options for getting events from the bus
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private IOrderedQueryable<IUserEvent> BuildQuery(IEventGetOptions options)
        {
            var allObjectsQuery = _messageBus.AsQueryable();

            if (options.Id.HasValue)
            {
                allObjectsQuery = allObjectsQuery.Where(o => o.Id == options.Id);
            }

            if (options.EventsUntil.HasValue)
            {
                allObjectsQuery = allObjectsQuery.Where(oq => oq.DateAdded <= options.EventsUntil);
            }

            if (options.VersionUntil.HasValue)
            {
                allObjectsQuery = allObjectsQuery.Where(oq => oq.Version <= options.VersionUntil.Value);
            }

            return allObjectsQuery
                .OrderBy(x => x.DateAdded);
        }

        public List<T> Get<T>(IEventGetOptions options)
        {
            return BuildQuery(options)
                .Select(o => (T)Convert.ChangeType(o, typeof(T)))
                .ToList();
        }

        /// <summary>
        /// Publishes a new event to the bus
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public bool Publish(IUserEvent @event)
        {
            @event.DateAdded = DateTime.UtcNow;
            @event.Version = _messageBus.Count(o => o.Id == @event.Id) + 1;
            _messageBus.Add(@event);

            // let any subscribers know that something has happened
            if(OnUserEvent != null)
            {
                OnUserEvent(this, @event);
            }

            return true;
        }

        public event EventHandler<IUserEvent> OnUserEvent;
    }
}