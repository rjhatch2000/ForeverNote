using ForeverNote.Api.DTOs.Shipping;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Api.Controllers.OData
{
    public partial class PickupPointController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public PickupPointController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.ShippingSettings))
                return Forbid();

            var points = await _mediator.Send(new GetQuery<PickupPointDto>() { Id = key });
            if (!points.Any())
                return NotFound();

            return Ok(points.FirstOrDefault());

        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.ShippingSettings))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<PickupPointDto>()));
        }
    }
}
