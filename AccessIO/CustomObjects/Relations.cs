using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dao = Microsoft.Office.Interop.Access.Dao;

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
                    //try { export.WriteProperty("PartialReplica", relation.PartialReplica); } catch { }      //Accessing this property causes an exception ¿?
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
            dao.Database db = App.Application.CurrentDb();
            dao.Relations relations = db.Relations;
            foreach (dao.Relation item in relations) {
                relations.Delete(item.Name);
            }
            relations.Refresh();

            using (StreamReader sr = new StreamReader(fileName)) {
                ImportObject import = new ImportObject(sr);
                import.ReadLine(2);      //Read 'Begin Relations' and 'Begin Relation' lines

                do {
                    string relationName = import.PeekObjectName();
                    Dictionary<string, object> relationProperties = import.ReadProperties();

                    dao.Relation relation = db.CreateRelation(relationName);
                    relation.Attributes = Convert.ToInt32(relationProperties["Attributes"]);
                    relation.ForeignTable = Convert.ToString(relationProperties["ForeignTable"]);
                    relation.Table = Convert.ToString(relationProperties["Table"]);
                    //try { relation.PartialReplica = Convert.ToBoolean(relationProperties["PartialReplica"]); } catch { }  //Accessing this property causes an exception ¿?

                    import.ReadLine(2);         //Read 'Begin Fields' and 'Begin Field' lines
                    while (!import.IsEnd) {
                        dao.Field field = relation.CreateField();
                        field.Name = import.PropertyValue();
                        import.ReadLine();
                        field.ForeignName = import.PropertyValue();
                        import.ReadLine(2);     //Read 'End Field' and ('Begin Field' or 'End Fields'

                        relation.Fields.Append(field);
                    }

                    import.ReadLine(2);         //Read 'End Relation' and ('Begin Relation or 'End Relations')
                    relations.Append(relation);

                } while (!import.IsEnd);
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
