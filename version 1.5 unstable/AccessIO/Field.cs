using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Field wrapper for <see cref="dao.Field"/> objects
    /// </summary>
    public class Field : AuxiliarObject {

        private dao.Field daoField;
        private Dictionary<string, object> props;

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
            try { 
                export.WriteProperty("DisplayControl", daoField.Properties["DisplayControl"].Value); 
                switch (Convert.ToInt32(daoField.Properties["DisplayControl"].Value)) {
                    case 110:   //listbox
                        try { export.WriteProperty("RowSourceType", daoField.Properties["RowSourceType"].Value); } catch { export.WriteProperty("RowSourceType"); }
                        try { export.WriteProperty("RowSource", daoField.Properties["RowSource"].Value); } catch { export.WriteProperty("RowSource"); }
                        try { export.WriteProperty("BoundColumn", daoField.Properties["BoundColumn"].Value); } catch { export.WriteProperty("BoundColumn"); }
                        try { export.WriteProperty("ColumnCount", daoField.Properties["ColumnCount"].Value); } catch { export.WriteProperty("ColumnCount"); }
                        try { export.WriteProperty("ColumnHeads", daoField.Properties["ColumnHeads"].Value); } catch { export.WriteProperty("ColumnHeads"); }
                        try { export.WriteProperty("ColumnWidths", daoField.Properties["ColumnWidths"].Value); } catch { export.WriteProperty("ColumnWidths"); }
                        break;
                    case 111:   //dropdown list
                        try { export.WriteProperty("RowSourceType", daoField.Properties["RowSourceType"].Value); } catch { export.WriteProperty("RowSourceType"); }
                        try { export.WriteProperty("RowSource", daoField.Properties["RowSource"].Value); } catch { export.WriteProperty("RowSource"); }
                        try { export.WriteProperty("BoundColumn", daoField.Properties["BoundColumn"].Value); } catch { export.WriteProperty("BoundColumn"); }
                        try { export.WriteProperty("ColumnCount", daoField.Properties["ColumnCount"].Value); } catch { export.WriteProperty("ColumnCount"); }
                        try { export.WriteProperty("ColumnHeads", daoField.Properties["ColumnHeads"].Value); } catch { export.WriteProperty("ColumnHeads"); }
                        try { export.WriteProperty("ColumnWidths", daoField.Properties["ColumnWidths"].Value); } catch { export.WriteProperty("ColumnWidths"); }
                        try { export.WriteProperty("ListRows", daoField.Properties["ListRows"].Value); } catch { export.WriteProperty("ListRows"); }
                        try { export.WriteProperty("ListWidth", daoField.Properties["ListWidth"].Value); } catch { export.WriteProperty("ListWidth"); }
                        try { export.WriteProperty("LimitToList", daoField.Properties["LimitToList"].Value); } catch { export.WriteProperty("LimitToList"); }
                        break;
                }
            } catch
            { export.WriteProperty("DisplayControl"); }
        }

        public void LoadProperties(dao.TableDef tableDef, ImportObject import) {
            props = import.ReadProperties();
            import.ReadLine(); //Reads the 'End Field' line

            daoField.Attributes = Convert.ToInt32(props["Attributes"]);

            //CollatingOrder is read only!!

            daoField.Type = Convert.ToInt16(props["Type"]);
            daoField.Name = Convert.ToString(props["Name"]);
            daoField.OrdinalPosition = Convert.ToInt16(props["OrdinalPosition"]);
            daoField.Size = Convert.ToInt32(props["Size"]);

            //SourceField, SourceTable, DataUpdatable are read only!!

            daoField.DefaultValue = Convert.ToString(props["DefaultValue"]);
            daoField.ValidationRule = Convert.ToString(props["ValidationRule"]);
            daoField.ValidationText = Convert.ToString(props["ValidationText"]);
            daoField.Required = Convert.ToBoolean(props["Required"]);

            //AllowZeroLength property is valid only for text fields
            if (daoField.Type == (short)dao.DataTypeEnum.dbText)
                daoField.AllowZeroLength = Convert.ToBoolean(props["AllowZeroLength"]);

            //VisibleValue is read only!!

        }

        public void AddCustomProperties() {
            AddOptionalProperty(props, "Description", dao.DataTypeEnum.dbText);
            AddOptionalProperty(props, "DecimalPlaces", dao.DataTypeEnum.dbInteger);
            AddOptionalProperty(props, "DisplayControl", dao.DataTypeEnum.dbInteger);
            AddOptionalProperty(props, "RowSourceType", dao.DataTypeEnum.dbText);
            AddOptionalProperty(props, "RowSource", dao.DataTypeEnum.dbMemo);
            AddOptionalProperty(props, "BoundColumn", dao.DataTypeEnum.dbInteger);
            AddOptionalProperty(props, "ColumnCount", dao.DataTypeEnum.dbInteger);
            AddOptionalProperty(props, "ColumnHeads", dao.DataTypeEnum.dbBoolean);
            AddOptionalProperty(props, "ColumnWidths", dao.DataTypeEnum.dbText);
            AddOptionalProperty(props, "ListRows", dao.DataTypeEnum.dbInteger);
            AddOptionalProperty(props, "ListWidth", dao.DataTypeEnum.dbText);
            AddOptionalProperty(props, "LimitToList", dao.DataTypeEnum.dbBoolean);
        }

        private void AddOptionalProperty(Dictionary<string, object> props, string propertyName, dao.DataTypeEnum dataType) {
            if (props.ContainsKey(propertyName) && props[propertyName] != null) {
                try {
                    daoField.Properties[propertyName].Value = props[propertyName];
                } catch (System.Runtime.InteropServices.COMException ex) {
                    if (ex.ErrorCode == -2146825018)    //Property don't exists in the properties collection
                        daoField.Properties.Append(daoField.CreateProperty(propertyName, dataType, props[propertyName]));
                    else
                        throw;
                }
            }
        }
    }
}
