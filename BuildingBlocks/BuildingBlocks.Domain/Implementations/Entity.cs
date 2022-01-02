using System;
using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain.Interfaces;

namespace BuildingBlocks.Domain.Implementations
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = Comb.Create();
        }

        [Key] public Guid Id { get; set; }
    }
}