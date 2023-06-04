﻿using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetForumBreadcrumb : IRequest<ForumBreadcrumbModel>
    {
        public string ForumGroupId { get; set; }
        public string ForumId { get; set; }
        public string ForumTopicId { get; set; }
    }
}
