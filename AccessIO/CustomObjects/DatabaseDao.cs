using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using dao = Microsoft.Office.Interop.Access.Dao;

namespace AccessIO {

    /// <summary>
    /// Database wrapper object for dao databases
    /// </summary>
    public class DatabaseDao : Database {

        dao.Database daoDatabase;

        public override object DaoObject {
            get {
                return this.daoDatabase;
            }
            set {
                daoDatabase = (dao.Database)value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app"><see cref="AccessApp"/> object</param>
        /// <param name="name">database file name</param>
        /// <param name="objectType">instance of <see cref="ObjectType"/> with ObjectType.DatabaseDao value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214", Justification="base constructor is called explicitly and there is not interaction with another member variables")]
        public DatabaseDao(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) {
            DaoObject = app.Application.CurrentDb();
        }

        public override object this[string propertyName] {
            get { return this.daoDatabase.Properties[propertyName].Value; }
        }

        /// <summary>
        /// Save the database properties to a file
        /// </summary>
        /// <param name="fileName">Name of the file where to save the properties</param>
        /// <remarks>
        /// <para>
        /// To facilitate subsequent read operations include the data type of the property at the beginning, as part of property value.
        /// The property is written in the format "propertyName: DataType,propertyValue" with no spaces between dataType and propertyValue.
        /// </para>
        /// <para>
        /// The xxxxxDataTypeTransform family has the same functionality than xxxxTransfor, 
        /// except that inserts the data type before the propertyValue
        /// </para>
        /// </remarks>
        public override void Save(string fileName) {

            Dictionary<string, IPropertyTransform> propsToWrite = gatherProperties();

            //Make sure the path exists
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            //Write the properties with help of an ExportObject
            dao.Database db = App.Application.CurrentDb();
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
        protected override Dictionary<string, IPropertyTransform> gatherProperties() {

            //Add known properties with special treatment
            //other properties are not allways present. If they are present add them and write with NoActionTransform
            //Use NullTransform for read only properties that do not should be written
            Dictionary<string, IPropertyTransform> propsToWrite = new Dictionary<string, IPropertyTransform>();
            NullTransform nullTransform = new NullTransform();
            NoActionDataTypeTransoform noActionTransform = new NoActionDataTypeTransoform();
            propsToWrite.Add("Name", new FileNameDataTypeTransform());
            propsToWrite.Add("Transactions", nullTransform);
            propsToWrite.Add("RecordsAffected", nullTransform);
            propsToWrite.Add("Build", nullTransform);
            propsToWrite.Add("Updatable", nullTransform);
            propsToWrite.Add("Connection", nullTransform);
            propsToWrite.Add("Version", nullTransform);

            dao.Database db = App.Application.CurrentDb();
            foreach (dao.Property prop in db.Properties) {
                if (!propsToWrite.ContainsKey(prop.Name)) {
                    propsToWrite.Add(prop.Name, noActionTransform);
                }
            }

            return propsToWrite;
            
        }
        
        protected override void InternalLoad(System.Collections.Generic.Dictionary<string, object> databaseProperties) {
            dao.Properties daoProperties = (dao.Properties)((dao.Database)DaoObject).Properties;
            PropertyCollectionDao properties = new PropertyCollectionDao(DaoObject, (dao.Properties)((dao.Database)DaoObject).Properties);
            foreach (KeyValuePair<string, object> item in databaseProperties) {

                //initialize default values
                int dataType = (int)dao.DataTypeEnum.dbText;
                string propertyValue = String.Empty;
                
                //Split property "value" in dataType,propertyValue. NOTE: In version 1.0, property value only contained the value itself, not the data type
                //the first group matches one or more digits followed by colon
                //    ?: is used for non capture group (if data type do not exists)
                //inside the first group is the dataType group
                //the second group matches any character after the colon
                Match match = Regex.Match(item.Value.ToString(), @"^(?:(?<dataType>[0-9]+),)*(?<value>.*)$");
                if (match.Success) {
                    if (match.Groups["dataType"].Success)
                        dataType = int.Parse(match.Groups["dataType"].Value);
                    if (match.Groups["value"].Success)
                        propertyValue = match.Groups["value"].Value;

                    if (propertyValue != String.Empty) {
                        properties.AddProperty(item.Key, propertyValue, (dao.DataTypeEnum)dataType);
                    }
                }
            }
        }

        protected override void ClearProperties() {
            dao.Properties properties = App.Application.CurrentDb().Properties;

            //If we delete AccessVersion property the database will be Access 2000 format
            string[] readOnlyProperties = new string[] {    "AccessVersion", "Name", "Connect", 
                                                            "Transactions", "Updatable", "CollatingOrder",
                                                            "QueryTimeout", "Version", "RecordsAffected", 
                                                            "ReplicaID", "DesignMasterID", "Connection" };

            foreach (dao.Property property in properties) {
                try {
                    if (!Array.Exists<string>(readOnlyProperties, p => p.Equals(property.Name)))
                        properties.Delete(property.Name);
                } catch (System.Runtime.InteropServices.COMException ex) {
                    System.Diagnostics.Debug.Print(String.Format("Property: {0}, Error {1}{2}", property.Name, ex.ErrorCode, ex.Message));
                }
            }
        }
    }
}
