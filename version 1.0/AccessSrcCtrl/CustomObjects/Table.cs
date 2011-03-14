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

        /// <summary>
        /// If <c>true</c> and table has data, <see cref="Load"/> method will overwrite the table structure and data will lost
        /// If <c>false</c> and table has data, <see cref="Load"/> method will raise an exception
        /// </summary>
        public bool AllowDataLost { get; set; }

        /// <summary>
        /// Return <c>true</c> if table has data
        /// </summary>
        public bool HasData {
            get {
                dao.Database db = App.Application.CurrentDb();
                if (db == null)
                    return false;
                else
                    return db.TableDefs[this.Name].RecordCount != 0;
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
            tableDef = app.Application.CurrentDb().TableDefs[name];
            AllowDataLost = (app as AccessMdb).AllowDataLost;
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
            //dao.TableDef tbDef = (dao.TableDef)this.DaoObject;

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
            throw new NotImplementedException();
        }

        public override void SaveProperties(ExportObject export) {
            try { export.WriteProperty("Attributes", tableDef.Attributes); } catch { export.WriteProperty("Attributes"); }
            try { export.WriteProperty("Connect", tableDef.Connect); } catch { export.WriteProperty("Connect"); }
            try { export.WriteProperty("ReplicaFilter", tableDef.ReplicaFilter); } catch { export.WriteProperty("ReplicaFilter"); }
            try { export.WriteProperty("SourceTableName", tableDef.SourceTableName); } catch { export.WriteProperty("SourceTableName"); }
            try { export.WriteProperty("ValidationRule", tableDef.ValidationRule); } catch { export.WriteProperty("ValidationRule"); }
            try { export.WriteProperty("ValidationText", tableDef.ValidationText); } catch { export.WriteProperty("ValidationText"); }
            foreach (dao.Field fld in tableDef.Fields) {
                ICustomObject fldObject = new Field(fld);
                export.WriteObject(fldObject, fld.Name);
            }
            foreach (dao.Index idx in tableDef.Indexes) {
                ICustomObject idxObject = AuxiliarObject.CreateInstance(App, idx, AccessIO.ObjectType.Index);
                export.WriteObject(idxObject, idx.Name);
            }
        }
    }
}
