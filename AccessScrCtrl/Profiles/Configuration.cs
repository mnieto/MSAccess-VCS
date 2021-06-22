using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccessScrCtrl.Profiles {
    class Configuration {
        private const string SettingsFileName = "AccessScrCtl.json";

        public static Configuration LoadConfiguration() {
            try {
                var cfg = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(SettingsFileName));
                return cfg;
            } catch  {
                return new Configuration();
            }       
        }

        public void SaveConfiguration() {
            File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public IEnumerable<ProfileName> LastProfiles() {
            return Profiles
                .OrderBy(x => x.LastUsedOrder)
                .Take(MRUNumber);
        }
        
        /// <summary>
        /// Adds or updates the profile in the MRU list
        /// </summary>
        public void AddProfile(Profile profile, string profilePath) {
            var profileName = Profiles
                .FirstOrDefault(x => string.Compare(x.FileName, profilePath, true) == 0);
            if (profileName != null) {
                profileName.LastUsedOrder = 1;
            } else {
                profileName = new ProfileName {
                    Name = profile.Name,
                    FileName = profilePath,
                    LastUsedOrder = 1
                };
                Profiles.Add(profileName);
            }
            int i = 2;
            foreach (var item in Profiles.Where(x => x != profileName).OrderBy(x => x.LastUsedOrder)) {
                item.LastUsedOrder = i++;
            }
            SaveConfiguration();
        }

        public Profile LoadProfile(string profilePath) {
            var profile = JsonConvert.DeserializeObject<Profile>(File.ReadAllText(profilePath));
            return profile;
        }

        public void RemoveProfile(string profilePath) {
            var p = Profiles.First(x => string.Equals(x.FileName, profilePath, StringComparison.InvariantCultureIgnoreCase));
            Profiles.Remove(p);
            SaveConfiguration();
        }

        public void SaveProfile(Profile profile, string profilePath) {
            File.WriteAllText(profilePath, JsonConvert.SerializeObject(profile, Formatting.Indented));
        }

        public List<ProfileName> Profiles { get; set; } = new List<ProfileName>();

        /// <summary>
        /// number of must recent used profiles to be shown in the MRU menu
        /// </summary>
        public int MRUNumber { get; set; } = 4;

    }
}
