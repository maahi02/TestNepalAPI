using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestNepal.Entities;
using TestNepal.Repository.Common;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestNepal.Repository.Infrastructure
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole>
    {
    }
}
