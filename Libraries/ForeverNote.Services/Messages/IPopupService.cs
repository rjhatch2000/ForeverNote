using ForeverNote.Core.Domain.Messages;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial interface IPopupService
    {
        /// <summary>
        /// Inserts a popup
        /// </summary>
        /// <param name="popup">Popup</param>        
        Task InsertPopupActive(PopupActive popup);
        /// <summary>
        /// Gets active banner for user
        /// </summary>
        /// <returns>BannerActive</returns>
        Task<PopupActive> GetActivePopupByUserId(string userId);

        /// <summary>
        /// Move popup to archive
        /// </summary>
        Task MovepopupToArchive(string id, string userId);

    }
}
