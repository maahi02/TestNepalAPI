using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Entities;
using TestNepal.Repository.Common;

namespace TestNepal.Repository.Infrastructure
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
    }
}
