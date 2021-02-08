using Application.Exceptions;
using Application.Interfaces;
using Infrastructure.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        [HttpGet("{id}/metrics")]
        public async Task<IActionResult> GetMetrics([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpGet("{id}/kubeconfig")]
        public async Task<IActionResult> GetKubeconfig([FromRoute] Guid id)
        {
            var downloadConfig = await clusterBusiness.DownloadKubeconfigAsync(id);

            if (!downloadConfig.found)
                return NotFound();

            if (downloadConfig.ready)
                return BadRequest();

            return Ok(downloadConfig.file);
        }

        [HttpGet("{id}/command/restart")]
        public async Task<IActionResult> Restart([FromRoute] Guid id)
        {
            var restartResult = await clusterBusiness.RestartClusterMasterAsync(id);

            if (!restartResult.found)
                return NotFound();

            if (restartResult.restart)
                return Ok();

            return BadRequest();
        }

        [HttpGet("{id}/command/{nodeid}/recycle")]
        public async Task<IActionResult> Recycle([FromRoute] Guid id, [FromRoute] Guid nodeId)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClusterCreateRequest request)
        {
            try
            {
                if ((await clusterBusiness.CreateClusterAsync(request)))
                {
                    return Ok();
                }
            }
            catch (DuplicateException ex)
            {
                logger.LogWarning(ex, "Error on create cluster.");
                return Conflict();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on create cluster.");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteResult = await clusterBusiness.DeleteClusterAsync(id);

            if (!deleteResult.found)
                return NotFound();

            if (deleteResult.update)
                return Ok();

            return BadRequest();
        }

        [HttpPatch("{id}/scale/{nodeCount}")]
        public async Task<IActionResult> HorizontalScale([FromRoute] Guid id, [FromRoute] int nodeCount)
        {
            return Ok();
        }
    }
}
