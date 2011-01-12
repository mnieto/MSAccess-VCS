using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Field wrapper for <see cref="dao.Field"/> objects
    /// </summary>
    public class Field : AuxiliarObject {

        private dao.Field daoField;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="daoField"><see cref="dao.Field"/> object</param>
        public Field(object daoField) {
            DaoObject = daoField;
        }

        public override object DaoObject {
            get {
                return this.daoField;
            }
            set {
                if (value != null && !(value is dao.Field))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(dao.Field).Name));
                this.daoField = (dao.Field)value;
            }
        }

        public override string ClassName {
            get { return "Field"; }
        }

        public override object this[string propertyName] {
            get {
                try {
                    return this.daoField.Properties[propertyName].Value;
                } catch (Exception) {
                    return null;
                }
            }
        }

        public override void SaveProperties(ExportObject export) {
            try { export.WriteProperty("Attributes", daoField.Attributes); } catch { export.WriteProperty("Attributes"); }
            try { export.WriteProperty("CollatingOrder", daoField.CollatingOrder); } catch { export.WriteProperty("CollatingOrder"); }
            try { export.WriteProperty("Type", daoField.Type); } catch { export.WriteProperty("Type"); }
            try { export.WriteProperty("Name", daoField.Name); } catch { export.WriteProperty("Name"); }
            try { export.WriteProperty("OrdinalPosition", daoField.OrdinalPosition); } catch { export.WriteProperty("OrdinalPosition"); }
            try { export.WriteProperty("Size", daoField.Size); } catch { export.WriteProperty("Size"); }
            try { export.WriteProperty("SourceField", daoField.SourceField); } catch { export.WriteProperty("SourceField"); }
            try { export.WriteProperty("SourceTable", daoField.SourceTable); } catch { export.WriteProperty("SourceTable"); }
            try { export.WriteProperty("DataUpdatable", daoField.DataUpdatable); } catch { export.WriteProperty("DataUpdatable"); }
            try { export.WriteProperty("DefaultValue", daoField.DefaultValue); } catch { export.WriteProperty("DefaultValue"); }
            try { export.WriteProperty("ValidationRule", daoField.ValidationRule); } catch { export.WriteProperty("ValidationRule"); }
            try { export.WriteProperty("ValidationText", daoField.ValidationText); } catch { export.WriteProperty("ValidationText"); }
            try { export.WriteProperty("Required", daoField.Required); } catch { export.WriteProperty("Required"); }
            try { export.WriteProperty("AllowZeroLength", daoField.AllowZeroLength); } catch { export.WriteProperty("AllowZeroLength"); }
            try { export.WriteProperty("VisibleValue", daoField.VisibleValue); } catch { export.WriteProperty("VisibleValue"); }
            try { export.WriteProperty("Description", daoField.Properties["Description"].Value); } catch { export.WriteProperty("Description"); }
            try { export.WriteProperty("DecimalPlaces", daoField.Properties["DecimalPlaces"].Value); } catch { export.WriteProperty("DecimalPlaces"); }
            try { export.WriteProperty("DisplayControl", daoField.Properties["DisplayControl"].Value); } catch { export.WriteProperty("DisplayControl"); }
        }
    }
}
