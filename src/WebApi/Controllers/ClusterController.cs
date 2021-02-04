using Infrastructure.Contracts.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetClustersAsync()
        {
            return Ok(await clusterBusiness.ListClusterAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClusterAsync([FromBody] ClusterCreateRequest request)
        {
            if (await clusterBusiness.CreateClusterAsync(request))
            {
                return Created(string.Empty, null);
            }

            return BadRequest();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClusterAsync([FromRoute] Guid id, [FromBody] ClusterUpdateRequest request)
        {
            var updateResult = await clusterBusiness.UpdateClusterAsync(id, request);

            if (!updateResult.found)
                return NotFound();

            if (updateResult.update)
                return Ok();

            return BadRequest();
        }
    }
}
