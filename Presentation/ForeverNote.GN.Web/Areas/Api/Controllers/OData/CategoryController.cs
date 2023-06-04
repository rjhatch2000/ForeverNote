using ForeverNote.Api.Commands.Models.Catalog;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Services.Security;
using MediatR;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Deltas;

namespace ForeverNote.Web.Areas.Api.Controllers.OData
{
    public partial class CategoryController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public CategoryController(
            IMediator mediator,
            IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
            if (!category.Any())
                return NotFound();

            return Ok(category.FirstOrDefault());
        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<CategoryDto>()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddCategoryCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryDto model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = model.Id });
            if (!category.Any())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateCategoryCommand() { Model = model });
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] string key, Delta<CategoryDto> model)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
            if (!category.Any())
            {
                return NotFound();
            }
            var cat = category.FirstOrDefault();
            model.Patch(cat);

            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateCategoryCommand() { Model = cat });
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Categories))
                return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
            if (!category.Any())
            {
                return NotFound();
            }

            await _mediator.Send(new DeleteCategoryCommand() { Model = category.FirstOrDefault() });

            return Ok();
        }
    }
}
