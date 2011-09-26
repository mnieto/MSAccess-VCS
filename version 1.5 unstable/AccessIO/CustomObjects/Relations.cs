using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AccessIO {
    /// <summary>
    /// Relations wrapper for <see cref="dao.Relations"/> object
    /// </summary>
    public class Relations : CustomObject {

        dao.Relations relations;

        public Relations(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) { }

        public override void Save(string fileName) {
            //Make sure the path exists
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            using (StreamWriter sw = new StreamWriter(fileName)) {
                ExportObject export = new ExportObject(sw);

                dao.Database db = App.Application.CurrentDb();

                export.WriteBegin(ClassName);
                foreach (dao.Relation relation in db.Relations) {
                    export.WriteBegin("Relation", relation.Name);
                    export.WriteProperty("Attributes", relation.Attributes);
                    export.WriteProperty("ForeignTable", relation.ForeignTable);
                    export.WriteProperty("Table", relation.Table);
                    export.WriteProperty("PartialReplica", relation.PartialReplica);
                    export.WriteBegin("Fields");
                    foreach (dao.Field fld in relation.Fields) {
                        export.WriteBegin("Field");
                        export.WriteProperty("Name", fld.Name);
                        export.WriteProperty("ForeignName", fld.ForeignName);
                        export.WriteEnd();
                    }
                    export.WriteEnd();
                    export.WriteEnd();
                }
                export.WriteEnd();
            }

        }

        public override void Load(string fileName) {

            //Delete first the existent relations
            dao.Relations relations = App.Application.CurrentDb().Relations;
            foreach (dao.Relation item in relations) {
                relations.Delete(item.Name);
            }
            relations.Refresh();

            using (StreamReader sr = new StreamReader(fileName)) {
                ImportObject import = new ImportObject(sr);
                import.ReadLine();
                string relationName = import.ReadObjectName();
                Dictionary<string, object> relationProperties = import.ReadProperties();

                dao.Relation relation = new dao.RelationClass();
                relation.Name = relationName;
                relation.Attributes = Convert.ToInt32(relationProperties["Attributes"]);
                relation.ForeignTable = Convert.ToString(relationProperties["ForeignTable"]);
                relation.Table = Convert.ToString(relationProperties["Table"]);
                relation.PartialReplica = Convert.ToBoolean(relationProperties["PartialReplica"]);

                import.ReadLine();
                while (!import.IsEnd) {
                    dao.Field field = new dao.FieldClass();
                    field.Name = import.PropertyValue();
                    import.ReadLine();
                    field.ForeignName = import.PropertyValue();
                    import.ReadLine(2);

                    relation.Fields.Append(field);
                }

                relations.Append(relation);
            }


        }

        public override object this[string propertyName] {
            get { return null; }
        }

        public override string ClassName {
            get { return "Relations"; }
        }

        public override object DaoObject {
            get {
                return this.relations;
            }
            set {
                if (value != null && !(value is dao.Relations))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(dao.Relations).Name));
                relations = (dao.Relations)value;
            }
        }

    }
}
