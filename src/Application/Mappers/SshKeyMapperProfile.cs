using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts.Response;

namespace Application.Mappers
{
    public class SshKeyMapperProfile : Profile
    {
        public SshKeyMapperProfile()
        {
            CreateMap<SshKey, SshKeyResponse>()
                .ForMember(m => m.PemAvailable, m => m.MapFrom(s => !string.IsNullOrEmpty(s.Pem)))
                .ForMember(m => m.PpkAvailable, m => m.MapFrom(s => !string.IsNullOrEmpty(s.Private)));
        }
    }
}
