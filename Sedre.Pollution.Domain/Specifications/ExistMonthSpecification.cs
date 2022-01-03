using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class ExistMonthSpecification: Specification<MonthIndicator>
    {

        private readonly int _date;

        public ExistMonthSpecification(int date)
        {
            _date = date;
        }

        public override Expression<Func<MonthIndicator, bool>> Criteria =>
            myMonthIndicator => myMonthIndicator.Date == _date;

    }
}