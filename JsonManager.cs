using System;
using System.IO;
using System.Text.Json;
using TruStretch.Models;

namespace TruStretch.Core
{
    public static class JsonManager
    {
        private static readonly string ConfigFolder =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TruStretch");

        private static readonly string ConfigFile =
            Path.Combine(ConfigFolder, "config.json");

        public static AppConfig Load()
        {
            try
            {
                if (!Directory.Exists(ConfigFolder))
                    Directory.CreateDirectory(ConfigFolder);

                if (!File.Exists(ConfigFile))
                {
                    var config = new AppConfig();
                    Save(config);
                    return config;
                }

                string json = File.ReadAllText(ConfigFile);

                var configData = JsonSerializer.Deserialize<AppConfig>(json);

                return configData ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }

        public static void Save(AppConfig config)
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            string json = JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(ConfigFile, json);
        }
    }
}