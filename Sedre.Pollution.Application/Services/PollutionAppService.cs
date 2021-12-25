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
using Sedre.Pollution.Domain.Enums;
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
        private readonly IAiInfo _aiInfo;
        private readonly IRecurringJobManager _recurringJobManager;

        public PollutionAppService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<Indicator> repository,
            IAiInfo aiInfo, IRecurringJobManager recurringJobManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _aiInfo = aiInfo;
            _recurringJobManager = recurringJobManager;
        }

        [HttpPost("SyncWithAiJob")]
        public void GetLastAiInfoJob()
        {
            _recurringJobManager.AddOrUpdate(
                "hang fire job name",
                () => GetLastAiInfo(),
                Cron.Hourly //"0 * * * *" every hour
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
        
        

        [HttpGet("IndicatorsType")]
        public IActionResult GetIndicatorsType()
        {
            var indicatorTypes = Enum.GetValues(typeof(IndicatorType));
            return Ok(_mapper.Map<List<EnumDto>>(indicatorTypes));
        }

        [HttpGet("MapData")]
        public async Task<IActionResult> GetMapData()
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);

            var colorDto = _mapper.Map<IList<MainMapDto>>(indicatorsDto.Indicators);
            
            return Ok(colorDto);
        }

        [HttpGet("CoordinateData")]
        public async Task<IActionResult> GetCoordinateData(
            [Required] double latitude , [Required] double longitude)
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);

            var distance = double.MaxValue;
            var closeIndicator = new AllDto();
            foreach (var indicator in indicatorsDto.Indicators)
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

            var allDto = new LastUiDataDtoOutputCoordinate<AllDto>
            {
                Date = indicatorsDto.Date,
                Time = indicatorsDto.Time,
                Indicators = _mapper.Map<AllDto>(closeIndicator)
            };
            return Ok(allDto);
        }
        
        

        [HttpGet("LastData")]
        public async Task<IActionResult> GetLastData(string indicator = "All")
        {
            var indicatorsDto = await SearchForLastData(DateTime.Now);
            var indicatorType = (IndicatorType) Enum.Parse(typeof(IndicatorType), indicator, true);
            switch (indicatorType)
            {
                
                case IndicatorType.All:
                    var allDto = new LastUiDataDtoOutput<AllDto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<AllDto>>(indicatorsDto.Indicators)
                    };
                    return Ok(allDto);
                
                case IndicatorType.Co:
                    var coDto = new LastUiDataDtoOutput<CoDto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<CoDto>>(indicatorsDto.Indicators)
                    };
                    return Ok(coDto);
                
                case IndicatorType.No2:
                    var no2Dto = new LastUiDataDtoOutput<No2Dto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<No2Dto>>(indicatorsDto.Indicators)
                    };
                    return Ok(no2Dto);
                
                case IndicatorType.O3:
                    var o3Dto = new LastUiDataDtoOutput<O3Dto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<O3Dto>>(indicatorsDto.Indicators)
                    };
                    return Ok(o3Dto);
                
                case IndicatorType.Pm10:
                    var pm10Dto = new LastUiDataDtoOutput<Pm10Dto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<Pm10Dto>>(indicatorsDto.Indicators)
                    };
                    return Ok(pm10Dto);
                
                case IndicatorType.Pm25:
                    var pm25Dto = new LastUiDataDtoOutput<Pm25Dto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<Pm25Dto>>(indicatorsDto.Indicators)
                    };
                    return Ok(pm25Dto);
                
                case IndicatorType.So2:
                    var so2Dto = new LastUiDataDtoOutput<So2Dto>
                    {
                        Date = indicatorsDto.Date,
                        Time = indicatorsDto.Time,
                        Indicators = _mapper.Map<IList<So2Dto>>(indicatorsDto.Indicators)
                    };
                    return Ok(so2Dto);
                default:
                    return NotFound();
            }
            
        }

        private async Task<LastUiDataDto> SearchForLastData(DateTime dateTime)
        {
            var pc = new PersianCalendar();
            var date = int.Parse( 
                pc.GetYear(dateTime).ToString("0000") + 
                pc.GetMonth(dateTime).ToString("00") + 
                pc.GetDayOfMonth(dateTime).ToString("00")
            );
            var time = pc.GetHour(dateTime);
            
            var myIndicators = await _repository.ListAsync(new ExistSpecification(date,time));
            var myIndicatorsWithoutDateAndTimeDto = _mapper.Map<IList<UiIndicatorDto>>(myIndicators);

            var myIndicatorsDto = new LastUiDataDto
            {
                Date = date,
                Time = time,
                Indicators = myIndicatorsWithoutDateAndTimeDto
            };
                
            if (myIndicators.Count == 0)
                myIndicatorsDto = await SearchForLastData(dateTime.AddHours(-1));
            
            
            return myIndicatorsDto;
        }
    }
    
}