﻿using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class GetPreviousDayData: Specification<HourIndicator>
    {

        private readonly int _date;

        public GetPreviousDayData(int date)
        {
            _date = date;
        }

        public override Expression<Func<HourIndicator, bool>> Criteria =>
            myHourIndicator => myHourIndicator.Date < _date ;

    }
}