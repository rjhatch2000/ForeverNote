﻿using ForeverNote.Api.DTOs.Customers;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Customers
{
    public class UpdateCustomerRoleCommand : IRequest<CustomerRoleDto>
    {
        public CustomerRoleDto Model { get; set; }
    }
}
