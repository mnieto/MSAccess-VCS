using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    [Serializable]
    class DataLostException: Exception {
        public DataLostException(string tableName) : base(string.Format(Properties.Resources.DataLostExceptionMessage, tableName)) { }
    }
}
