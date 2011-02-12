using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AccessIO {

    /// <summary>
    /// Base class for Database wrapper objects
    /// </summary>
    public abstract class Database : CustomObject {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app"><see cref="AccessApp"/> object</param>
        /// <param name="name">database file name</param>
        /// <param name="objectType">instance of <see cref="ObjectType"/> with ObjectType.DatabasePrj or ObjectType.DatabaseDao value</param>
        public Database(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) { }

        public override string ClassName {
            get { return "Database"; }
        }

        public override void Save(string fileName) {

            //Add known properties with special treatment
            //other properties are not allways present. If they are present add them and write with NoActionTransform
            Dictionary<string, IPropertyTransform> propsToWrite = new Dictionary<string, IPropertyTransform>();
            NullTransform nullTransform = new NullTransform();
            NoActionTransform noActionTransform = new NoActionTransform();
            propsToWrite.Add("Name", new FileNameTransform());
            propsToWrite.Add("Transactions", nullTransform);
            propsToWrite.Add("RecordsAffected", nullTransform);
            propsToWrite.Add("AccessVersion", nullTransform);
            propsToWrite.Add("Build", nullTransform);
            propsToWrite.Add("ProjVer", nullTransform);
            propsToWrite.Add("Connection", nullTransform);
            propsToWrite.Add("Version", nullTransform);

            dao.Database db = App.Application.CurrentDb();
            foreach (dao.Property prop in db.Properties) {
                if (!propsToWrite.ContainsKey(prop.Name)) {
                    propsToWrite.Add(prop.Name, noActionTransform);
                }
            }

            //Make sure the path exists
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            //Write the properties with help of an ExportObject
            using (StreamWriter sw = new StreamWriter(fileName)) {
                ExportObject export = new ExportObject(sw);
                string dbFileName = System.IO.Path.GetFileName(App.FileName);
                export.WriteBegin(ClassName, dbFileName);
                foreach (string item in propsToWrite.Keys) {
                    propsToWrite[item].WriteTransform(export, db.Properties[item]);
                }
                export.WriteEnd();
            }
        }
    }
}
