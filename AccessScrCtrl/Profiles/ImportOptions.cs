using System.Collections.Generic;

namespace AccessScrCtrl.Profiles {
    public class ImportOptions {
        public bool OverwriteDatabase { get; set; }
        public bool ConfirmImportedObjects { get; set; }
        public bool RemoveNotLoaded { get; set; }
        public List<string> AllowDataLostTables { get; set; }
        public List<string> Excludes { get; set; }
    }
}
