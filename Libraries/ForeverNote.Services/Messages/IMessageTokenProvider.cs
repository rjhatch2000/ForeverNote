using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        Task AddUserTokens(LiquidObject liquidObject, User user, Language language);
        Task AddNoteTokens(LiquidObject liquidObject, Note note, Language language);
        string[] GetListOfCampaignAllowedTokens();
        string[] GetListOfAllowedTokens();
        string[] GetListOfUserReminderAllowedTokens(UserReminderRuleEnum rule);
    }
}