using System;
using System.Collections.Generic;
using System.IO;

namespace CHI_MVCCodeHelper
{
    [Serializable]
    public static class Settings
    {
        private static readonly List<SettingsItem> settingsList = new List<SettingsItem>();
        private static readonly string settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\CodeHelperSettings.set";

        public static String Get(String name, String defVal)
        {
            ReloadSettings();
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet != null)
            {
                var x = 1;
                return findSet.Value;
            }
            settingsList.Add(new SettingsItem(name, defVal));
            SaveSettings();
            return defVal;
        }

        public static Boolean Get(String name, Boolean defVal)
        {
            ReloadSettings();
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet != null)
            {
                try
                {
                    return Boolean.Parse(findSet.Value);
                }
                catch
                {
                    return defVal;
                }
            }
            settingsList.Add(new SettingsItem(name, defVal.ToString()));
            SaveSettings();
            return defVal;
        }

        public static int Get(String name, int defVal)
        {
            ReloadSettings();
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet != null)
            {
                try { return int.Parse(findSet.Value); }
                catch { return defVal; }
            }
            settingsList.Add(new SettingsItem(name, defVal.ToString()));
            SaveSettings();
            return defVal;
        }

        public static void Set(String name, String val)
        {
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet == null)
            {
                settingsList.Add(new SettingsItem(name, val));
                SaveSettings();
            }
            else
            {
                findSet.Value = val;
                SaveSettings();
            }
        }

        public static void Set(String name, Boolean val)
        {
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet == null)
            {
                settingsList.Add(new SettingsItem(name, val.ToString()));
                SaveSettings();
            }
            else
            {
                findSet.Value = val.ToString();
                SaveSettings();
            }
        }

        public static void Set(String name, int val)
        {
            SettingsItem findSet = settingsList.Find(x => x.Name == name);
            if (findSet == null)
            {
                settingsList.Add(new SettingsItem(name, val.ToString()));
                SaveSettings();
            }
            else
            {
                findSet.Value = val.ToString();
                SaveSettings();
            }
        }

        public static void SaveSettings()
        {
            string[] settings = new string[settingsList.Count];
            int idx = 0;
            foreach (SettingsItem setting in settingsList)
            {
                settings[idx] = setting.Name + "=" + setting.Value;
                idx++;
            }
            File.WriteAllLines(settingsPath, settings);
        }

        public static void ReloadSettings()
        {
            if (File.Exists(settingsPath))
            {
                settingsList.Clear();
                string[] settings = File.ReadAllLines(settingsPath);
                foreach (string line in settings)
                {
                    string[] lineArray = line.Split('=');
                    settingsList.Add(new SettingsItem(lineArray[0], lineArray[1]));
                }
            }
        }

        public class SettingsItem
        {
            public String Name { get; set; }

            public String Value { get; set; }

            public SettingsItem()
            {
            }

            public SettingsItem(string name, string value)
            {
                Value = value;
                Name = name;
            }
        }
    }
}