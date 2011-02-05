using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {

    [Serializable]
    public class AccessIOException : Exception {
        public AccessIOException(string message) : base(message) { }
    }

    /// <summary>
    /// DataLostException is thrown if you want to import a table definition to a table that contais data.
    /// If you want to avoid it, you should set the <see cref="Table.AllowDataLost"/> property to <c>true</c>
    /// </summary>
    [Serializable]
    class DataLostException : AccessIOException {
        public DataLostException(string tableName) : base(string.Format(Properties.Resources.DataLostExceptionMessage, tableName)) { }
    }

    /// <summary>
    /// WrongFileFormatException is thrown when an unexpected format is encountered reading a text file during import to Access
    /// </summary>
    [Serializable]
    public class WrongFileFormatException : AccessIOException {

        public WrongFileFormatException(string message) : base(message) { }
        public WrongFileFormatException(string message, string fileName) : base(message) {
            FileName = fileName;
        }
        public WrongFileFormatException(string message, string fileName, int lineNumber)
            : base(message) {
            FileName = fileName;
            LineNumber = lineNumber;
        }

        public string FileName { get; set; }
        public int LineNumber { get; set; }
    }
}
