using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kubernox.Service.Business
{
    public class TemplateBusiness : ITemplateBusiness
    {
        private readonly ILogger<TemplateBusiness> logger;
        private readonly ITemplateRepository templateRepository;
        private readonly IMapper mapper;

        public TemplateBusiness(ILogger<TemplateBusiness> logger, ITemplateRepository templateRepository, IMapper mapper)
        {
            this.logger = logger;
            this.templateRepository = templateRepository;
            this.mapper = mapper;
        }

        public async Task<TemplateResponse[]> GetTemplatesAsync()
        {
            var dbTemplates = await templateRepository.ReadsAsync();
            return dbTemplates.Select(t => mapper.Map<TemplateResponse>(t)).ToArray();
        }

        public async Task<bool> CreateTemplateAsync(TemplateCreateRequest request)
        {
            var dbTemplate = mapper.Map<Template>(request);

            return ((await templateRepository.InsertAsync(dbTemplate)) > 0);
        }

        public async Task<(bool found, bool success)> UpdateTemplateAsync(int id, TemplateUpdateRequest request)
        {
            var dbTemplate = await templateRepository.ReadAsync(t => t.Id == id);

            if (dbTemplate != null)
            {
                if (!string.IsNullOrEmpty(request.Name))
                    dbTemplate.Name = request.Name;

                dbTemplate.CpuCount = request.CpuCount;
                dbTemplate.MemoryCount = request.MemoryCount;
                dbTemplate.DiskSpace = request.DiskSpace;

                if (!string.IsNullOrEmpty(request.Name))
                    dbTemplate.BaseTemplate = request.BaseTemplate;

                return (true, (await templateRepository.UpdateAsync(dbTemplate)) > 0);
            }

            return (false, false);
        }

        public async Task<(bool found, bool success)> DeleteTemplateAsync(int id)
        {
            var dbTemplate = await templateRepository.ReadAsync(t => t.Id == id);

            if (dbTemplate != null)
            {
                dbTemplate.DeleteAt = DateTime.UtcNow;

                return (true, (await templateRepository.UpdateAsync(dbTemplate)) > 0);
            }

            return (false, false);
        }
    }
}
