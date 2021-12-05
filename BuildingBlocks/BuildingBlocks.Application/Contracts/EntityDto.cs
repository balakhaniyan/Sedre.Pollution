﻿using System;

namespace BuildingBlocks.Application.Contracts
{
    public abstract class EntityDto<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}