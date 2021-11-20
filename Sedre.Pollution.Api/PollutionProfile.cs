﻿using System.Collections.Generic;
using BuildingBlocks.Application;
using Humanizer;
using Sedre.Pollution.Application.Contracts;
using Sedre.Pollution.Domain;
using Sedre.Pollution.Domain.Enums;
using Sedre.Pollution.Domain.Models;
using Sedre.Pollution.Domain.ProxyServices.Dto;

namespace Sedre.Pollution.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class PollutionProfile : BuildingBlocksProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public PollutionProfile()
        {
            CreateMap<AiIndicatorDto, Indicator>()
                .ForMember(x => x.Latitude, opt => opt.MapFrom(c => c.Latitudes))
                .ForMember(x => x.Longitude, opt => opt.MapFrom(c => c.Longtitudes))
                .ForMember(x => x.Co, opt => opt.MapFrom(c => c.CO))
                .ForMember(x => x.No2, opt => opt.MapFrom(c => c.NO2))
                .ForMember(x => x.O3, opt => opt.MapFrom(c => c.O3))
                .ForMember(x => x.Pm10, opt => opt.MapFrom(c => c.PM10))
                .ForMember(x => x.Pm25, opt => opt.MapFrom(c => c.PM2_5))
                .ForMember(x => x.So2, opt => opt.MapFrom(c => c.SO2));

            CreateMap<Indicator, UiIndicatorDto>();
            

            CreateMap<UiIndicatorDto, CoDto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.Co})));

            CreateMap<UiIndicatorDto, No2Dto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.No2})));

            CreateMap<UiIndicatorDto, O3Dto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.O3})));

            CreateMap<UiIndicatorDto, Pm10Dto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.Pm10})));

            CreateMap<UiIndicatorDto, Pm25Dto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.Pm25})));

            CreateMap<UiIndicatorDto, So2Dto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.So2})));


            CreateMap<UiIndicatorDto, AllDto>()
                .ForMember(x => x.All, opt => 
                    opt.MapFrom(c => Formula.AllFormula(new List<double>{c.Co,c.No2,c.O3,c.Pm10,c.Pm25,c.So2})));



            CreateMap<IndicatorType, EnumDto>()
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Humanize()));


        }
    }
}