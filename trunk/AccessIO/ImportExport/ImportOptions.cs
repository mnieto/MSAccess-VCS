using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// Import settings that controls the behavior of the import process
    /// </summary>
    public class ImportOptions: ICloneable {

        /// <summary>
        /// If <c>true</c> import will delete existing database and recreate it. If <c>fasle</c> import will overwrite individual objects. Default: false
        /// </summary>
        public bool OverwriteDatabase { get; set; }

        /// <summary>
        /// If <c>true </c> import will prompt for overwrited objects, else objects will overwrited quietly. Default false.
        /// </summary>
        public bool Prompt { get; set; }

        /// <summary>
        /// Delete not imported objects: This (should) is equivalent to <see cref="OverwriteDatabase"/>. Default false.
        /// </summary>
        public bool DeleteNotLoaded { get; set; }

        /// <summary>
        /// List of table names that will be deleted prior to recreate the table, even if they content data. Default: empty list
        /// </summary>
        /// <remarks>
        /// <para>This property is ignored when importing to adp file</para>
        /// <para>
        /// <list type="bullet">
        /// </list>
        /// <item><description>If a table is present in this list and contains data, data will be deleted quietly.</description></item>
        /// <item><description>If a table is present in this list and do not contains data, this property do not affect the normal import behavior.</description></item>
        /// <item><description>If a table is not present in this list and contains data, a <see cref="DataLostException"/> exception will be throwed.</description></item>
        /// </para>
        /// </remarks>
        public List<string> AllowDataLost { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImportOptions() {
            AllowDataLost = new List<string>();
        }

        #region ICloneable Members

        public object Clone() {
            ImportOptions tmp = new ImportOptions();
            tmp.AllowDataLost.AddRange(this.AllowDataLost);
            tmp.DeleteNotLoaded = this.DeleteNotLoaded;
            tmp.OverwriteDatabase = this.OverwriteDatabase;
            tmp.Prompt = this.Prompt;
            return tmp;
        }

        #endregion
    }
}
