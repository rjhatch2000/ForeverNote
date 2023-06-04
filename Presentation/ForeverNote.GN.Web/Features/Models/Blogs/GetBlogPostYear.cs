using ForeverNote.Web.Models.Blogs;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Blogs
{
    public class GetBlogPostYear : IRequest<IList<BlogPostYearModel>>
    {
    }
}
