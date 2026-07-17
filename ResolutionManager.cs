using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using TruStretch.Models;

namespace TruStretch
{
    public static class ResolutionManager
    {
        private static string NirCmdPath =>
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Tools",
                "nircmdc64.exe"
            );


        private static string ConfigPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TruStretch",
                "config.json"
            );



        public static void ApplySmartToggle()
        {
            try
            {
                if (!File.Exists(NirCmdPath))
                {
                    MessageBox.Show(
                        "NirCmd not found:\n" + NirCmdPath
                    );
                    return;
                }


                AppConfig config = LoadConfig();


                int currentWidth =
                    (int)SystemParameters.PrimaryScreenWidth;

                int currentHeight =
                    (int)SystemParameters.PrimaryScreenHeight;



                // Currently NOT default:
                // return to fallback resolution
                if (currentWidth != config.DefaultWidth ||
                    currentHeight != config.DefaultHeight)
                {
                    SetResolution(
                        config.DefaultWidth,
                        config.DefaultHeight
                    );

                    return;
                }



                // Currently default:
                // apply selected profile

                if (string.IsNullOrWhiteSpace(config.LastProfile))
                {
                    return;
                }


                string[] parts =
                    config.LastProfile.Split('x');


                if (parts.Length != 2)
                    return;


                if (int.TryParse(parts[0], out int width) &&
                    int.TryParse(parts[1], out int height))
                {
                    SetResolution(width, height);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Resolution error:\n" + ex.Message
                );
            }
        }



        public static void SetResolution(
            int width,
            int height)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = NirCmdPath,
                Arguments = $"setdisplay {width} {height} 32",
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }



        private static AppConfig LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new AppConfig();


            try
            {
                return JsonSerializer.Deserialize<AppConfig>(
                    File.ReadAllText(ConfigPath),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                )
                ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }
    }
}