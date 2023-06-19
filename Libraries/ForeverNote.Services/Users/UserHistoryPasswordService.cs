using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public class UserHistoryPasswordService : IUserHistoryPasswordService
    {
        private readonly IRepository<UserHistoryPassword> _userHistoryPasswordNoteRepository;
        private readonly IMediator _mediator;

        public UserHistoryPasswordService(IRepository<UserHistoryPassword> userHistoryPasswordNoteRepository,
            IMediator mediator)
        {
            _userHistoryPasswordNoteRepository = userHistoryPasswordNoteRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Insert a user history password
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task InsertUserPassword(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var chp = new UserHistoryPassword
            {
                Password = user.Password,
                PasswordFormatId = user.PasswordFormatId,
                PasswordSalt = user.PasswordSalt,
                UserId = user.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            await _userHistoryPasswordNoteRepository.InsertAsync(chp);

            //event notification
            await _mediator.EntityInserted(chp);
        }

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual async Task<IList<UserHistoryPassword>> GetPasswords(string userId, int passwordsToReturn)
        {
            return await Task.FromResult(_userHistoryPasswordNoteRepository
                    .Table
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(password => password.CreatedOnUtc)
                    .Take(passwordsToReturn)
                    .ToList());
        }

    }
}
