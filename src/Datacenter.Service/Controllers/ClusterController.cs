using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Datacenter.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClusterController : ControllerBase
    {
        private readonly ILogger<ClusterController> logger;
        private readonly IClusterBusiness clusterBusiness;

        public ClusterController(ILogger<ClusterController> logger, IClusterBusiness clusterBusiness)
        {
            this.logger = logger;
            this.clusterBusiness = clusterBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await clusterBusiness.ListClusterAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/kubeconfig")]
        public async Task<IActionResult> GetKubeconfig(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/command/restart")]
        public async Task<IActionResult> Restart(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/command/{nodeid}/recycle")]
        public async Task<IActionResult> Recycle(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }

        [HttpPatch("{id}/scale/out")]
        public async Task<IActionResult> HorizontalScale(int id)
        {
            return Ok();
        }

        [HttpPatch("{id}/scale/up")]
        public async Task<IActionResult> VerticalScale(int id)
        {
            return Ok();
        }
    }
}
