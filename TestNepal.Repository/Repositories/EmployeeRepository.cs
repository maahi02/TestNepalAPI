
using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;

namespace TestNepal.Repository.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
