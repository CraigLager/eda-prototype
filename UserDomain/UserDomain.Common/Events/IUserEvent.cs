using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Events
{
    public interface IUserEvent
    {
        public Guid Id { get; }
        public string Message { get; }
        public DateTime DateAdded { get; set; }
        public int Version { get; set; }

        public UserData.UserData UserData { get; set; }
    }
}
