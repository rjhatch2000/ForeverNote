﻿using ForeverNote.Core.Domain.Common;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Stores;
using ForeverNote.Web.Features.Models.Common;
using ForeverNote.Web.Features.Models.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Customers
{
    public class GetAddressListHandler : IRequestHandler<GetAddressList, CustomerAddressListModel>
    {
        private readonly ICountryService _countryService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IMediator _mediator;
        public GetAddressListHandler(ICountryService countryService,
            IStoreMappingService storeMappingService,
            IMediator mediator)
        {
            _countryService = countryService;
            _storeMappingService = storeMappingService;
            _mediator = mediator;
        }

        public async Task<CustomerAddressListModel> Handle(GetAddressList request, CancellationToken cancellationToken)
        {
            var model = new CustomerAddressListModel();
            var addresses = new List<Address>();
            foreach (var item in request.Customer.Addresses)
            {
                if (string.IsNullOrEmpty(item.CountryId))
                {
                    addresses.Add(item);
                    continue;
                }
                var country = await _countryService.GetCountryById(item.CountryId);
                if (country != null || _storeMappingService.Authorize(country))
                {
                    addresses.Add(item);
                    continue;
                }
            }

            foreach (var address in addresses)
            {
                var countries = await _countryService.GetAllCountries(request.Language.Id);
                var addressModel = await _mediator.Send(new GetAddressModel() {
                    Language = request.Language,
                    Store = request.Store,
                    Model = null,
                    Address = address,
                    ExcludeProperties = false,
                    LoadCountries = () => countries
                });
                model.Addresses.Add(addressModel);
            }

            return model;
        }
    }
}
