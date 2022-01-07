using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Application.Contracts;
using BuildingBlocks.Domain.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Sedre.Pollution.Application.Contracts;
using Sedre.Pollution.Domain.Models;
using Sedre.Pollution.Domain.ProxyServices;
using Sedre.Pollution.Domain.Specifications;
using Sedre.Pollution.Domain.Statics;

namespace Sedre.Pollution.Application.Services
{
    [ApiController]
    [Route("api/[controller]")]

    public class PollutionAppService : ControllerBase
    { 
        
        #region init
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HourIndicator> _hourIndicatorRepository;
        private readonly IRepository<DayIndicator> _dayIndicatorRepository;
        private readonly IRepository<MonthIndicator> _monthIndicatorRepository;
        private readonly IAiInfo _aiInfo;
        private readonly IRecurringJobManager _recurringJobManager;

        public PollutionAppService(IMapper mapper, IUnitOfWork unitOfWork,
            IRepository<HourIndicator> hourIndicatorRepository, 
            IRepository<DayIndicator> dayIndicatorRepository,
            IRepository<MonthIndicator> monthIndicatorRepository,
            IAiInfo aiInfo, IRecurringJobManager recurringJobManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hourIndicatorRepository = hourIndicatorRepository;
            _dayIndicatorRepository = dayIndicatorRepository;
            _monthIndicatorRepository = monthIndicatorRepository;
            _aiInfo = aiInfo;
            _recurringJobManager = recurringJobManager;
        }
        
        #endregion

        #region front apis

        [HttpGet("MapData")]
        public async Task<IActionResult> GetMapData()
        {
            var indicatorsDto = await SearchForLastHourData(DateTime.Now);
            var colorDto = _mapper.Map<IList<MainMapDto>>(indicatorsDto.Indicators);
            return Ok(colorDto);
        }

        [HttpGet("CoordinateHourData")]
        public async Task<IActionResult> GetCoordinateHourData(
            [Required] double latitude, [Required] double longitude,
            [Range(0,23)] int? beginHour, [Range(0,23)] int? endHour)
        {
            var indicatorsDto = await SearchForLastHourData(DateTime.Now);
            var closeIndicator = FindClosestPoint(indicatorsDto.Indicators,latitude, longitude);
            
            if (beginHour.HasValue && endHour.HasValue)
            {
                if(beginHour.Value > endHour.Value)
                {
                    return BadRequest(new ResponseDto(Error.BeginEndConstraint));
                }

                if (beginHour.Value > indicatorsDto.Time)
                {
                    return BadRequest(new ResponseDto(Error.NoDataFoundInBeginAndEnd));
                }

                var lastHour = Math.Min(indicatorsDto.Time, endHour.Value);
                closeIndicator = await SearchForHourPeriod(closeIndicator,
                    indicatorsDto.Date, beginHour.Value,lastHour);

                indicatorsDto.Date = 0;
                indicatorsDto.Time = 0;
            }
            
            var result = new CoordinateDto
            {
                Date = indicatorsDto.Date,
                Time = indicatorsDto.Time,
                Indicator = closeIndicator
            };
            return Ok(result);
        }        
        
        [HttpGet("CoordinateDayData")]
        public async Task<IActionResult> GetCoordinateDayData(
            [Required] double latitude, [Required] double longitude,
            [Range(1,31)] int? beginDay, [Range(1,31)] int? endDay)
        {
            var indicatorsDto = await SearchForLastDayData(DateTime.Now);
            var closeIndicator = FindClosestPoint(indicatorsDto.Indicators,latitude, longitude);
            
            if (beginDay.HasValue && endDay.HasValue)
            {
                if(beginDay.Value > endDay.Value)
                {
                    return BadRequest(new ResponseDto(Error.BeginEndConstraint));
                }

                if (beginDay.Value > indicatorsDto.Date % 100)
                {
                    return BadRequest(new ResponseDto(Error.NoDataFoundInBeginAndEnd));
                }

                var lastDay = Math.Min(indicatorsDto.Date % 100, endDay.Value);
                closeIndicator = await SearchForDayPeriod(closeIndicator,
                    (indicatorsDto.Date - indicatorsDto.Date % 100)/100, beginDay.Value,lastDay);

                indicatorsDto.Date = 0;
                indicatorsDto.Time = 0;
            }
            
            var result = new CoordinateDto
            {
                Date = indicatorsDto.Date,
                Indicator = closeIndicator
            };
            return Ok(result);
        }
        
        [HttpGet("CoordinateMonthData")]
        public async Task<IActionResult> GetCoordinateMonthData(
            [Required] double latitude, [Required] double longitude,
            int? beginMonth, int? endMonth)
        {
            var indicatorsDto = await SearchForLastMonthData(DateTime.Now);
            var closeIndicator = FindClosestPoint(indicatorsDto.Indicators,latitude, longitude);
            
            if (beginMonth.HasValue && endMonth.HasValue)
            {
                if(beginMonth.Value > endMonth.Value)
                {
                    return BadRequest(new ResponseDto(Error.BeginEndConstraint));
                }

                if (beginMonth.Value > (indicatorsDto.Date - indicatorsDto.Date % 100) /100)
                {
                    return BadRequest(new ResponseDto(Error.NoDataFoundInBeginAndEnd));
                }

                var lastMonth = Math.Min((indicatorsDto.Date - indicatorsDto.Date % 100) /100, endMonth.Value);
                closeIndicator = await SearchForMonthPeriod(closeIndicator,
                    beginMonth.Value,lastMonth);

                indicatorsDto.Date = 0;
                indicatorsDto.Time = 0;
            }
            
            var result = new CoordinateDto
            {
                Date = indicatorsDto.Date,
                Indicator = closeIndicator
            };
            return Ok(result);
        }

        #endregion

        #region private methods

        private AllDto FindClosestPoint(IEnumerable<AllDto> indicators, double latitude, double longitude)
        {
            var closeIndicator = new AllDto();
            var distance = double.MaxValue;
            foreach (var indicator in indicators)
            {
                var averageLatitude = 
                    (indicator.ALatitude + indicator.BLatitude + indicator.CLatitude + indicator.DLatitude) /4;
                var averageLongitude = 
                    (indicator.ALongitude + indicator.BLongitude + indicator.CLongitude + indicator.DLongitude) /4;

                var tmpDistance = 
                    Math.Abs(averageLatitude - latitude) + Math.Abs(averageLongitude - longitude);

                if (!(tmpDistance < distance)) continue;
                distance = tmpDistance;
                closeIndicator = _mapper.Map<AllDto>(indicator);
            }

            return closeIndicator;
        }
        
        private async Task<LastDataDto> SearchForLastHourData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            var time = pc.GetHour(dateTime);
            
            var myHourIndicators = await _hourIndicatorRepository.ListAsync(new ExistHourSpecification(date,time));
            var myHourIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<AllDto>>(myHourIndicators);

            var myHourIndicatorsDto = new LastDataDto
            {
                Date = date,
                Time = time,
                Indicators = myHourIndicatorsWithoutDateAndTimeDto
            };
                
            if (myHourIndicators.Count == 0)
                myHourIndicatorsDto = await SearchForLastHourData(dateTime.AddHours(-1));
            
            return myHourIndicatorsDto;
        }
        
        private async Task<LastDataDto> SearchForLastDayData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            
            var myDayIndicators = await _dayIndicatorRepository.ListAsync(new ExistDaySpecification(date));
            var myDayIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<AllDto>>(myDayIndicators);

            var myDayIndicatorsDto = new LastDataDto
            {
                Date = date,
                Indicators = myDayIndicatorsWithoutDateAndTimeDto
            };
                
            if (myDayIndicators.Count == 0)
                myDayIndicatorsDto = await SearchForLastDayData(dateTime.AddDays(-1));
            
            return myDayIndicatorsDto;
        }
        
        private async Task<LastDataDto> SearchForLastMonthData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            
            var myMonthIndicators = await _monthIndicatorRepository.ListAsync(new ExistMonthSpecification((date - date %100) /100));
            var myMonthIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<AllDto>>(myMonthIndicators);

            var myMonthIndicatorsDto = new LastDataDto
            {
                Date = date,
                Indicators = myMonthIndicatorsWithoutDateAndTimeDto
            };
                
            if (myMonthIndicators.Count == 0)
                myMonthIndicatorsDto = await SearchForLastMonthData(dateTime.AddMonths(-1));
            
            return myMonthIndicatorsDto;
        }

        private async Task<AllDto> SearchForHourPeriod(AllDto closeIndicator,
            int date, int beginHour, int endHour)
        {
            var myAllDto = new AllDto();
            var myList = await _hourIndicatorRepository.ListAsync(new GetPeriodHour(
                closeIndicator.ALatitude, closeIndicator.ALongitude,
                closeIndicator.BLatitude, closeIndicator.BLongitude,
                closeIndicator.CLatitude, closeIndicator.CLongitude,
                closeIndicator.DLatitude, closeIndicator.DLongitude,
                date, beginHour, endHour));

            if (myList.Count == 0) return myAllDto;
            var sumCo = 0.00;
            var sumNo2 = 0.00;
            var sumO3 = 0.00;
            var sumPm10 = 0.00;
            var sumPm25 = 0.00;
            var sumSo2 = 0.00;
            var minMoment = int.MaxValue;
            var minValue = double.MaxValue;
            var maxMoment = int.MinValue;
            var maxValue = double.MinValue;
            foreach (var t in myList)
            {
                sumCo += t.Co;
                sumNo2 += t.No2;
                sumO3 += t.O3;
                sumPm10 += t.Pm10;
                sumPm25 += t.Pm25;
                sumSo2 += t.So2;

                var tmp = Formula.DefineAll(new List<double> {t.Co, t.No2, t.O3, t.Pm10, t.Pm25, t.So2});
                if (tmp < minValue)
                {
                    minValue = tmp;
                    minMoment = t.Time;
                }
                if (tmp > maxValue)
                {
                    maxValue = tmp;
                    maxMoment = t.Time;
                }
            }

            var myIndicator = new AllDto
            {
                ALatitude = closeIndicator.ALatitude,
                ALongitude = closeIndicator.ALongitude,
                BLatitude = closeIndicator.BLatitude,
                BLongitude = closeIndicator.BLongitude,
                CLatitude = closeIndicator.CLatitude,
                CLongitude = closeIndicator.CLongitude,
                DLatitude = closeIndicator.DLatitude,
                DLongitude = closeIndicator.DLongitude,
                Co = Math.Round(sumCo / myList.Count, 2),
                No2 = Math.Round(sumNo2 / myList.Count, 2),
                O3 = Math.Round(sumO3 / myList.Count, 2),
                Pm10 = Math.Round(sumPm10 / myList.Count, 2),
                Pm25 = Math.Round(sumPm25 / myList.Count, 2),
                So2 = Math.Round(sumSo2 / myList.Count, 2),
                MinMoment = minMoment,
                MinValue = minValue,
                MaxMoment = maxMoment,
                MaxValue = maxValue
            };

            return myIndicator;
        }
        
        private async Task<AllDto> SearchForDayPeriod(AllDto closeIndicator,
            int date, int beginDay, int endDay)
        {
            var myAllDto = new AllDto();
            var myList = await _dayIndicatorRepository.ListAsync(new GetPeriodDay(
                closeIndicator.ALatitude, closeIndicator.ALongitude,
                closeIndicator.BLatitude, closeIndicator.BLongitude,
                closeIndicator.CLatitude, closeIndicator.CLongitude,
                closeIndicator.DLatitude, closeIndicator.DLongitude,
                date, beginDay, endDay));

            if (myList.Count == 0) return myAllDto;
            var sumCo = 0.00;
            var sumNo2 = 0.00;
            var sumO3 = 0.00;
            var sumPm10 = 0.00;
            var sumPm25 = 0.00;
            var sumSo2 = 0.00;
            var minMoment = int.MaxValue;
            var minValue = double.MaxValue;
            var maxMoment = int.MinValue;
            var maxValue = double.MinValue;
            foreach (var t in myList)
            {
                sumCo += t.Co;
                sumNo2 += t.No2;
                sumO3 += t.O3;
                sumPm10 += t.Pm10;
                sumPm25 += t.Pm25;
                sumSo2 += t.So2;
                
                var tmp = Formula.DefineAll(new List<double> {t.Co, t.No2, t.O3, t.Pm10, t.Pm25, t.So2});
                if (tmp < minValue)
                {
                    minValue = tmp;
                    minMoment = t.Date;
                }
                if (tmp > maxValue)
                {
                    maxValue = tmp;
                    maxMoment = t.Date;
                }
            }

            var myIndicator = new AllDto
            {
                ALatitude = closeIndicator.ALatitude,
                ALongitude = closeIndicator.ALongitude,
                BLatitude = closeIndicator.BLatitude,
                BLongitude = closeIndicator.BLongitude,
                CLatitude = closeIndicator.CLatitude,
                CLongitude = closeIndicator.CLongitude,
                DLatitude = closeIndicator.DLatitude,
                DLongitude = closeIndicator.DLongitude,
                Co = Math.Round(sumCo / myList.Count, 2),
                No2 = Math.Round(sumNo2 / myList.Count, 2),
                O3 = Math.Round(sumO3 / myList.Count, 2),
                Pm10 = Math.Round(sumPm10 / myList.Count, 2),
                Pm25 = Math.Round(sumPm25 / myList.Count, 2),
                So2 = Math.Round(sumSo2 / myList.Count, 2),
                MinMoment = minMoment,
                MinValue = minValue,
                MaxMoment = maxMoment,
                MaxValue = maxValue
            };

            return myIndicator;
        }
        
        private async Task<AllDto> SearchForMonthPeriod(AllDto closeIndicator,
            int beginMonth, int endMonth)
        {
            var myAllDto = new AllDto();
            var myList = await _monthIndicatorRepository.ListAsync(new GetPeriodMonth(
                closeIndicator.ALatitude, closeIndicator.ALongitude,
                closeIndicator.BLatitude, closeIndicator.BLongitude,
                closeIndicator.CLatitude, closeIndicator.CLongitude,
                closeIndicator.DLatitude, closeIndicator.DLongitude,
                beginMonth, endMonth));

            if (myList.Count == 0) return myAllDto;
            var sumCo = 0.00;
            var sumNo2 = 0.00;
            var sumO3 = 0.00;
            var sumPm10 = 0.00;
            var sumPm25 = 0.00;
            var sumSo2 = 0.00;
            var minMoment = int.MaxValue;
            var minValue = double.MaxValue;
            var maxMoment = int.MinValue;
            var maxValue = double.MinValue;
            foreach (var t in myList)
            {
                sumCo += t.Co;
                sumNo2 += t.No2;
                sumO3 += t.O3;
                sumPm10 += t.Pm10;
                sumPm25 += t.Pm25;
                sumSo2 += t.So2;
                
                var tmp = Formula.DefineAll(new List<double> {t.Co, t.No2, t.O3, t.Pm10, t.Pm25, t.So2});
                if (tmp < minValue)
                {
                    minValue = tmp;
                    minMoment = t.Date;
                }
                if (tmp > maxValue)
                {
                    maxValue = tmp;
                    maxMoment = t.Date;
                }
            }

            var myIndicator = new AllDto
            {
                ALatitude = closeIndicator.ALatitude,
                ALongitude = closeIndicator.ALongitude,
                BLatitude = closeIndicator.BLatitude,
                BLongitude = closeIndicator.BLongitude,
                CLatitude = closeIndicator.CLatitude,
                CLongitude = closeIndicator.CLongitude,
                DLatitude = closeIndicator.DLatitude,
                DLongitude = closeIndicator.DLongitude,
                Co = Math.Round(sumCo / myList.Count, 2),
                No2 = Math.Round(sumNo2 / myList.Count, 2),
                O3 = Math.Round(sumO3 / myList.Count, 2),
                Pm10 = Math.Round(sumPm10 / myList.Count, 2),
                Pm25 = Math.Round(sumPm25 / myList.Count, 2),
                So2 = Math.Round(sumSo2 / myList.Count, 2),
                MinMoment = minMoment,
                MinValue = minValue,
                MaxMoment = maxMoment,
                MaxValue = maxValue
            };

            return myIndicator;
        }
        
        #endregion

        #region job apis

        [HttpGet("ActiveJobs")]
        public void GetActiveJobs()
        {
            _recurringJobManager.AddOrUpdate(
                "get ai info every hour",
                () => GetLastAiInfo(),
                Cron.Hourly //"0 * * * *" every hour
            );
            
            _recurringJobManager.AddOrUpdate(
                "compute last day average",
                () => GetComputeDayAverage(),
                "30 0 * * *" //every day on 4AM = 12:30 + 3:30 (time zone)
            );
            
            _recurringJobManager.AddOrUpdate(
                "compute last month average",
                () => GetComputeMonthAverage(),
                "45 0 2 * *" //every second day of month at 4:15AM = 12:45 + 3:30 (time zone)
            );
            
        }

        [HttpGet("SyncWithAi")]
        public async Task<IActionResult> GetLastAiInfo()
        {
            var myIndicatorsDto = await _aiInfo.GetLastData();

            var date = int.Parse(myIndicatorsDto.Date);
            var time = int.Parse(myIndicatorsDto.Time);

            var alreadyExist = await _hourIndicatorRepository.GetAsync(new ExistHourSpecification(date, time));
            if (!(alreadyExist is null))
                return BadRequest(new ResponseDto(Error.LastDataExist));

            var myIndicatorsWithoutDateAndTime = _mapper.Map<List<HourIndicator>>(myIndicatorsDto.indicators);
            var myIndicators = myIndicatorsWithoutDateAndTime.Select(c =>
            {
                c.Date = date;
                c.Time = time;
                return c;
            }).ToList();

            await _hourIndicatorRepository.AddList(myIndicators);
            await _unitOfWork.CompleteAsync();

            return Ok(new ResponseDto(Error.LastDataReceived));
        }

        [HttpGet("ComputeDayAverage")]
        public async Task<IActionResult> GetComputeDayAverage()
        {
            var indicatorsDto = await SearchForLastHourData(DateTime.Now);
            var previousData = await _hourIndicatorRepository.ListAsync(new GetPreviousDayData(indicatorsDto.Date));
            var sortedPreviousData = previousData.OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
            
            IList<List<HourIndicator>> listOfLists = new List<List<HourIndicator>>();
            var lastIndicatorDate = sortedPreviousData.First().Date;
            var lastIndicatorTime = sortedPreviousData.First().Time;
            var tmp = new List<HourIndicator>();
            foreach (var indicator in sortedPreviousData)
            {
                if (lastIndicatorDate == indicator.Date && lastIndicatorTime == indicator.Time)
                {
                    tmp.Add(indicator);
                }
                else if(lastIndicatorDate == indicator.Date && lastIndicatorTime != indicator.Time)
                {
                    if (tmp.Count == 0) continue;
                    listOfLists.Add(tmp);
                    tmp = new List<HourIndicator>();
                    lastIndicatorTime--;
                }
                else
                {
                    break;
                }
            }
            listOfLists.Add(tmp);

            for (var i = 0; i < listOfLists.First().Count; i++)
            {
                var sumCo = 0.00;
                var sumNo2 = 0.00;
                var sumO3 = 0.00;
                var sumPm10 = 0.00;
                var sumPm25 = 0.00;
                var sumSo2 = 0.00;
                foreach (var t in listOfLists)
                {
                    sumCo += t[i].Co;
                    sumNo2 += t[i].No2;
                    sumO3 += t[i].O3;
                    sumPm10 += t[i].Pm10;
                    sumPm25 += t[i].Pm25;
                    sumSo2 += t[i].So2;
                }

                var myDayIndicator = new DayIndicator
                {
                    Date = listOfLists[0][i].Date,
                    ALatitude = listOfLists[0][i].ALatitude,
                    ALongitude = listOfLists[0][i].ALongitude,
                    BLatitude = listOfLists[0][i].BLatitude,
                    BLongitude = listOfLists[0][i].BLongitude,
                    CLatitude = listOfLists[0][i].CLatitude,
                    CLongitude = listOfLists[0][i].CLongitude,
                    DLatitude = listOfLists[0][i].DLatitude,
                    DLongitude = listOfLists[0][i].DLongitude,
                    Co = Math.Round(sumCo / listOfLists.Count, 2),
                    No2 = Math.Round(sumNo2 / listOfLists.Count, 2),
                    O3 = Math.Round(sumO3 / listOfLists.Count, 2),
                    Pm10 = Math.Round(sumPm10 / listOfLists.Count, 2),
                    Pm25 = Math.Round(sumPm25 / listOfLists.Count, 2),
                    So2 = Math.Round(sumSo2 / listOfLists.Count, 2),
                };
                await _dayIndicatorRepository.Add(myDayIndicator);
            }

            await _hourIndicatorRepository.DeleteAsync(previousData);
            await _unitOfWork.CompleteAsync();
            return Ok(new ResponseDto(Error.PreviousDayAveraged));
        }

        [HttpGet("ComputeMonthAverage")]
        public async Task<IActionResult> GetComputeMonthAverage()
        {
            var indicatorsDto = await SearchForLastDayData(DateTime.Now);
            var previousData = await _dayIndicatorRepository.ListAsync(new GetPreviousMonthData(indicatorsDto.Date));
            var sortedPreviousData = previousData.OrderByDescending(x => x.Date).ToList();
            
            IList<List<DayIndicator>> listOfLists = new List<List<DayIndicator>>();
            var lastIndicatorDay = sortedPreviousData.First().Date % 100; // day
            var lastIndicatorMonth = (sortedPreviousData.First().Date - lastIndicatorDay) / 100;
            var tmp = new List<DayIndicator>();
            foreach (var indicator in sortedPreviousData)
            {
                if (lastIndicatorMonth == (indicator.Date - indicator.Date % 100 ) / 100
                    && lastIndicatorDay == indicator.Date % 100)
                {
                    tmp.Add(indicator);
                }
                else if(lastIndicatorMonth == (indicator.Date - indicator.Date % 100 ) / 100
                        && lastIndicatorDay != indicator.Date % 100)
                {
                    if (tmp.Count == 0) continue;
                    listOfLists.Add(tmp);
                    tmp = new List<DayIndicator>();
                    lastIndicatorDay--;
                }
                else
                {
                    break;
                }
            }
            listOfLists.Add(tmp);

            for (var i = 0; i < listOfLists.First().Count; i++)
            {
                var sumCo = 0.00;
                var sumNo2 = 0.00;
                var sumO3 = 0.00;
                var sumPm10 = 0.00;
                var sumPm25 = 0.00;
                var sumSo2 = 0.00;
                foreach (var t in listOfLists)
                {
                    sumCo += t[i].Co;
                    sumNo2 += t[i].No2;
                    sumO3 += t[i].O3;
                    sumPm10 += t[i].Pm10;
                    sumPm25 += t[i].Pm25;
                    sumSo2 += t[i].So2;
                }

                var myMonthIndicator = new MonthIndicator
                {
                    Date = (listOfLists[0][i].Date - listOfLists[0][i].Date % 100) /100,
                    ALatitude = listOfLists[0][i].ALatitude,
                    ALongitude = listOfLists[0][i].ALongitude,
                    BLatitude = listOfLists[0][i].BLatitude,
                    BLongitude = listOfLists[0][i].BLongitude,
                    CLatitude = listOfLists[0][i].CLatitude,
                    CLongitude = listOfLists[0][i].CLongitude,
                    DLatitude = listOfLists[0][i].DLatitude,
                    DLongitude = listOfLists[0][i].DLongitude,
                    Co = Math.Round(sumCo / listOfLists.Count, 2),
                    No2 = Math.Round(sumNo2 / listOfLists.Count, 2),
                    O3 = Math.Round(sumO3 / listOfLists.Count, 2),
                    Pm10 = Math.Round(sumPm10 / listOfLists.Count, 2),
                    Pm25 = Math.Round(sumPm25 / listOfLists.Count, 2),
                    So2 = Math.Round(sumSo2 / listOfLists.Count, 2),
                };
                await _monthIndicatorRepository.Add(myMonthIndicator);
            }

            await _dayIndicatorRepository.DeleteAsync(previousData);
            await _unitOfWork.CompleteAsync();
            return Ok(new ResponseDto(Error.PreviousMonthAveraged));
        }

        #endregion
        
    }
    
}