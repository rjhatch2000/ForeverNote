using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// Note tag service interface
    /// </summary>
    public partial interface IUserTagService
    {
        /// <summary>
        /// Delete a user tag
        /// </summary>
        /// <param name="noteTag">User tag</param>
        Task DeleteUserTag(UserTag userTag);

        /// <summary>
        /// Gets all user tags
        /// </summary>
        /// <returns>User tags</returns>
        Task<IList<UserTag>> GetAllUserTags();

        /// <summary>
        /// Gets all user for tag id
        /// </summary>
        /// <returns>Users</returns>
        Task<IPagedList<User>> GetUsersByTag(string userTagId = "", int pageIndex = 0, int pageSize = 2147483647);
        /// <summary>
        /// Gets user tag
        /// </summary>
        /// <param name="userTagId">User tag identifier</param>
        /// <returns>Note tag</returns>
        Task<UserTag> GetUserTagById(string userTagId);

        /// <summary>
        /// Gets user tag by name
        /// </summary>
        /// <param name="name">User tag name</param>
        /// <returns>User tag</returns>
        Task<UserTag> GetUserTagByName(string name);

        /// <summary>
        /// Gets user tags search by name
        /// </summary>
        /// <param name="name">User tags name</param>
        /// <returns>User tags</returns>
        Task<IList<UserTag>> GetUserTagsByName(string name);

        /// <summary>
        /// Inserts a user tag
        /// </summary>
        /// <param name="userTag">User tag</param>
        Task InsertUserTag(UserTag userTag);

        /// <summary>
        /// Insert tag to a user
        /// </summary>
        Task InsertTagToUser(string userTagId, string userId);

        /// <summary>
        /// Delete tag from a user
        /// </summary>
        Task DeleteTagFromUser(string userTagId, string userId);

        /// <summary>
        /// Updates the user tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        Task UpdateUserTag(UserTag userTag);

        /// <summary>
        /// Get number of users
        /// </summary>
        /// <param name="userTagId">User tag identifier</param>
        /// <returns>Number of notes</returns>
        Task<int> GetUserCount(string userTagId);

        /// <summary>
        /// Gets user tag notes for user tag
        /// </summary>
        /// <param name="userTagId">User tag id</param>
        /// <returns>User tag notes</returns>
        Task<IList<UserTagNote>> GetUserTagNotes(string userTagId);

        /// <summary>
        /// Gets user tag notes for user tag
        /// </summary>
        /// <param name="userTagId">User tag id</param>
        /// <param name="noteId">Note id</param>
        /// <returns>User tag note</returns>
        Task<UserTagNote> GetUserTagNote(string userTagId, string noteId);

        /// <summary>
        /// Gets user tag note
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns>User tag note</returns>
        Task<UserTagNote> GetUserTagNoteById(string id);

        /// <summary>
        /// Inserts a user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        Task InsertUserTagNote(UserTagNote userTagNote);

        /// <summary>
        /// Updates the user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        Task UpdateUserTagNote(UserTagNote userTagNote);

        /// <summary>
        /// Delete a user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        Task DeleteUserTagNote(UserTagNote userTagNote);

    }
}
