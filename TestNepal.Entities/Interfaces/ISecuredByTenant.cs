using System;

namespace TestNepal.Entities.Common
{
    public interface ISecuredByTenant
    {
        Guid TenantId { get; set; }
    }
}