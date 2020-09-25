using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using System.IO;

namespace AccessIO {
    
    /// <summary>
    /// Database wrapper for access projects databases
    /// </summary>
    public class DatabasePrj : Database {

        private Access.CurrentProject project;

        public override object DaoObject {
            get {
                return this.project;
            }
            set {
                if (value != null && !(value is Access.CurrentProject))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(Access.CurrentProject).Name));
                project = (Access.CurrentProject)value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app"><see cref="AccessApp"/> object</param>
        /// <param name="name">database file name</param>
        /// <param name="objectType">instance of <see cref="ObjectType"/> with ObjectType.DatabasePrj value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214", Justification="base constructor is called explicitly and there is not interaction with another member variables")]
        public DatabasePrj(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) {
            DaoObject = app.Application.CurrentProject;
        }

        public override object this[string propertyName] {
            get { return this.project.Properties[propertyName].Value; }
        }

        public override void Save(string fileName) {

            Dictionary<string, IPropertyTransform> propsToWrite = gatherProperties();

            //Make sure the path exists
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            //Write the properties with help of an ExportObject
            using (StreamWriter sw = new StreamWriter(fileName)) {
                ExportObject export = new ExportObject(sw);
                string dbFileName = System.IO.Path.GetFileName(App.FileName);
                export.WriteBegin(ClassName, dbFileName);
                foreach (string item in propsToWrite.Keys) {
                    propsToWrite[item].WriteTransform(export, App.Application.CurrentProject.Properties[item]);
                }
                export.WriteEnd();
            }

        }


        protected override Dictionary<string, IPropertyTransform> gatherProperties() {
            Dictionary<string, IPropertyTransform> propsToWrite = new Dictionary<string, IPropertyTransform>();
            NullTransform nullTransform = new NullTransform();
            NoActionTransform noActionTransform = new NoActionTransform();
            propsToWrite.Add("AccessVersion", nullTransform);
            propsToWrite.Add("Build", nullTransform);
            propsToWrite.Add("ProjVer", nullTransform);

            foreach (Access.AccessObjectProperty prop in App.Application.CurrentProject.Properties) {
                if (!propsToWrite.ContainsKey(prop.Name))
                    propsToWrite.Add(prop.Name, noActionTransform);
            }
            return propsToWrite;
        }

        protected override void InternalLoad(System.Collections.Generic.Dictionary<string, object> databaseProperties) {

            //Generates 0x80030005 (STG_E_ACCESSDENIED) exception if Access project is not connected to a sql database
            Access.AccessObjectProperties properties = App.Application.CurrentProject.Properties;
            PropertyCollectionAccessObject propertyCollection = new PropertyCollectionAccessObject(properties);
            foreach (KeyValuePair<string, object> item in databaseProperties) {
                propertyCollection.AddProperty(item.Key, item.Value);
            }
        }

        protected override void ClearProperties() {
            Access.AccessObjectProperties properties = App.Application.CurrentProject.Properties;
            foreach (Access.AccessObjectProperty property in properties) {
                try {
                    //If we delete AccessVersion property the database will be Access 2000 format
                    if (property.Name != "AccessVersion")
                        properties.Remove(property);
                } catch { 
                    //Ignore any exception
                }
            }
        }

    }
}
