using System;
using System.Linq.Expressions;
using BuildingBlocks.Domain.Interfaces;
using Sedre.Pollution.Domain.Models;

namespace Sedre.Pollution.Domain.Specifications
{
    public class GetPeriodHour: Specification<HourIndicator>
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
        private readonly int _beginHour;
        private readonly int _endHour;

        public GetPeriodHour(
            double aLatitude,double aLongitude,
            double bLatitude,double bLongitude,
            double cLatitude,double cLongitude,
            double dLatitude,double dLongitude,
            int date,int beginHour,int endHour)
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
            _beginHour = beginHour;
            _endHour = endHour;
        }

        public override Expression<Func<HourIndicator, bool>> Criteria =>
            myIndicator => Math.Abs(myIndicator.ALatitude - _aLatitude) < Tolerance &&
                           Math.Abs(myIndicator.ALongitude - _aLongitude) < Tolerance &&
                           Math.Abs(myIndicator.BLatitude - _bLatitude) < Tolerance &&
                           Math.Abs(myIndicator.BLongitude - _bLongitude) < Tolerance &&
                           Math.Abs(myIndicator.CLatitude - _cLatitude) < Tolerance && 
                           Math.Abs(myIndicator.CLongitude - _cLongitude) < Tolerance && 
                           Math.Abs(myIndicator.DLatitude - _dLatitude) < Tolerance && 
                           Math.Abs(myIndicator.DLongitude - _dLongitude) < Tolerance && 
                           myIndicator.Date == _date && 
                           myIndicator.Time >= _beginHour && 
                           myIndicator.Time <= _endHour;
    }
}