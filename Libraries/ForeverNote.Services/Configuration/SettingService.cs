using ForeverNote.Core.Caching;
using ForeverNote.Core.Caching.Constants;
using ForeverNote.Core.Configuration;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForeverNote.Services.Configuration
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public class SettingService : ISettingService
    {
        #region Fields

        private readonly IRepository<Setting> _settingRepository;
        private readonly ICacheBase _cacheBase;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheBase">Cache manager</param>
        /// <param name="settingRepository">Setting repository</param>
        public SettingService(ICacheBase cacheBase,
            IRepository<Setting> settingRepository)
        {
            _cacheBase = cacheBase;
            _settingRepository = settingRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns>Setting</returns>
        private IList<Setting> GetSettingsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return _settingRepository.Table.Where(x => x.Name == name.ToLowerInvariant()).ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual async Task InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.InsertAsync(setting);

            //cache
            if (clearCache)
                await _cacheBase.Clear();

        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual async Task UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.UpdateAsync(setting);

            //cache
            if (clearCache)
                await _cacheBase.Clear();

        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual async Task DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.DeleteAsync(setting);

            //cache
            await _cacheBase.Clear();

        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        public virtual Task<Setting> GetSettingById(string settingId)
        {
            return _settingRepository.GetByIdAsync(settingId);
        }


        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Setting value</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var keyCache = string.Format(CacheKey.SETTINGS_BY_KEY, key);
            return _cacheBase.Get(keyCache, () =>
            {
                var settings = GetSettingsByName(key);
                key = key.Trim().ToLowerInvariant();
                if (!settings.Any()) return defaultValue;
                
                var setting = settings.FirstOrDefault();
                return setting != null ? JsonSerializer.Deserialize<T>(setting.Metadata) : defaultValue;
            });
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual async Task SetSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            key = key.Trim().ToLowerInvariant();

            var settings = GetSettingsByName(key);
            var setting = settings.Any() ? settings.FirstOrDefault() : null;
            if (setting != null)
            {
                //update
                setting.Metadata = JsonSerializer.Serialize(value);
                await UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                var metadata = JsonSerializer.Serialize(value);
                setting = new Setting {
                    Name = key.ToLowerInvariant(),
                    Metadata = metadata
                };
                await InsertSetting(setting, clearCache);
            }
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            return _settingRepository.Table.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="storeId">Store identifier for which settings should be loaded</param>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            return (T)LoadSetting(typeof(T));
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="type">Type</param>
        public virtual ISettings LoadSetting(Type type)
        {
            var key = string.Format(CacheKey.SETTINGS_BY_KEY, type.Name);
            return _cacheBase.Get(key, () =>
            {
                var settings = GetSettingsByName(type.Name);
                if (!settings.Any()) return Activator.CreateInstance(type) as ISettings;
                var setting = settings.FirstOrDefault();

                if (setting != null) return JsonSerializer.Deserialize(setting.Metadata, type) as ISettings;
                return Activator.CreateInstance(type) as ISettings;
            });
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="settings">Setting instance</param>
        public virtual async Task SaveSetting<T>(T settings) where T : ISettings, new()
        {
            var dbSettings = GetSettingsByName(typeof(T).Name);
            var setting = dbSettings.Any() ? dbSettings.FirstOrDefault() : null;
            if (setting != null)
            {
                //update
                setting.Metadata = JsonSerializer.Serialize(settings);
                await UpdateSetting(setting);
            }
            else
            {
                //insert
                setting = new Setting {
                    Name = typeof(T).Name.ToLowerInvariant(),
                    Metadata = JsonSerializer.Serialize(settings),
                };
                await InsertSetting(setting);
            }

            //and now clear cache
            await ClearCache();
        }

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual async Task DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = GetSettingsByName(typeof(T).Name);

            foreach (var setting in settingsToDelete)
                await DeleteSetting(setting);
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual async Task ClearCache()
        {
            await _cacheBase.RemoveByPrefix(CacheKey.SETTINGS_PATTERN_KEY);
        }

        #endregion
    }
}