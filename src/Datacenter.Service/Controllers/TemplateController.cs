using Application.Interfaces;
using Infrastructure.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Datacenter.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> logger;
        private readonly ITemplateBusiness templateBusiness;

        public TemplateController(ILogger<TemplateController> logger, ITemplateBusiness templateBusiness)
        {
            this.logger = logger;
            this.templateBusiness = templateBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await templateBusiness.GetTemplatesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TemplateCreateRequest request)
        {
            var createResult = await templateBusiness.CreateTemplateAsync(request);

            if (createResult)
                return Created(string.Empty, null);

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TemplateUpdateRequest request)
        {
            var updateResult = await templateBusiness.UpdateTemplateAsync(id, request);

            if (!updateResult.found)
                return NotFound();

            if (updateResult.success)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteResult = await templateBusiness.DeleteTemplateAsync(id);

            if (!deleteResult.found)
                return NotFound();

            if (deleteResult.success)
                return Ok();

            return BadRequest();
        }
    }
}
