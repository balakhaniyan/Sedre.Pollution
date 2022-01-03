using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class GetPeriodDay: Specification<DayIndicator>
    {
        private const double Tolerance = 0.0001;

        private readonly double _aLatitude;
        private readonly double _aLongitude;
        private readonly double _bLatitude;
        private readonly double _bLongitude;
        private readonly double _cLatitude;
        private readonly double _cLongitude;
        private readonly double _dLatitude;
        private readonly double _dLongitude;
        private readonly int _date;
        private readonly int _beginDay;
        private readonly int _endDay;

        public GetPeriodDay(
            double aLatitude,double aLongitude,
            double bLatitude,double bLongitude,
            double cLatitude,double cLongitude,
            double dLatitude,double dLongitude,
            int date,int beginDay,int endDay)
        {
            _aLatitude = aLatitude;
            _aLongitude = aLongitude;
            _bLatitude = bLatitude;
            _bLongitude = bLongitude;
            _cLatitude = cLatitude;
            _cLongitude = cLongitude;
            _dLatitude = dLatitude;
            _dLongitude = dLongitude;
            _date = date;
            _beginDay = beginDay;
            _endDay = endDay;
        }

        public override Expression<Func<DayIndicator, bool>> Criteria =>
            myIndicator => Math.Abs(myIndicator.ALatitude - _aLatitude) < Tolerance &&
                           Math.Abs(myIndicator.ALongitude - _aLongitude) < Tolerance &&
                           Math.Abs(myIndicator.BLatitude - _bLatitude) < Tolerance &&
                           Math.Abs(myIndicator.BLongitude - _bLongitude) < Tolerance &&
                           Math.Abs(myIndicator.CLatitude - _cLatitude) < Tolerance && 
                           Math.Abs(myIndicator.CLongitude - _cLongitude) < Tolerance && 
                           Math.Abs(myIndicator.DLatitude - _dLatitude) < Tolerance && 
                           Math.Abs(myIndicator.DLongitude - _dLongitude) < Tolerance && 
                           (myIndicator.Date - myIndicator.Date % 100) /100 == _date && 
                           myIndicator.Date % 100 >= _beginDay && 
                           myIndicator.Date % 100 <= _endDay;
    }
}