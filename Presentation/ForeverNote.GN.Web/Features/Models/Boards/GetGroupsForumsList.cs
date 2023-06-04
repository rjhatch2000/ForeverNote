using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetGroupsForumsList : IRequest<IEnumerable<SelectListItem>>
    {
    }
}
