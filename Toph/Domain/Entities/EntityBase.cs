using System;
using Toph.Common.DataAccess;

namespace Toph.Domain.Entities
{
    public abstract class EntityBase : Entity<int>
    {
        public virtual int Version { get; protected set; }
    }
}
