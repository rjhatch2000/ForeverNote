using DotLiquid;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Common;
using ForeverNote.Services.Users;
using System.Collections.Generic;
using System.Net;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidUser : Drop
    {
        private readonly CommonSettings _commonSettings;
        private User _user;
        private Language _language;

        public LiquidUser(
            CommonSettings commonSettings,
            User user,
            Language language
        )
        {
            _commonSettings = commonSettings;
            _user = user;
            _language = language;

            AdditionalTokens = new Dictionary<string, string>();
        }

        public string Email
        {
            get { return _user.Email; }
        }

        public string Username
        {
            get { return _user.Username; }
        }

        public string FullName
        {
            get { return _user.GetFullName(); }
        }

        public string FirstName
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.FirstName); }
        }

        public string LastName
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.LastName); }
        }

        public string Gender 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.Gender); }
        }

        public string DateOfBirth 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.DateOfBirth); }
        }        

        public string Company 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.Company); }
        }

        public string StreetAddress 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.StreetAddress); }
        }

        public string StreetAddress2 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.StreetAddress2); }
        }

        public string ZipPostalCode 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.ZipPostalCode); }
        }

        public string City 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.City); }
        }

        public string Phone 
        {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.Phone); }
        }

        public string Fax {
            get { return _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.Fax); }
        }

        public string PasswordRecoveryURL
        {
            get { return string.Format("{0}passwordrecovery/confirm?token={1}&email={2}", (_commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url), _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.PasswordRecoveryToken), WebUtility.UrlEncode(_user.Email)); }
        }

        public string AccountActivationURL
        {
            get { return string.Format("{0}user/activation?token={1}&email={2}", (_commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url), _user.GetAttributeFromEntity<string>(SystemUserAttributeNames.AccountActivationToken), WebUtility.UrlEncode(_user.Email)); ; }
        }

        public string WishlistURLForUser
        {
            get { return string.Format("{0}wishlist/{1}", (_commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url), _user.UserGuid); }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}