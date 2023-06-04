using ForeverNote.Api.DTOs.Common;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Api.Controllers.OData
{
    public partial class LanguageController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public LanguageController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Languages))
                return Forbid();

            var language = await _mediator.Send(new GetQuery<LanguageDto>() { Id = key });
            if (!language.Any())
                return NotFound();

            return Ok(language.FirstOrDefault());
        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Languages))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<LanguageDto>()));
        }
    }
}
