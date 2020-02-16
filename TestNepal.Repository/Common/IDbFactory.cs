using TestNepal.Context;
using System;


namespace TestNepal.Repository.Common
{
    public interface IDbFactory:IDisposable
    {
        TestNepalContext Init();
    }
}
