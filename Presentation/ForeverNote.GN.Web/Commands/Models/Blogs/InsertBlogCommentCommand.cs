using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Web.Models.Blogs;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Blogs
{
    public class InsertBlogCommentCommand : IRequest<BlogComment>
    {
        public BlogPostModel Model { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
