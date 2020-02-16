using System;

namespace TestNepal.Entities.Common
{
    public interface ISecuredByUser
    {
        Guid UserId { get; set; }
    }
}