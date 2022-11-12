using System.Collections.Generic;

namespace AccessScrCtrl.Profiles {
    public class ExportOptions {
        public string ExtensionNaming { get; set; }
        public List<string> ExportTableData { get; set; }
        public string ExportTableFormat { get; set; }
        public string ExportQueryDefAs { get; set; }
        public List<string> Excludes { get; set; }
    }
}
