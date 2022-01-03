using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class ExistHourSpecification: Specification<HourIndicator>
    {

        private readonly int _date;
        private readonly int _time;

        public ExistHourSpecification(int date, int time)
        {
            _date = date;
            _time = time;
        }

        public override Expression<Func<HourIndicator, bool>> Criteria =>
            myIndicator => myIndicator.Date == _date && myIndicator.Time == _time;

    }
}