using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class ExistDaySpecification: Specification<DayIndicator>
    {

        private readonly int _date;

        public ExistDaySpecification(int date)
        {
            _date = date;
        }

        public override Expression<Func<DayIndicator, bool>> Criteria =>
            myDayIndicator => myDayIndicator.Date == _date;

    }
}