using Infrastructure.Contracts.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class DatacenterMapperProfile : Profile
    {
        public DatacenterMapperProfile()
        {
            CreateMap<DatacenterNode, DatacenterNodeResponse>();
        }
    }
}
