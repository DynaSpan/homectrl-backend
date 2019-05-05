using System;
using MicroOrm.Dapper.Repositories.Attributes;
using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;

namespace HomeCTRL.Backend.Core.Database.Entity
{
    public abstract class Entity : BaseEntity
    {
        [UpdatedAt]
        public DateTime? LastEditDate { get; set; }

        [Deleted]
        public bool Deleted { get; set; }
    }
}