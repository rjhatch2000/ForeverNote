using ForeverNote.Api.Commands.Models.Catalog;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Api.Controllers.OData
{
    public partial class SpecificationAttributeController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public SpecificationAttributeController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();

            var specificationAttribute = await _mediator.Send(new GetQuery<SpecificationAttributeDto>() { Id = key });
            if (!specificationAttribute.Any())
                return NotFound();

            return Ok(specificationAttribute.FirstOrDefault());
        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<SpecificationAttributeDto>()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SpecificationAttributeDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddSpecificationAttributeCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SpecificationAttributeDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateSpecificationAttributeCommand() { Model = model });
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] string key, Delta<SpecificationAttributeDto> model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();


            var specification = await _mediator.Send(new GetQuery<SpecificationAttributeDto>() { Id = key });
            if (!specification.Any())
            {
                return NotFound();
            }
            var spec = specification.FirstOrDefault();
            model.Patch(spec);

            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateSpecificationAttributeCommand() { Model = spec });
                return Ok();
            }
            return BadRequest(ModelState);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Attributes))
                return Forbid();

            var specification = await _mediator.Send(new GetQuery<SpecificationAttributeDto>() { Id = key });
            if (!specification.Any())
            {
                return NotFound();
            }
            await _mediator.Send(new DeleteSpecificationAttributeCommand() { Model = specification.FirstOrDefault() });

            return Ok();
        }
    }
}
