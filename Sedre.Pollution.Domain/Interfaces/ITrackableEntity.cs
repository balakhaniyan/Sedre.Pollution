﻿using System;

namespace Sedre.Pollution.Domain.Interfaces
{
    public interface ITrackableEntity : IEntity, ISoftDeletable
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
    }
}