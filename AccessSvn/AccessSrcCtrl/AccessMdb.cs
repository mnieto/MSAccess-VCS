using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;

namespace AccessIO {
    public class AccessMdb : AccessApp {
        
        private dao.Database database;

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
        }

        /// <summary>
        /// Creates an array with the valid object types for this access file
        /// </summary>
        protected override void InitializeAllowedObjetTypes() {
            AllowedObjetTypes = new ObjectType[]  {
                ObjectType.Table,
                ObjectType.Query,
                ObjectType.Form,
                ObjectType.Report,
                ObjectType.DataAccessPage,
                ObjectType.Module,
                ObjectType.Macro,
                ObjectType.Default
            };
        }


        public override System.Collections.Generic.List<AccessIO.IObjectName> LoadObjectNames(ObjectType objectType) {
            
            Database = Application.CurrentDb();

            if (!IsAllowedType(objectType))
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");

            List<IObjectName> lst = new List<IObjectName>();
            if (objectType == ObjectType.Default) {
                lst.Add(new ObjectName(Properties.Resources.DatabaseProperties, ObjectType.DatabaseDao));
                lst.Add(new ObjectName(Properties.Resources.References, ObjectType.References));
                lst.Add(new ObjectName(Properties.Resources.Relations, ObjectType.Relations));
            } else {
                string containerName = GetContainerNameFromObjectType(objectType);
                if (IsStandardContainerName(containerName)) {
                    dao.Container container = Database.Containers[containerName];
                    foreach (dao.Document doc in container.Documents) {
                        lst.Add(new ObjectName(doc.Name, objectType));
                    }
                } else {
                    lst.AddRange(GetDaoObjects(containerName));
                }
            }
            return lst;
        }

        private const char tempPrefix = '~';    //prefix for temp objects in MSAccess.

        private bool IsStandardContainerName(string containerName) {
            if (containerName == "Tables")      //We need to do some extra filtering on table's collection
                return false;
            for (int i = 0; i < Database.Containers.Count; i++) {
                if (Database.Containers[i].Name == containerName)
                    return true;
            }
            return false;

        }



        private IEnumerable<IObjectName> GetDaoObjects(string containerName) {
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

        private IEnumerable<IObjectName> GetRelations() {
            return new List<IObjectName>();
        }

        private IEnumerable<IObjectName> GetQueries() {
            List<IObjectName> lst = new List<IObjectName>();
            foreach (dao.QueryDef qry in Database.QueryDefs) {
                if (qry.Name[0] != tempPrefix) {
                    lst.Add(new ObjectName(qry.Name, ObjectType.Query));
                }
            }
            return lst;
        }

        private IEnumerable<IObjectName> GetTables() {
            const int systemTable = -2147483648;
            List<IObjectName> lst = new List<IObjectName>();
            foreach (dao.TableDef tableDef in Database.TableDefs) {
                bool isSystemTable = tableDef.Attributes == 2 || tableDef.Attributes == systemTable;
                isSystemTable = isSystemTable || (tableDef.Name[0] == tempPrefix);
                if (!isSystemTable) {
                    lst.Add(new ObjectName(tableDef.Name, ObjectType.Table));
                }
            }
            return lst;
        }



    }
}
