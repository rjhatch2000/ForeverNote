using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Services.Common;
using ForeverNote.Services.Media;
using ForeverNote.Web.Features.Models.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Customers
{
    public class GetAvatarHandler : IRequestHandler<GetAvatar, CustomerAvatarModel>
    {
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;

        public GetAvatarHandler(IPictureService pictureService,
            MediaSettings mediaSettings)
        {
            _pictureService = pictureService;
            _mediaSettings = mediaSettings;
        }

        public async Task<CustomerAvatarModel> Handle(GetAvatar request, CancellationToken cancellationToken)
        {
            var model = new CustomerAvatarModel();
            model.AvatarUrl = await _pictureService.GetPictureUrl(request.Customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId),
                _mediaSettings.AvatarPictureSize, false);

            return model;
        }
    }
}
