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
    public partial class ManufacturerController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public ManufacturerController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();

            var manufacturer = await _mediator.Send(new GetQuery<ManufacturerDto>() { Id = key });
            if (!manufacturer.Any())
                return NotFound();

            return Ok(manufacturer);
        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<ManufacturerDto>()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ManufacturerDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddManufacturerCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ManufacturerDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();


            var manufacturer = await _mediator.Send(new GetQuery<ManufacturerDto>() { Id = model.Id });
            if (!manufacturer.Any())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateManufacturerCommand() { Model = model });
                return Ok(model);
            }

            return BadRequest(ModelState);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] string key, Delta<ManufacturerDto> model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();

            var manufacturer = await _mediator.Send(new GetQuery<ManufacturerDto>() { Id = key });
            if (!manufacturer.Any())
            {
                return NotFound();
            }
            var man = manufacturer.FirstOrDefault();
            model.Patch(man);

            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateManufacturerCommand() { Model = man });
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Manufacturers))
                return Forbid();

            var manufacturer = await _mediator.Send(new GetQuery<ManufacturerDto>() { Id = key });
            if (!manufacturer.Any())
            {
                return NotFound();
            }

            await _mediator.Send(new DeleteManufacturerCommand() { Model = manufacturer.FirstOrDefault() });

            return Ok();
        }
    }
}
