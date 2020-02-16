using System;

namespace TestNepal.Entities.Common
{
    public interface IDeleted
    {
        DateTime? DeletedDate { get; set; }
        Guid? DeletedById { get; set; }
    }
}