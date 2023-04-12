using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Events
{
    public class UserEvent : IUserEvent
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime DateAdded { get; set; }

        public UserData.UserData UserData { get; set; }
        public int Version { get; set; }
    }
}
