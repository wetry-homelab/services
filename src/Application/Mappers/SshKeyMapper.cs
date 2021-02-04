using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts.Response;

namespace Application.Mappers
{
    public class SshKeyMapper : Profile
    {
        public SshKeyMapper()
        {
            CreateMap<SshKey, SshKeyResponse>();
        }
    }
}
