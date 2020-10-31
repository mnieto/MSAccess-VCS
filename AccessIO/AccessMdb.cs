using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using dao = Microsoft.Office.Interop.Access.Dao;

namespace AccessIO {
    public class AccessMdb : AccessApp {

        private dao.Database database;
        private const char tempPrefix = '~';    //prefix for temp objects in MSAccess.

        /// <summary>
        /// Underlying <see cref="dao.Database"/> object
        /// </summary>
        public dao.Database Database {
            get {
                if (database == null)
                    database = Application.CurrentDb();
                return database;
            }
            set {
                database = value;
            }
        }

        /// <summary>
        /// If <c>true</c> and table has data, <see cref="Load"/> method will overwrite the table structure and data will lost
        /// If <c>false</c> and table has data, <see cref="Load"/> method will raise an exception
        /// </summary>
        /// <remarks>
        /// Controls the default value for the <see cref="Table.AllowDataLost"/> property of the database tables
        /// </remarks>
        public bool AllowDataLost { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Path and file name of the .mdb file</param>
        public AccessMdb(string fileName) {
            this.FileName = fileName;
            this.ProjectType = AccessProjectType.Mdb;
            this.AllowedContainers = new ContainersMdb();
        }

        public override List<IObjecOptions> LoadObjectNames(string containerInvariantName) {
        	
            Database = Application.CurrentDb();

            ContainerNames container = AllowedContainers.Find(containerInvariantName);
            if (container == null)
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");

            List<IObjecOptions> lst = new List<IObjecOptions>();
            if (containerInvariantName == ObjectType.General.ToString()) {
                lst.Add(new ObjectOptions(Properties.Resources.DatabaseProperties, ObjectType.DatabaseDao));
                lst.Add(new ObjectOptions(Properties.Resources.References, ObjectType.References));
                lst.Add(new ObjectOptions(Properties.Resources.Relations, ObjectType.Relations));
            } else if (IsStandardContainerName(container.InvariantName)) {
                dao.Container daoContainer = Database.Containers[container.InvariantName];
                foreach (dao.Document doc in daoContainer.Documents) {
                    lst.Add(new ObjectOptions(doc.Name, container.DefaultObjectType));
                }
            } else {
                lst.AddRange(GetDaoObjects(container.InvariantName));
            }
            return lst;
        }



        private bool IsStandardContainerName(string containerName) {
            if (containerName == "Tables")      //We need to do some extra filtering on table's collection
                return false;
            for (int i = 0; i < Database.Containers.Count; i++) {
                if (Database.Containers[i].Name == containerName)
                    return true;
            }
            return false;

        }

        private IEnumerable<IObjecOptions> GetDaoObjects(string containerName) {
            switch (containerName) {
                case "Tables":
                    return GetTables();
                case "Queries":
                    return GetQueries();
                case "Relations":
                    return GetRelations();
                default:
                    throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "containerName");

            }
        }

        private IEnumerable<IObjecOptions> GetRelations() {
            return new List<IObjecOptions>();
        }

        private IEnumerable<IObjecOptions> GetQueries() {
            List<IObjecOptions> lst = new List<IObjecOptions>();
            foreach (dao.QueryDef qry in Database.QueryDefs) {
                if (qry.Name[0] != tempPrefix) {
                    lst.Add(new ObjectOptions(qry.Name, ObjectType.Query));
                }
            }
            return lst;
        }

        private IEnumerable<IObjecOptions> GetTables() {
            const int systemTable = -2147483648;
            List<IObjecOptions> lst = new List<IObjecOptions>();
            foreach (dao.TableDef tableDef in Database.TableDefs) {
                bool isSystemTable = tableDef.Attributes == 2 || tableDef.Attributes == systemTable;
                isSystemTable = isSystemTable || (tableDef.Name[0] == tempPrefix);
                if (!isSystemTable) {
                    lst.Add(new ObjectOptions(tableDef.Name, ObjectType.Table));
                }
            }
            return lst;
        }


        public override void OpenDatabase() {
            //TODO: Check for password protected databases
            //TODO: Check for databases attached to workgroup database
            string fullFileName = System.IO.Path.GetFullPath(FileName);
            dao.Database db = Application.DBEngine.OpenDatabase(System.IO.Path.GetFullPath(fullFileName));
            try {
                if (double.Parse(db.Version, System.Globalization.CultureInfo.InvariantCulture) < 4.0)
                    throw new Exception(Properties.ImportRes.InvalidFileFormat);
                else {
                    string accVersion = db.Properties["AccessVersion"].Value.ToString();
                    accVersion = accVersion.Substring(0, accVersion.IndexOf('.'));
                    if (int.Parse(accVersion) < 9)
                        throw new Exception(Properties.ImportRes.InvalidFileFormat);
                }
            } finally {
                db.Close();
            }
            Application.OpenCurrentDatabase(fullFileName);
        }

    }
}
