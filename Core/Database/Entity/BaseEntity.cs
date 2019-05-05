using System;
using System.ComponentModel.DataAnnotations;
using MicroOrm.Dapper.Repositories.Attributes;
using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;

namespace HomeCTRL.Backend.Core.Database.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}