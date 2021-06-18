using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccessScrCtrl {
    class Configuration {
        private const string SettingsFileName = "AccessScrCtl.json";
        public static Configuration LoadConfiguration() {
            var cfg = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(SettingsFileName));
            return cfg;
        }

        public void SaveConfiguration() {
            File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(this));
        }

        public IEnumerable<Profile> LastProfiles() {
            return Profiles
                .OrderBy(x => x.LastUsedOrder)
                .Take(MRUNumber);
        }

        public List<Profile> Profiles { get; set; }
        public int MRUNumber { get; set; }
    }

    class Profile {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int LastUsedOrder { get; set; }
    }
}
