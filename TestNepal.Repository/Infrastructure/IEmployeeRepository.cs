using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Repository.Common;

namespace TestNepal.Repository.Infrastructure
{
    public interface IEmployeeRepository : IRepository<Entities.Employee>
    {
    }
}
