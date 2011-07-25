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
        private List<Field> Fields;

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
            Fields = new List<Field>();
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
                //export.ListProperties(tbDef.Name, tbDef.Properties);
                export.WriteBegin(ClassName, TableName);
                export.WriteProperty("Attributes", tbDef.Attributes);
                export.WriteProperty("Connect", tbDef.Connect);
                export.WriteProperty("SourceTableName", tbDef.SourceTableName);
                export.WriteProperty("ValidationRule", tbDef.ValidationRule);
                export.WriteProperty("ValidationText", tbDef.ValidationText);

                PropertyCollection propColl = new PropertyCollection(tbDef, tbDef.Properties);
                propColl.TryWriteProperty(export, "Description");
                propColl.TryWriteProperty(export, "ConflictTable");
                propColl.TryWriteProperty(export, "ReplicaFilter");
                propColl.TryWriteProperty(export, "Orientation");
                propColl.TryWriteProperty(export, "OrderByOn");
                propColl.TryWriteProperty(export, "SubdatasheetName");
                propColl.TryWriteProperty(export, "LinkChildFields");
                propColl.TryWriteProperty(export, "LinkMasterFields");
                propColl.TryWriteProperty(export, "SubdatasheetHeight");
                propColl.TryWriteProperty(export, "SubdatasheetExpanded");
                propColl.TryWriteProperty(export, "DefaultView");
                propColl.TryWriteProperty(export, "OrderBy");

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

            try {
                //read table properties
                Dictionary<string, object> props = import.ReadProperties();

                //tableDef.Attributes = Convert.ToInt32(props["Attributes"]);
                tableDef.Connect = Convert.ToString(props["Connect"]);
                tableDef.SourceTableName = Convert.ToString(props["SourceTableName"]);
                tableDef.ValidationRule = Convert.ToString(props["ValidationRule"]);
                tableDef.ValidationText = Convert.ToString(props["ValidationText"]);

                //Linked tables do not allow fields nor indexes definitions
                //but ms access properties are allowed
                bool isLinkedTable = !String.IsNullOrEmpty(Convert.ToString(props["Connect"]));

                    //read fields
                    import.ReadLine(); //Read the 'Begin Fields' line
                    while (!import.IsEnd) {
                        dao.Field fld = ReadField(import);
                        if (!isLinkedTable)
                            tableDef.Fields.Append(fld);
                    }

                if (!isLinkedTable) {
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

                //According with MS-doc: The object to which you are adding the user-defined property must already be appended to a collection.
                //see: http://msdn.microsoft.com/en-us/library/ff820932.aspx
                //So: After fields added to the tableDef and tableDef added to the database, we add the custom properties
                //This properties are also available for linked tables
                foreach (Field field in Fields)
                    field.AddCustomProperties();
                AddCustomProperties(props);

            } catch (Exception ex) {
                string message = String.Format(AccessIO.Properties.ImportRes.ErrorAtLineNum, import.LineNumber, ex.Message);
                throw new WrongFileFormatException(message, fileName, import.LineNumber);
            }           
        }

        private void AddCustomProperties(Dictionary<string, object> props) {
            PropertyCollection propColl = new PropertyCollection(tableDef, tableDef.Properties);
            propColl.AddOptionalProperty(props, "Description", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "ConflictTable", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "ReplicaFilter", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "Orientation", DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "OrderByOn", DataTypeEnum.dbBoolean);
            propColl.AddOptionalProperty(props, "SubdatasheetName", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "LinkChildFields", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "LinkMasterFields", DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "SubdatasheetHeight", DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "SubdatasheetExpanded", DataTypeEnum.dbBoolean);
            propColl.AddOptionalProperty(props, "DefaultView", DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "OrderBy", DataTypeEnum.dbText);
        }

        private dao.Field ReadField(ImportObject import) {
            Field field = new Field(tableDef.CreateField());
            field.LoadProperties(tableDef, import);
            Fields.Add(field);
            return (dao.Field)field.DaoObject;
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
