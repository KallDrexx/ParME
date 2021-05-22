using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Parme.Editor.Settings
{
    public class AppSettings
    {
        private const int RecentListSize = 10;
        private const string SettingsFileName = "Parme.config";
        private static readonly string SettingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Parme");
        private static readonly string FullSettingsPath = Path.Combine(SettingsFolder, SettingsFileName);

        // ReSharper disable once MemberCanBePrivate.Global
        public List<string> RecentlyOpenedFiles { get; set; } = new List<string>();
        public AutoMoveTextureOption AutoMoveTextureOption { get; set; } = AutoMoveTextureOption.Ask;

        public static AppSettings Load()
        {
            try
            {
                var json = File.ReadAllText(FullSettingsPath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                return settings ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void AddOpenedFileName(string fileName)
        {
            RecentlyOpenedFiles ??= new List<string>();
            RecentlyOpenedFiles.Remove(fileName);
            
            RecentlyOpenedFiles.Insert(0, fileName);
            for (var x = RecentlyOpenedFiles.Count - 1; x > RecentListSize; x--)
            {
                RecentlyOpenedFiles.RemoveAt(x);
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            var tempFile = Path.GetTempFileName();
            var backupFile = FullSettingsPath + ".bak";
            
            File.WriteAllText(tempFile, json);

            if (File.Exists(FullSettingsPath))
            {
                File.Replace(tempFile, FullSettingsPath, backupFile);
            }
            else
            {
                if (!Directory.Exists(SettingsFolder))
                {
                    Directory.CreateDirectory(SettingsFolder);
                }
                
                File.Move(tempFile, FullSettingsPath);
            }
        }
    }
}