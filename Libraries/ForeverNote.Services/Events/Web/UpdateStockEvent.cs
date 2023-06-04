﻿using ForeverNote.Core.Domain.Catalog;
using MediatR;

namespace ForeverNote.Services.Events.Web
{
    public class UpdateStockEvent : INotification
    {
        private readonly Product _product;

        public UpdateStockEvent(Product product)
        {
            _product = product;
        }

        public Product Result { get { return _product; } }
    }
}
