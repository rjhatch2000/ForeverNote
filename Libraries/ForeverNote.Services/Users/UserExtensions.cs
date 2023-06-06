using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForeverNote.Services.Users
{
    public static class UserExtensions
    {
        public static string CouponSeparator => ";";

        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        public static string GetFullName(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var firstName = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.FirstName);
            var lastName = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.LastName);

            var fullName = "";
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                fullName = string.Format("{0} {1}", firstName, lastName);
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!string.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }
        /// <summary>
        /// Formats the user name
        /// </summary>
        /// <param name="user">Source</param>
        /// <param name="stripTooLong">Strip too long user name</param>
        /// <param name="maxLength">Maximum user name length</param>
        /// <returns>Formatted text</returns>
        public static string FormatUserName(this User user, UserNameFormat userNameFormat, bool stripTooLong = false, int maxLength = 0)
        {
            if (user == null)
                return string.Empty;

            var result = string.Empty;
            switch (userNameFormat)
            {
                case UserNameFormat.ShowEmails:
                    result = user.Email;
                    break;
                case UserNameFormat.ShowUsernames:
                    result = user.Username;
                    break;
                case UserNameFormat.ShowFullNames:
                    result = user.GetFullName();
                    break;
                case UserNameFormat.ShowFirstName:
                    result = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.FirstName);
                    break;
                default:
                    break;
            }

            if (stripTooLong && maxLength > 0)
            {
                result = CommonHelper.EnsureMaximumLength(result, maxLength);
            }

            return result;
        }

        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public static string[] ParseAppliedCouponCodes(this User user, string key)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = user.GetAttributeFromEntity<string>(key);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            return existingCouponCodes.Split(CouponSeparator);

        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public static string ApplyCouponCode(this User user, string key, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = user.GetAttributeFromEntity<string>(key);
            if (string.IsNullOrEmpty(existingCouponCodes))
            {
                return couponCode;
            }
            else
            {
                return string.Join(CouponSeparator, existingCouponCodes.Split(CouponSeparator).Append(couponCode).Distinct());
            }
        }
        /// <summary>
        /// Adds a coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public static string ApplyCouponCode(this User user, string key, string[] couponCodes)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = user.GetAttributeFromEntity<string>(key);
            if (string.IsNullOrEmpty(existingCouponCodes))
            {
                return string.Join(CouponSeparator, couponCodes);
            }
            else
            {
                var coupons = existingCouponCodes.Split(CouponSeparator).ToList();
                coupons.AddRange(couponCodes.ToList());
                return string.Join(CouponSeparator, coupons.Distinct());
            }
        }
        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public static string RemoveCouponCode(this User user, string key, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = user.GetAttributeFromEntity<string>(key);
            if (string.IsNullOrEmpty(existingCouponCodes))
            {
                return "";
            }
            else
            {
                return string.Join(CouponSeparator, existingCouponCodes.Split(CouponSeparator).Except(new List<string> { couponCode }).Distinct());
            }
        }

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        public static bool IsPasswordRecoveryTokenValid(this User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var cPrt = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.PasswordRecoveryToken);
            if (string.IsNullOrEmpty(cPrt))
                return false;

            if (!cPrt.Equals(token, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userSettings">User settings</param>
        /// <returns>Result</returns>
        public static bool IsPasswordRecoveryLinkExpired(this User user, UserSettings userSettings)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (userSettings == null)
                throw new ArgumentNullException(nameof(userSettings));

            if (userSettings.PasswordRecoveryLinkDaysValid == 0)
                return false;

            var geneatedDate = user.GetAttributeFromEntity<DateTime?>(SystemUserAttributeNames.PasswordRecoveryTokenDateGenerated);
            if (!geneatedDate.HasValue)
                return false;

            var daysPassed = (DateTime.UtcNow - geneatedDate.Value).TotalDays;
            if (daysPassed > userSettings.PasswordRecoveryLinkDaysValid)
                return true;

            return false;
        }

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        public static bool PasswordIsExpired(this User user, UserSettings userSettings)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //setting disabled for all
            if (userSettings.PasswordLifetime == 0)
                return false;

            int currentLifetime;
            if (!user.PasswordChangeDateUtc.HasValue)
                currentLifetime = int.MaxValue;
            else
                currentLifetime = (DateTime.UtcNow - user.PasswordChangeDateUtc.Value).Days;

            return currentLifetime >= userSettings.PasswordLifetime;
        }
    }
}
