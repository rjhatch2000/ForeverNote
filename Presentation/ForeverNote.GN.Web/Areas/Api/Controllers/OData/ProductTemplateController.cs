﻿using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Api.Controllers.OData
{
    public partial class ProductTemplateController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public ProductTemplateController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
                return Forbid();

            var template = await _mediator.Send(new GetMessageTemplateQuery() { Id = key, TemplateName = typeof(Core.Domain.Catalog.ProductTemplate).Name });
            if (!template.Any())
                return NotFound();

            return Ok(template.FirstOrDefault());

        }

        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
                return Forbid();

            return Ok(await _mediator.Send(new GetMessageTemplateQuery() { TemplateName = typeof(Core.Domain.Catalog.ProductTemplate).Name }));
        }
    }
}
