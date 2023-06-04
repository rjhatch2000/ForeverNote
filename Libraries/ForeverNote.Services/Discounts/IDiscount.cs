using ForeverNote.Core.Plugins;
using System.Collections.Generic;

namespace ForeverNote.Services.Discounts
{
    /// <summary>
    /// Represents a discount requirement rule
    /// </summary>
    public partial interface IDiscount : IPlugin
    {
        IList<IDiscountRequirementRule> GetRequirementRules();
    }
}
