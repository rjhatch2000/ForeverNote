using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Users;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class InstallationService
    {
        protected virtual async Task InstallUsers(string defaultUserEmail, string defaultUserPassword)
        {
            //admin user
            var adminUser = new User
            {
                UserGuid = Guid.NewGuid(),
                Email = defaultUserEmail,
                Username = defaultUserEmail,
                Password = defaultUserPassword,
                PasswordFormatId = (int)PasswordFormat.Clear,
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                PasswordChangeDateUtc = DateTime.UtcNow,
                FirstName = "John",
                LastName = "Smith",
            };
            ////adminUser.Addresses.Add(defaultAdminUserAddress);
            ////adminUser.BillingAddress = defaultAdminUserAddress;
            ////adminUser.ShippingAddress = defaultAdminUserAddress;
            ////adminUser.Groups.Add(crAdministrators.Id);
            ////adminUser.Groups.Add(crRegistered.Id);
            ////adminUser.GenericAttributes.Add(new GenericAttribute() { Key = SystemUserFieldNames.FirstName, Value = "John", StoreId = "" });
            ////adminUser.GenericAttributes.Add(new GenericAttribute() { Key = SystemUserFieldNames.LastName, Value = "Smith", StoreId = "" });
            await _userRepository.InsertAsync(adminUser);

            //////Anonymous user
            ////var anonymousUser = new User
            ////{
            ////    Email = "builtin@anonymous.com",
            ////    UserGuid = Guid.NewGuid(),
            ////    PasswordFormatId = (int)PasswordFormat.Clear,
            ////    AdminComment = "Built-in system guest record used for anonymous requests.",
            ////    Active = true,
            ////    IsSystemAccount = true,
            ////    SystemName = SystemUserNames.Anonymous,
            ////    CreatedOnUtc = DateTime.UtcNow,
            ////    LastActivityDateUtc = DateTime.UtcNow,
            ////};
            ////anonymousUser.Groups.Add(crGuests.Id);
            ////await _userRepository.InsertAsync(anonymousUser);

            //////search engine (crawler) built-in user
            ////var searchEngineUser = new User
            ////{
            ////    Email = "builtin@search_engine_record.com",
            ////    UserGuid = Guid.NewGuid(),
            ////    PasswordFormatId = PasswordFormat.Clear,
            ////    AdminComment = "Built-in system guest record used for requests from search engines.",
            ////    Active = true,
            ////    IsSystemAccount = true,
            ////    SystemName = SystemUserNames.SearchEngine,
            ////    CreatedOnUtc = DateTime.UtcNow,
            ////    LastActivityDateUtc = DateTime.UtcNow,
            ////};
            ////searchEngineUser.Groups.Add(crGuests.Id);
            ////await _userRepository.InsertAsync(searchEngineUser);


            //built-in user for background tasks
            var backgroundTaskUser = new User
            {
                Email = "builtin@background-task-record.com",
                UserGuid = Guid.NewGuid(),
                PasswordFormatId = (int)PasswordFormat.Clear,
                Password = Guid.NewGuid().ToString(),
                ////AdminComment = "Built-in system record used for background tasks.",
                Active = true,
                IsSystemAccount = true,
                SystemName = SystemUserNames.BackgroundTask,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };
            ////backgroundTaskUser.Groups.Add(crGuests.Id);
            await _userRepository.InsertAsync(backgroundTaskUser);

        }
    }
}
