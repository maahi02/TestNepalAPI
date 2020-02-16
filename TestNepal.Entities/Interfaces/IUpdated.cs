using System;

namespace TestNepal.Entities.Common
{
    public interface IUpdated
    {
        DateTime? ModifiedOn { get; set; }
        Guid? ModifiedById { get; set; }
    }
}