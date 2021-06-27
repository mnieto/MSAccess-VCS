using System.Runtime.Serialization;

namespace AccessScrCtrl.Profiles {
    public class Profile {
        public string Name { get; set; }
        public int Version { get; set; } = 1;
        public string AccessFile { get; set; }
        public string WorkingCopy { get; set; }
        public string LastSaveDateTime { get; set; }
        public string LogLevel { get; set; }
        public string LogFileName { get; set; }
        public ImportOptions ImportOptions { get; set; }
        public ExportOptions ExportOptions { get; set; }

        /// <summary>
        /// Holds the profile file name. Used in memory for convenience, but not serialized in the configuration file
        /// </summary>
        [IgnoreDataMember]
        internal string FileName { get; set; }

        [IgnoreDataMember]
        public string AccessFileFullPath => Helpers.PathUtil.GetFullPath(AccessFile, FileName);
        [IgnoreDataMember]
        public string WorkingCopyFullPath => Helpers.PathUtil.GetFullPath(WorkingCopy, FileName);

    }
}
