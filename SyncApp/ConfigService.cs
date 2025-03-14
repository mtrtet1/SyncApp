using System.Text.Json;

namespace SyncApp
{
    public class ConfigService
    {
        public string DbConnectionString { get; set; }
        public string ApiUrl { get; set; }
        public string ApiToken { get; set; }
        public int SyncInterval { get; set; } = 5; // Default 5 minutes

        private static readonly string ConfigFilePath = "config.json";

        public static ConfigService LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                string json = File.ReadAllText(ConfigFilePath);
                return JsonSerializer.Deserialize<ConfigService>(json);
            }
            else
            {
                var defaultConfig = new ConfigService();

                defaultConfig.ApiUrl = "https://storizone.com";
                defaultConfig.ApiToken = "e4dc10f4-9488-4df7-a23c-feb538e694c5";
                defaultConfig.DbConnectionString = "Data Source=MMRAISH;Initial Catalog=Kids;Integrated Security=true;";

                File.WriteAllText(ConfigFilePath, JsonSerializer.Serialize(defaultConfig));
                return defaultConfig;
            }
        }

        public void SaveConfig()
        {
            File.WriteAllText(ConfigFilePath, JsonSerializer.Serialize(this));
        }
    }
}
