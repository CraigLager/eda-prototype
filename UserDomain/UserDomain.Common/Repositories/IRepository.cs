using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Common.Repositories
{
    public interface IRepository<T>
    {
        public T GetById(Guid id);

        public T GetById(Guid id, DateTime atDate);
        public T GetById(Guid id, int version);
    }
}
