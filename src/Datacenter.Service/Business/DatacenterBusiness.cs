using Application.Contracts.Response;
using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datacenter.Service.Business
{
    public class DatacenterBusiness : IDatacenterBusiness
    {
        private readonly ILogger<DatacenterBusiness> logger;
        private readonly IDatacenterRepository datacenterRepository;
        private readonly IMapper mapper;

        public DatacenterBusiness(ILogger<DatacenterBusiness> logger, IDatacenterRepository datacenterRepository, IMapper mapper)
        {
            this.logger = logger;
            this.datacenterRepository = datacenterRepository;
            this.mapper = mapper;
        }

        public async Task<DatacenterNodeResponse[]> GetDatacenter()
        {
            var nodes = await datacenterRepository.ReadsAsync();

            return nodes.Select(node => mapper.Map<DatacenterNodeResponse>(node)).ToArray();
        }
    }
}
