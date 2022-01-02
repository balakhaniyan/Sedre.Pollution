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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Indicator> _repository;
        private readonly IRepository<DayIndicator> _dayIndicatorRepository;
        private readonly IRepository<MonthIndicator> _monthIndicatorRepository;
        private readonly IAiInfo _aiInfo;
        private readonly IRecurringJobManager _recurringJobManager;

        public PollutionAppService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<Indicator> repository, 
            IRepository<DayIndicator> dayIndicatorRepository,IRepository<MonthIndicator> monthIndicatorRepository,
            IAiInfo aiInfo, IRecurringJobManager recurringJobManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _dayIndicatorRepository = dayIndicatorRepository;
            _monthIndicatorRepository = monthIndicatorRepository;
            _aiInfo = aiInfo;
            _recurringJobManager = recurringJobManager;
        }
        
        [HttpGet("MapData")]
        public async Task<IActionResult> GetMapData()
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);
            var colorDto = _mapper.Map<IList<MainMapDto>>(indicatorsDto.Indicators);
            return Ok(colorDto);
        }

        [HttpGet("CoordinateData")]
        public async Task<IActionResult> GetCoordinateData([Required] double latitude , [Required] double longitude)
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);
            var closeIndicator = FindClosestPoint(indicatorsDto.Indicators,latitude, longitude);
            
            var result = new CoordinateDto
            {
                Date = indicatorsDto.Date,
                Time = indicatorsDto.Time,
                Indicator = closeIndicator
            };
            return Ok(result);
        }

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
        
        private async Task<LastDataDto> SearchForLastData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            var time = pc.GetHour(dateTime);
            
            var myIndicators = await _repository.ListAsync(new ExistSpecification(date,time));
            var myIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<AllDto>>(myIndicators);

            var myIndicatorsDto = new LastDataDto
            {
                Date = date,
                Time = time,
                Indicators = myIndicatorsWithoutDateAndTimeDto
            };
                
            if (myIndicators.Count == 0)
                myIndicatorsDto = await SearchForLastData(dateTime.AddHours(-1));
            
            return myIndicatorsDto;
        }
        
        private async Task<LastMonthDataDto> SearchForLastMonthData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            
            var myDayIndicators = await _dayIndicatorRepository.ListAsync(new ExistDaySpecification(date));
            var myDayIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<AllDto>>(myDayIndicators);

            var myDayIndicatorsDto = new LastMonthDataDto
            {
                Date = date,
                Indicators = myDayIndicatorsWithoutDateAndTimeDto
            };
                
            if (myDayIndicators.Count == 0)
                myDayIndicatorsDto = await SearchForLastMonthData(dateTime.AddDays(-1));
            
            return myDayIndicatorsDto;
        }

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

            var alreadyExist = await _repository.GetAsync(new ExistSpecification(date, time));
            if (!(alreadyExist is null))
                return BadRequest(new ResponseDto(Error.LastDataExist));

            var myIndicatorsWithoutDateAndTime = _mapper.Map<List<Indicator>>(myIndicatorsDto.indicators);
            var myIndicators = myIndicatorsWithoutDateAndTime.Select(c =>
            {
                c.Date = date;
                c.Time = time;
                return c;
            }).ToList();

            await _repository.AddList(myIndicators);
            await _unitOfWork.CompleteAsync();

            return Ok(new ResponseDto(Error.LastDataReceived));
        }

        [HttpGet("ComputeDayAverage")]
        public async Task<IActionResult> GetComputeDayAverage()
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);
            var previousData = await _repository.ListAsync(new GetPreviousDayData(indicatorsDto.Date));
            var sortedPreviousData = previousData.OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
            
            IList<List<Indicator>> listOfLists = new List<List<Indicator>>();
            var lastIndicatorDate = sortedPreviousData.First().Date;
            var lastIndicatorTime = sortedPreviousData.First().Time;
            var tmp = new List<Indicator>();
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
                    tmp = new List<Indicator>();
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

            await _repository.DeleteAsync(previousData);
            await _unitOfWork.CompleteAsync();
            return Ok(new ResponseDto(Error.PreviousDayAveraged));
        }

        [HttpGet("ComputeMonthAverage")]
        public async Task<IActionResult> GetComputeMonthAverage()
        {
            var indicatorsDto = await SearchForLastMonthData(DateTime.Now);
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
    }
    
}