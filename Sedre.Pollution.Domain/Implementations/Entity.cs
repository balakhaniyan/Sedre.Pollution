using System;
using Sedre.Pollution.Domain.Interfaces;

namespace Sedre.Pollution.Domain.Implementations
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