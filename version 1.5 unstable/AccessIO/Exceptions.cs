using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace AccessIO {

    [Serializable]
    public class AccessIOException : Exception {
        public AccessIOException(string message) : base(message) { }
        public AccessIOException(string message, Exception innerException) : base(message, innerException) { }
        protected AccessIOException(SerializationInfo info, StreamingContext context): base(info, context) { }
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
        protected WrongFileFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {
            if (info != null) {
                FileName = info.GetString("FileName");
                LineNumber = info.GetInt32("LineNumber");
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) {
            base.GetObjectData(info, context);
            if (info != null) {
                info.AddValue("FileName", FileName);
                info.AddValue("LineNumber", LineNumber);
            }
        }

        public string FileName { get; set; }
        public int LineNumber { get; set; }
    }
}
