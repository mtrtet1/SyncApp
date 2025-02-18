using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
