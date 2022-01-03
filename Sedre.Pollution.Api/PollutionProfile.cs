using AutoMapper;
using Sedre.Pollution.Application.Contracts;
using Sedre.Pollution.Domain.Models;
using Sedre.Pollution.Domain.ProxyServices.Dto;
using Sedre.Pollution.Domain.Statics;

namespace Sedre.Pollution.Api
{
    /// <inheritdoc />
    public class PollutionProfile : Profile
    {
        /// <inheritdoc />
        public PollutionProfile()
        {
            CreateMap<AiIndicatorDto, HourIndicator>()
                .ForMember(x => x.Co, opt => opt.MapFrom(c => c.CO))
                .ForMember(x => x.No2, opt => opt.MapFrom(c => c.NO2))
                .ForMember(x => x.O3, opt => opt.MapFrom(c => c.O3))
                .ForMember(x => x.Pm10, opt => opt.MapFrom(c => c.PM10))
                .ForMember(x => x.Pm25, opt => opt.MapFrom(c => c.PM2_5))
                .ForMember(x => x.So2, opt => opt.MapFrom(c => c.SO2));

            CreateMap<HourIndicator, AllDto>();
            CreateMap<DayIndicator, AllDto>();
            CreateMap<MonthIndicator, AllDto>();
            
            CreateMap<AllDto, MainMapDto>()
                .ForMember(x => x.Color,
                    opt => opt.MapFrom(c =>
                        Formula.DefineColor(c.All)))
                .ForMember(x=>x.Coordinates,
                    opt=>opt.MapFrom(c=>
                        Formula.MakeCoordinatesList(c.ALatitude,c.ALongitude,c.BLatitude,c.BLongitude,c.CLatitude,c.CLongitude,c.DLatitude,c.DLongitude)));
            

        }
    }
}