using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dao = Microsoft.Office.Interop.Access.Dao;

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

                PropertyCollectionDao propColl = new PropertyCollectionDao(tbDef, tbDef.Properties);
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
                //TODO: Add new option menu to ignore linked tables errors if the linked document do not exist
                //      Check if the linked document exists before iterate the indexes collection
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

            using (StreamReader sr = new StreamReader(fileName)) {
                ImportObject import = new ImportObject(sr);

                string objName = import.ReadObjectName();
                if (string.Compare(this.Name, objName, true) != 0)
                    this.Name = objName;

                dao.Database db = App.Application.CurrentDb();
                bool tableExists = false;
                string tempTableName = this.Name;
                //if (ExistsTableDef(db, this.Name)) {
                //    foreach (dao.Relation relation in GetTableRelations(db)) {
                //        db.Relations.Delete(relation.Name);
                //    }
                //    db.TableDefs.Delete(this.Name);
                //}
                if (ExistsTableDef(db, this.Name)) {
                    tableExists = true;
                    tempTableName = String.Format("{0}{1}", this.Name, DateTime.Now.ToString("yyyyMMddHHmmssff"));
                    tempTableName = tempTableName.Substring(tempTableName.Length - Math.Min(64, tempTableName.Length));
                }
                tableDef = db.CreateTableDef(tempTableName);

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
                        if (import.IsBegin) {
                            import.ReadLine();  //Read the 'Begin Index' line.
                            while (!import.IsEnd) {
                                dao.Index idx = ReadIndex(import);
                                if (idx == null)
                                    break;
                                tableDef.Indexes.Append(idx);
                            }
                        }
                    }
                    db.TableDefs.Append(tableDef);
                    db.TableDefs.Refresh();

                    //According with MS-doc: The object to which you are adding the user-defined property must already be appended to a collection.
                    //see: http://msdn.microsoft.com/en-us/library/ff820932.aspx
                    //So: After fields added to the tableDef and tableDef added to the database, we add the custom properties
                    //This properties are also available for linked tables
                    foreach (Field field in Fields)
                        field.AddCustomProperties();
                    AddCustomProperties(props);

                    //manage table relations
                    if (tableExists) {
                        List<dao.Relation> relationsList = new List<dao.Relation>();
                        foreach (dao.Relation relation in GetTableRelations(db)) {
                            dao.Relation newRelation = db.CreateRelation(relation.Name, relation.Table, relation.ForeignTable, relation.Attributes);
                            //try { newRelation.PartialReplica = relation.PartialReplica; } catch { }     //Accessing this property causes an exception ¿?
                            foreach (dao.Field field in relation.Fields) {
                                dao.Field newField = newRelation.CreateField();
                                newField.Name = field.Name;
                                newField.ForeignName = field.ForeignName;
                                newRelation.Fields.Append(newField);
                            }
                            relationsList.Add(newRelation);
                            db.Relations.Delete(relation.Name);
                        }
                        db.Relations.Refresh();

                        db.TableDefs.Delete(this.Name);
                        db.TableDefs[tempTableName].Name = this.Name;
                        db.TableDefs.Refresh();

                        foreach (dao.Relation relation in relationsList) {
                            try {
                                db.Relations.Append(relation);
                            } catch { 
                                //not allways we can restore the relation: the field do not exists or has changed the data type
                            }
                        }
                    }

                } catch (Exception ex) {
                    if (tableExists)
                        db.TableDefs.Delete(tempTableName);
                    string message = String.Format(AccessIO.Properties.ImportRes.ErrorAtLineNum, import.LineNumber, ex.Message);
                    throw new WrongFileFormatException(message, fileName, import.LineNumber);
                }
            }
        }

        private void AddCustomProperties(Dictionary<string, object> props) {
            PropertyCollectionDao propColl = new PropertyCollectionDao(tableDef, tableDef.Properties);
            propColl.AddOptionalProperty(props, "Description", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "ConflictTable", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "ReplicaFilter", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "Orientation", dao.DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "OrderByOn", dao.DataTypeEnum.dbBoolean);
            propColl.AddOptionalProperty(props, "SubdatasheetName", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "LinkChildFields", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "LinkMasterFields", dao.DataTypeEnum.dbText);
            propColl.AddOptionalProperty(props, "SubdatasheetHeight", dao.DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "SubdatasheetExpanded", dao.DataTypeEnum.dbBoolean);
            propColl.AddOptionalProperty(props, "DefaultView", dao.DataTypeEnum.dbInteger);
            propColl.AddOptionalProperty(props, "OrderBy", dao.DataTypeEnum.dbText);
        }

        private dao.Field ReadField(ImportObject import) {
            Field field = new Field(tableDef.CreateField());
            field.LoadProperties(tableDef, import);
            Fields.Add(field);
            return (dao.Field)field.DaoObject;
        }

        private dao.Index ReadIndex(ImportObject import) {
            
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
            import.ReadLine(2);       //Read the 'End Index' line and the 'Begin Index' or 'End Indexes'
            return idx;
        }

        private bool ExistsTableDef(dao.Database db, string tableName) {
            foreach (dao.TableDef tbDef in db.TableDefs) {
                if (string.Compare(tbDef.Name, tableName, true) == 0)
                    return true;
            }
            return false;
        }

        private List<dao.Relation> GetTableRelations(dao.Database db) {
            List<dao.Relation> result = new List<dao.Relation>();
            foreach (dao.Relation relation in db.Relations) {
                if (relation.Table == this.Name || relation.ForeignTable == this.Name)
                    result.Add(relation);
            }
            return result;
        }
    }
}
