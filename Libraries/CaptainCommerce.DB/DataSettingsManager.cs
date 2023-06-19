using System.Text.Json;

namespace CaptainCommerce.DB
{
    /// <summary>
    /// Manager of data settings (connection string)
    /// </summary>
    public static class DataSettingsManager
    {

        private static DataSettings _dataSettings;

        private static bool? _databaseIsInstalled;

        /// <summary>
        /// Load settings
        /// </summary>
        public static DataSettings LoadSettings(string settingsPath, bool reloadSettings = false)
        {
            if (!reloadSettings && _dataSettings != null)
                return _dataSettings;

            if (!File.Exists(settingsPath))
                return new DataSettings();

            try
            {
                var text = File.ReadAllText(settingsPath);
                _dataSettings = JsonSerializer.Deserialize<DataSettings>(text);
            }
            catch
            {
                //Try to read file
                var connectionString = File.ReadLines(settingsPath).FirstOrDefault();
                _dataSettings = new DataSettings() { ConnectionString = connectionString, DbProvider = DbProvider.MongoDB };

            }
            return _dataSettings;
        }

        public static DataSettings LoadDataSettings(DataSettings dataSettings)
        {
            _dataSettings = dataSettings;
            return _dataSettings;
        }

        /// <summary>
        /// Returns a value indicating whether database is already installed
        /// </summary>
        /// <returns></returns>
        public static bool DatabaseIsInstalled(string settingsPath)
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var settings = _dataSettings ?? LoadSettings(settingsPath);
                _databaseIsInstalled = settings != null && !string.IsNullOrEmpty(settings.ConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        public static void ResetCache()
        {
            _databaseIsInstalled = false;
        }

        /// <summary>
        /// Save settings to a file
        /// </summary>
        /// <param name="settings"></param>
        public static async Task SaveSettings(string filePath, DataSettings settings)
        {
            if (!File.Exists(filePath))
                using var fs = File.Create(filePath);
            var data = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, data);
        }
    }
}
