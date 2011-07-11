using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dao;

namespace AccessIO {
    /// <summary>
    /// <see cref="dao.Table"/> wrapper
    /// </summary>
    public class Table : CustomObject {

        private dao.TableDef tableDef;

        private bool AllowDataLost { 
            get { return (Options as OptionsTable).AllowDataLost; }
            set { (Options as OptionsTable).AllowDataLost = value; }
        }

        /// <summary>
        /// Return <c>true</c> if table has data
        /// </summary>
        public bool HasData {
            get {
                dao.Database db = App.Application.CurrentDb();
                if (db == null)
                    return false;
                else {
                    if (ExistsTableDef(db, this.Name))
                        return db.TableDefs[this.Name].RecordCount != 0;
                    else
                        return false;
                }
            }
        }

        /// <summary>
        /// Name of the underlying table
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Name of the class name
        /// </summary>
        public override string ClassName {
            get { return "Table"; }
        }

        public override object DaoObject {
            get {
                return this.tableDef;
            }
            set {
                if (value != null && !(value is dao.TableDef))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(dao.TableDef).Name));
                this.tableDef = (dao.TableDef)value;
            }
        }

        /// <summary>
        /// Indexer to the object's properties
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        /// <returns>property value or String.Empty if property is not set</returns>
        /// <exception cref="">If propertyName do not exists in the object's instance</exception>
        /// <exception cref="">If propertyName is not allowed for this object class</exception>
        public override object this[string propertyName] {
            get {
                try {
                    return tableDef.Properties[propertyName].Value;
                } catch (Exception) {
                    return null;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">AccessScrCtrl application</param>
        /// <param name="name">Name of the object name</param>
        /// <param name="objectType">Access object type</param>
        public Table(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) {
            //TODO: Ver posibilidad de no instanciar tableDef hasta el método Load (habría que cambiar el método Save)
            dao.Database db = app.Application.CurrentDb();
            if (ExistsTableDef(db, name.ToString()))
                tableDef = app.Application.CurrentDb().TableDefs[name];
            else
                tableDef = null;
            TableName = name;
        }

        /// <summary>
        /// Save table defintion to <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">File name where save the table definition</param>
        public override void Save(string fileName) {
            
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            dao.DBEngine dbEngine = new dao.DBEngine();
            dao.Database db = dbEngine.OpenDatabase(App.FileName);
            dao.TableDef tbDef = db.TableDefs[Name];

            using (StreamWriter sw = new StreamWriter(fileName)) {
                ExportObject export = new ExportObject(sw);
                export.WriteBegin(ClassName, TableName);
                export.WriteProperty("Attributes", tbDef.Attributes);
                export.WriteProperty("Connect", tbDef.Connect);
                export.WriteProperty("SourceTableName", tbDef.SourceTableName);
                export.WriteProperty("ValidationRule", tbDef.ValidationRule);
                export.WriteProperty("ValidationText", tbDef.ValidationText);
                export.WriteBegin("Fields");
                foreach (dao.Field field in tbDef.Fields) {
                    export.WriteObject(new Field(field));
	            }
                export.WriteEnd();      //End Fields
                export.WriteBegin("Indexes");
                foreach (dao.Index daoIndex in tbDef.Indexes) {
                    export.WriteObject(new Index(daoIndex));
                }
                export.WriteEnd();  //End Indexes
                export.WriteEnd();  //End Table
            }
            db.Close();

        }

        public override void Load(string fileName) {
            if (!AllowDataLost && HasData)
                throw new DataLostException(this.Name);
            
            StreamReader sr = new StreamReader(fileName);
            ImportObject import = new ImportObject(sr);

            string objName = import.ReadObjectName();
            if (string.Compare(this.Name, objName, true) != 0)
                this.Name = objName;

            //TODO: if exists table: do not delete, create a copy: if all is right delete the copy; if not restore the copy
            dao.Database db = App.Application.CurrentDb();
            if (ExistsTableDef(db, this.Name))
                db.TableDefs.Delete(this.Name);
            tableDef = db.CreateTableDef(this.Name);

            //read table properties
            Dictionary<string, object> props = import.ReadProperties();
            
            //tableDef.Attributes = Convert.ToInt32(props["Attributes"]);
            tableDef.Connect = Convert.ToString(props["Connect"]);
            if (props.ContainsKey("ReplicaFilter"))
                tableDef.ReplicaFilter = Convert.ToString(props["ReplicaFilter"]);
            tableDef.SourceTableName = Convert.ToString(props["SourceTableName"]);
            tableDef.ValidationRule = Convert.ToString(props["ValidationRule"]);
            tableDef.ValidationText = Convert.ToString(props["ValidationText"]);

            //Attached tables do not allow fields nor indexes definitions
            if (String.IsNullOrEmpty(Convert.ToString(props["Connect"]))) {

                //read fields
                import.ReadLine(); //Read the 'Begin Fields' line
                while (!import.IsEnd) {
                    dao.Field fld = ReadField(import);
                    tableDef.Fields.Append(fld);
                }

                //read indexes
                import.ReadLine();  //Read the 'Begin Indexes' line. If there is not indexes, CurrentLine == End
                while (!import.IsEnd) {
                    dao.Index idx = ReadIndex(import);
                    if (idx == null)
                        break;
                    tableDef.Indexes.Append(idx);
                }

            }

            App.Application.CurrentDb().TableDefs.Append(tableDef);
            
        }

        private dao.Field ReadField(ImportObject import) {
            Dictionary<string, object> props = import.ReadProperties();
            import.ReadLine(); //Reads the 'End Field' line

            dao.Field fld = tableDef.CreateField();
            fld.Attributes = Convert.ToInt32(props["Attributes"]);
            
            //CollatingOrder is read only!!
            
            fld.Type = Convert.ToInt16(props["Type"]);
            fld.Name = Convert.ToString(props["Name"]);
            fld.OrdinalPosition = Convert.ToInt16(props["OrdinalPosition"]);
            fld.Size = Convert.ToInt32(props["Size"]);
            
            //SourceField, SourceTable, DataUpdatable are read only!!
            
            fld.DefaultValue = Convert.ToString(props["DefaultValue"]);
            fld.ValidationRule = Convert.ToString(props["ValidationRule"]);
            fld.ValidationText = Convert.ToString(props["ValidationText"]);
            fld.Required = Convert.ToBoolean(props["Required"]);

            //Setting directly the property causes an Not valid oparation exception
            if (fld.Type == (short)dao.DataTypeEnum.dbText)
                fld.AllowZeroLength =Convert.ToBoolean(props["AllowZeroLength"]); 
            
            //VisibleValue is read only!!

            //Check if exists optional properties
            if (props["Description"] != null)
                fld.CreateProperty("Description", dao.DataTypeEnum.dbText, Convert.ToString(props["Description"]));
            if (props["DecimalPlaces"] != null)
                fld.CreateProperty("DecimalPlaces", dao.DataTypeEnum.dbInteger, Convert.ToString(props["DecimalPlaces"]));
            if (props["DisplayControl"] != null)
                fld.CreateProperty("DisplayControl", dao.DataTypeEnum.dbInteger, Convert.ToString(props["DisplayControl"]));

            return fld;

        }

        private dao.Index ReadIndex(ImportObject import) {
            
            import.ReadLine();  //Read the 'Begin Index' line (or End Indexes if there aren't indexes)
            if (import.IsEnd)
                return null;

            dao.Index idx = tableDef.CreateIndex();
            Dictionary<string, object> props = import.ReadProperties();
            idx.Name = Convert.ToString(props["Name"]);
            idx.Primary = Convert.ToBoolean(props["Primary"]);
            idx.Unique = Convert.ToBoolean(props["Unique"]);
            idx.IgnoreNulls = Convert.ToBoolean(props["IgnoreNulls"]);
            idx.Required = Convert.ToBoolean(props["Required"]);

            import.ReadLine(); //Read the 'Begin Fields' line
            while (!import.IsEnd) {
                dao.Field fld = idx.CreateField();
                import.ReadLine();
                fld.Name = import.PropertyValue();
                import.ReadLine();
                fld.Attributes = Convert.ToInt32(import.PropertyValue());
                ((dao.IndexFields)idx.Fields).Append(fld);
                import.ReadLine(2);  //Skip the 'End Field' line and read the next 'Begin Field' line (or 'End Fields' if there aren't more fields)
            }
            return idx;
        }

        private bool ExistsTableDef(dao.Database db, string tableName) {
            foreach (dao.TableDef tbDef in db.TableDefs) {
                if (string.Compare(tbDef.Name, tableName, true) == 0)
                    return true;
            }
            return false;
        }
    }
}
