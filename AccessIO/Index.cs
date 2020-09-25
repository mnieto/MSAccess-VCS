using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Index wrapper for <see cref="dao.Index"/> objects
    /// </summary>
    public class Index: AuxiliarObject {

        private dao.Index daoIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="daoIndex"><see cref="dao.Index"/> object</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214", Justification = "base constructor is called explicitly and there is not interaction with another member variables")]
        public Index(object daoIndex) {
            DaoObject = daoIndex;
        }

        public override object DaoObject {
            get {
                return this.daoIndex;
            }
            set {
                if (value != null && !(value is dao.Index))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(dao.Index).Name));
                this.daoIndex = (dao.Index)value;
            }
        }


        public override string ClassName {
            get { return "Index"; }
        }

        public override object this[string propertyName] {
            get {
                try {
                    return this.daoIndex.Properties[propertyName].Value;
                } catch {
                    return null;
                }
            }
        }

        public override void SaveProperties(ExportObject export) {
            export.WriteProperty("Name", daoIndex.Name);
            export.WriteProperty("Primary", daoIndex.Primary);
            export.WriteProperty("Unique", daoIndex.Unique);
            export.WriteProperty("IgnoreNulls", daoIndex.IgnoreNulls);
            export.WriteProperty("Required", daoIndex.Required);
            export.WriteBegin("Fields");
            dao.IndexFields idxFiels = (dao.IndexFields)daoIndex.Fields;
            for (int i = 0; i < idxFiels.Count; i++) {
                dao.Field fld = (dao.Field)idxFiels[i];
                export.WriteBegin("Field");
                export.WriteProperty("Name", fld.Name);
                export.WriteProperty("Attributes", fld.Attributes); //Descending order (dao.FieldAttributeEnum.dbDescending)
                export.WriteEnd();
            }
            export.WriteEnd();
        }
    }
}
