using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Services.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MediatR;

namespace ForeverNote.Services.Messages
{
    public partial class PopupService : IPopupService
    {
        private readonly IRepository<PopupActive> _popupActiveRepository;
        private readonly IRepository<PopupArchive> _popupArchiveRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popupActiveRepository">Popup Active repository</param>
        /// <param name="popupArchiveRepository">Popup Archive repository</param>
        /// <param name="mediator">Mediator</param>
        public PopupService(IRepository<PopupActive> popupActiveRepository,
            IRepository<PopupArchive> popupArchiveRepository,
            IMediator mediator)
        {
            _popupActiveRepository = popupActiveRepository;
            _popupArchiveRepository = popupArchiveRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Inserts a popup
        /// </summary>
        /// <param name="popup">Popup</param>        
        public virtual async Task InsertPopupActive(PopupActive popup)
        {
            if (popup == null)
                throw new ArgumentNullException("popup");

            await _popupActiveRepository.InsertAsync(popup);

            //event notification
            await _mediator.EntityInserted(popup);
        }


        public virtual async Task<PopupActive> GetActivePopupByUserId(string userId)
        {
            var query = from c in _popupActiveRepository.Table
                        where c.UserId == userId
                        orderby c.CreatedOnUtc
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task MovepopupToArchive(string id, string userId)
        {
            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(id))
                return;

            var query = from c in _popupActiveRepository.Table
                        where c.UserId == userId && c.Id == id
                        select c;

            var popup = await query.FirstOrDefaultAsync();
            if (popup != null)
            {
                var archiveBanner = new PopupArchive()
                {
                    Body = popup.Body,
                    BACreatedOnUtc = popup.CreatedOnUtc,
                    CreatedOnUtc = DateTime.UtcNow,
                    UserActionId = popup.UserActionId,
                    UserId = popup.UserId,
                    PopupActiveId = popup.Id,
                    PopupTypeId = popup.PopupTypeId,
                    Name = popup.Name,
                };
                await _popupArchiveRepository.InsertAsync(archiveBanner);
                await _popupActiveRepository.DeleteAsync(popup);
            }

        }

    }
}
