using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kubernox.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstanceController : ControllerBase
    {
        private readonly ILogger<InstanceController> logger;

        public InstanceController(ILogger<InstanceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }

        [HttpGet("{id}/snapshot")]
        public async Task<IActionResult> GetSnapshot(int id)
        {
            return Ok();
        }

        [HttpPost("{id}/snapshot/create")]
        public async Task<IActionResult> CreateSnapshot(int id)
        {
            return Ok();
        }

        [HttpPut("{id}/snapshot/{snapId}/restore")]
        public async Task<IActionResult> RestoreSnapshot(int id, Guid snapId)
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

        [HttpGet("{id}/command/restart")]
        public async Task<IActionResult> Restart(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/command/start")]
        public async Task<IActionResult> Start(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/command/stop")]
        public async Task<IActionResult> Stop(int id)
        {
            return Ok();
        }
    }
}
