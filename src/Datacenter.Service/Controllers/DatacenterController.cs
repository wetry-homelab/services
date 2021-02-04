using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Datacenter.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatacenterController : ControllerBase
    {
        private readonly ILogger<DatacenterController> logger;
        private readonly IDatacenterBusiness datacenterBusiness;

        public DatacenterController(ILogger<DatacenterController> logger, IDatacenterBusiness datacenterBusiness)
        {
            this.logger = logger;
            this.datacenterBusiness = datacenterBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await datacenterBusiness.GetDatacenter());
        }
    }
}
