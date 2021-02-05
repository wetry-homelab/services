using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;

namespace Application.Mappers
{
    public class TemplateMapperProfile : Profile
    {
        public TemplateMapperProfile()
        {
            CreateMap<Template, TemplateResponse>();
            CreateMap<TemplateCreateRequest, Template>();
        }
    }
}
