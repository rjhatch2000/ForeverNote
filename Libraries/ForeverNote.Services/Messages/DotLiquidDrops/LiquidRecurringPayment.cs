using DotLiquid;
using ForeverNote.Core.Domain.Orders;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidRecurringPayment : Drop
    {
        private RecurringPayment _recurringPayment;

        public LiquidRecurringPayment(RecurringPayment recurringPayment)
        {
            this._recurringPayment = recurringPayment;

            AdditionalTokens = new Dictionary<string, string>();
        }

        public string Id
        {
            get { return _recurringPayment.Id.ToString(); }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}