using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        Task AddCustomerTokens(LiquidObject liquidObject, Customer customer, Language language, CustomerNote customerNote = null);
        Task AddProductTokens(LiquidObject liquidObject, Product product, Language language);
        string[] GetListOfCampaignAllowedTokens();
        string[] GetListOfAllowedTokens();
        string[] GetListOfCustomerReminderAllowedTokens(CustomerReminderRuleEnum rule);
    }
}