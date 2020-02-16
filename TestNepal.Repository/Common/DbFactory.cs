using TestNepal.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Repository.Common
{
    public class DbFactory: Disposable,IDbFactory
    {
        TestNepalContext dbContext;
        private readonly Guid _tenantId;
        private readonly Guid _userId;
        public DbFactory()
        {

        }
        public DbFactory(Guid tenantId, Guid userId)
        {
            _tenantId = tenantId;
            _userId = userId;
        }

        public TestNepalContext Init()
        {
            return dbContext ?? (dbContext = new TestNepalContext(_tenantId, _userId));
        }
        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
