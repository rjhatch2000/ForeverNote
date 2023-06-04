using ForeverNote.Web.Models.Blogs;
using MediatR;

namespace ForeverNote.Web.Features.Models.Blogs
{
    public class GetHomePageBlog: IRequest<HomePageBlogItemsModel>
    {
    }
}
