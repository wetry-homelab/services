using Application.Interfaces;
using Infrastructure.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Datacenter.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SshKeyController : ControllerBase
    {
        private readonly ILogger<SshKeyController> logger;
        private readonly ISshKeyBusiness sshKeyBusiness;

        public SshKeyController(ILogger<SshKeyController> logger, ISshKeyBusiness sshKeyBusiness)
        {
            this.logger = logger;
            this.sshKeyBusiness = sshKeyBusiness;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await sshKeyBusiness.DeleteKeyAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await sshKeyBusiness.ReadKeysAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ImportNewKey([FromBody] SshKeyCreateRequest request)
        {
            if ((await sshKeyBusiness.InsertKeyAsync(request)))
            {
                return Created(string.Empty, null);
            }

            return BadRequest();
        }
    }
}
