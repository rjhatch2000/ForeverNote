﻿using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetManufacturerTemplateViewPath : IRequest<string>
    {
        public string TemplateId { get; set; }
    }
}
