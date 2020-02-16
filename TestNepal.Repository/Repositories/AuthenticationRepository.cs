
using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;

namespace TestNepal.Repository.Repositories
{
    public class AuthenticationRepository : RepositoryBase<Authentication>, IAuthenticationRepository
    {
        public AuthenticationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
