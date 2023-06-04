using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Authentication;
using ForeverNote.Web.Features.Models.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Customers
{
    public class GetTwoFactorAuthenticationHandler : IRequestHandler<GetTwoFactorAuthentication, CustomerInfoModel.TwoFactorAuthenticationModel>
    {
        private readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;
        private readonly CustomerSettings _customerSetting;

        public GetTwoFactorAuthenticationHandler(ITwoFactorAuthenticationService twoFactorAuthenticationService,
            CustomerSettings customerSetting)
        {
            _twoFactorAuthenticationService = twoFactorAuthenticationService;
            _customerSetting = customerSetting;
        }

        public async Task<CustomerInfoModel.TwoFactorAuthenticationModel> Handle(GetTwoFactorAuthentication request, CancellationToken cancellationToken)
        {
            var secretkey = Guid.NewGuid().ToString();
            var setupInfo = await _twoFactorAuthenticationService.GenerateCodeSetup(secretkey, request.Customer, request.Language, _customerSetting.TwoFactorAuthenticationType);
            var model = new CustomerInfoModel.TwoFactorAuthenticationModel {
                CustomValues = setupInfo.CustomValues,
                SecretKey = secretkey,
                TwoFactorAuthenticationType = _customerSetting.TwoFactorAuthenticationType
            };
            return model;
        }
    }
}
