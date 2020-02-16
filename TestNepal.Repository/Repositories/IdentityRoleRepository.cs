
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestNepal.Repository.Repositories
{
    public class IdentityRoleRepository : RepositoryBase<IdentityRole>, IIdentityRoleRepository
    {
        public IdentityRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
