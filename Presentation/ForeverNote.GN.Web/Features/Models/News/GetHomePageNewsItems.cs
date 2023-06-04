using ForeverNote.Web.Models.News;
using MediatR;

namespace ForeverNote.Web.Features.Models.News
{
    public class GetHomePageNewsItems : IRequest<HomePageNewsItemsModel>
    {
    }
}
