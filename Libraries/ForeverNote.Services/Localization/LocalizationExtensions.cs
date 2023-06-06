using ForeverNote.Core;
using ForeverNote.Core.Configuration;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Configuration;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ForeverNote.Services.Localization
{
    public static class LocalizationExtensions
    {

        /// <summary>
        /// Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector, string languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : ParentEntity, ILocalizedEntity
        {
            return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public static TPropType GetLocalized<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector, string languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : ParentEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            TPropType result = default(TPropType);
            string resultStr = string.Empty;

            //load localized value
            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            if (!String.IsNullOrEmpty(languageId))
            {
                if (entity.Locales.Any())
                {
                    var en = entity.Locales.FirstOrDefault(x => x.LanguageId == languageId && x.LocaleKey == localeKey);
                    if (en != null)
                    {
                        resultStr = en.LocaleValue;
                        if (!String.IsNullOrEmpty(resultStr))
                            result = CommonHelper.To<TPropType>(resultStr);
                    }
                }
            }

            //set default value if required
            if (String.IsNullOrEmpty(resultStr) && returnDefaultValue)
            {
                result = (TPropType)(propInfo.GetValue(entity));
            }

            return result;
        }


        /// <summary>
        /// Get localized property of setting
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public static string GetLocalizedSetting<T>(this T settings, ISettingService settingService,
            Expression<Func<T, string>> keySelector, string languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);

            //we do not support localized settings per store (overridden store settings)
            var setting = settingService.GetSetting(key);
            if (setting == null)
                return null;

            return setting.GetLocalized(x => x.Value, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// Get localized value of enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService, IWorkContext workContext)
            where T : struct
        {
            if (workContext == null)
                throw new ArgumentNullException("workContext");

            return GetLocalizedEnum(enumValue, localizationService, workContext.WorkingLanguage.Id);
        }
        /// <summary>
        /// Get localized value of enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService, string languageId)
            where T : struct
        {
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");

            if (!typeof(T).GetTypeInfo().IsEnum) throw new ArgumentException("T must be an enumerated type");

            //localized value
            string resourceName = string.Format("Enums.{0}.{1}",
                typeof(T),
                enumValue.ToString());
            string result = localizationService.GetResource(resourceName, languageId, false, "", true);

            //set default value if required
            if (String.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }
    }
}
