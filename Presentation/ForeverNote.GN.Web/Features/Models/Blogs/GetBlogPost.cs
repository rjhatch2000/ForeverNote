using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Web.Models.Blogs;
using MediatR;

namespace ForeverNote.Web.Features.Models.Blogs
{
    public class GetBlogPost : IRequest<BlogPostModel>
    {
        public BlogPost BlogPost { get; set; }
    }
}
