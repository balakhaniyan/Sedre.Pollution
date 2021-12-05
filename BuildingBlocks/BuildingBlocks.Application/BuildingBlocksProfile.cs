using AutoMapper;
using BuildingBlocks.Application.Contracts;
using BuildingBlocks.Domain.Models;

namespace BuildingBlocks.Application
{
    public class BuildingBlocksProfile : Profile
    {
        public BuildingBlocksProfile()
        {
            CreateMap<Document, DocumentDto>();
        }

    }
}