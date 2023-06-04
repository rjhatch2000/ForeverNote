﻿using ForeverNote.Api.Commands.Models.Customers;
using ForeverNote.Api.DTOs.Customers;
using ForeverNote.Api.Extensions;
using ForeverNote.Services.Customers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Commands.Handlers.Customers
{
    public class UpdateCustomerAddressCommandHandler : IRequestHandler<UpdateCustomerAddressCommand, AddressDto>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerAddressCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<AddressDto> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(request.Customer.Id);
            if (customer != null)
            {
                var address = customer.Addresses.FirstOrDefault(x => x.Id == request.Address.Id);
                if (address != null)
                {
                    address = request.Address.ToEntity(address);
                    address.CustomerId = request.Customer.Id;
                    await _customerService.UpdateAddress(address);
                    return address.ToModel();
                }
            }
            return null;
        }
    }
}
