namespace AccessScrCtrl.Profiles {
    class Profile {
        public string Name { get; set; }
        public int Version { get; set; }
        public string AccessFile { get; set; }
        public string WorkingCopy { get; set; }
        public string LastSaveDateTime { get; set; }
        public string LogLevel { get; set; }
        public string LogFileName { get; set; }
        public ImportOptions ImportOptions { get; set; }
        public ExportOptions ExportOptions { get; set; }
    }
}
