using System;

namespace BuildingBlocks.Domain.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}