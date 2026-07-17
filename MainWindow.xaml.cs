using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using TruStretch.Models;

namespace TruStretch
{
    public partial class MainWindow : Window
    {
        private AppConfig config = new AppConfig();

        private List<string> profiles = new();


        private string ConfigPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TruStretch",
                "config.json"
            );


        public MainWindow()
        {
            InitializeComponent();

            LoadData();
            HookEvents();

            RefreshCurrentResolutionLabel();
        }



        // ---------------- INIT ----------------

        private void HookEvents()
        {
            ProfileDropdown.SelectionChanged += ProfileDropdown_SelectionChanged;
        }



        // ---------------- WINDOW ----------------

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }


        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        // ---------------- ADD PROFILE ----------------

        private void AddProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(WidthBox.Text, out int width))
                return;

            if (!int.TryParse(HeightBox.Text, out int height))
                return;


            string name = $"{width}x{height}";


            if (!profiles.Contains(name))
            {
                profiles.Add(name);

                ProfileDropdown.Items.Add(name);


                config.Profiles.Add(new ResolutionProfile
                {
                    Name = name,
                    Width = width,
                    Height = height
                });


                SaveData();
            }


            WidthBox.Clear();
            HeightBox.Clear();


            RefreshCurrentResolutionLabel();
        }



        // ---------------- DELETE PROFILE ----------------

        private void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileDropdown.SelectedItem == null)
                return;


            string selected =
                ProfileDropdown.SelectedItem.ToString();


            profiles.Remove(selected);

            ProfileDropdown.Items.Remove(selected);


            config.Profiles.RemoveAll(
                x => x.Name == selected
            );


            if (config.LastProfile == selected)
                config.LastProfile = "";


            SaveData();

            RefreshCurrentResolutionLabel();
        }



        // ---------------- PROFILE SELECT ----------------

        private void ProfileDropdown_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (ProfileDropdown.SelectedItem == null)
                return;


            config.LastProfile =
                ProfileDropdown.SelectedItem.ToString();


            SaveData();


            RefreshCurrentResolutionLabel();
        }



        // ---------------- DEFAULT ----------------

        private void SaveDefault_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(DefaultWidthBox.Text, out int width))
                return;


            if (!int.TryParse(DefaultHeightBox.Text, out int height))
                return;


            config.DefaultWidth = width;
            config.DefaultHeight = height;


            SaveData();


            RefreshCurrentResolutionLabel();
        }



        // ---------------- CURRENT DISPLAY ----------------

        private void RefreshCurrentResolutionLabel()
        {
            if (config == null)
                return;


            if (!string.IsNullOrWhiteSpace(config.LastProfile))
            {
                CurrentResText.Text =
                    $"Current: {config.LastProfile}";
            }
            else
            {
                CurrentResText.Text =
                    "Current: N/A (Add a profile)";
            }
        }



        // ---------------- LOAD ----------------

        private void LoadData()
        {
            config = new AppConfig();


            if (!File.Exists(ConfigPath))
            {
                DefaultWidthBox.Text =
                    config.DefaultWidth.ToString();

                DefaultHeightBox.Text =
                    config.DefaultHeight.ToString();

                return;
            }


            try
            {
                string json =
                    File.ReadAllText(ConfigPath);


                config =
                    JsonSerializer.Deserialize<AppConfig>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    )
                    ?? new AppConfig();

            }
            catch
            {
                config = new AppConfig();

                File.Delete(ConfigPath);
            }



            profiles.Clear();

            ProfileDropdown.Items.Clear();


            foreach (ResolutionProfile profile in config.Profiles)
            {
                if (profile == null)
                    continue;


                if (string.IsNullOrWhiteSpace(profile.Name))
                    continue;


                profiles.Add(profile.Name);

                ProfileDropdown.Items.Add(profile.Name);
            }



            DefaultWidthBox.Text =
                config.DefaultWidth.ToString();


            DefaultHeightBox.Text =
                config.DefaultHeight.ToString();
        }



        // ---------------- SAVE ----------------

        private void SaveData()
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(ConfigPath)
            );


            File.WriteAllText(
                ConfigPath,
                JsonSerializer.Serialize(
                    config,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                )
            );
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}