using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AccessIO {

    /// <summary>
    /// Backup helper for saving a copy before a Load operation
    /// </summary>
    public class BackupHelper {

        private string backupFileName;
        private string fileName;
        
        /// <summary>
        /// Name of the file to do the backup
        /// </summary>
        /// <exception cref="ArgumentNullException">If <see cref="FileName"/> is a null string</exception>
        /// <exception cref="ArgumentException">If FileName do not exists</exception>
        public string FileName {
            get { return fileName; }
            private set {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();
                if (!File.Exists(value))
                    throw new ArgumentException();
                fileName = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Name of the file to do the backup</param>
        public BackupHelper(string fileName) {
            FileName = fileName;
        }

        /// <summary>
        /// Do the copy operation over a temporary file
        /// </summary>
        public void DoBackUp() {
            backupFileName = Path.GetTempFileName();
            File.Copy(FileName, backupFileName, true);
        }

        /// <summary>
        /// Accepts the work and deletes the temporary file
        /// </summary>
        public void Commit() {
            try {
                File.Delete(backupFileName);
            } catch { 
                //We don't care if delete fails
            }
        }

        /// <summary>
        /// Restores the backup file
        /// </summary>
        public void RollBack() {
            File.Copy(backupFileName, FileName, true);
        }

    }
}
