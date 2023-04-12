using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Events
{
    public class EventGetOptions : IEventGetOptions
    {
        public Guid? Id { get; set; }
        public DateTime? EventsUntil { get; set; }
        public int? VersionUntil { get; set; }
    }
}
