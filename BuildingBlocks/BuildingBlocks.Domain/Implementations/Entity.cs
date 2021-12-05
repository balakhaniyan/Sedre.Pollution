using System;
using BuildingBlocks.Domain.Interfaces;

namespace BuildingBlocks.Domain.Implementations
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = Comb.Create();
        }

        public Guid Id { get; set; }
    }
}