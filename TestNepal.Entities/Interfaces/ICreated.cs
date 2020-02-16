using System;

namespace TestNepal.Entities.Common
{
    public interface ICreated
    {
        DateTime CreatedOn { get; set; }
        Guid CreatedById { get; set; }
    }
}