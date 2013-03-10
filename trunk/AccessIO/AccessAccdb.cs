using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessIO {
    public class AccessAccdb: AccessApp {

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

        public AccessAccdb(string fileName) {
            this.FileName = fileName;
            this.ProjectType = AccessProjectType.Accdb;
            this.AllowedContainers = new ContainersAccdb();
        }

        public override void OpenDatabase() {
            Application.OpenCurrentDatabase(FileName);
            if (GetAccessVersion(Application.CurrentDb().Version) < 12)
                throw new Exception(Properties.ImportRes.Invalid2007FileFormat);
        }

        public override void CreateDatabase() {
            Application.NewCurrentDatabase(System.IO.Path.GetFullPath(FileName));
        }

        public override void CreateDatabase(Dictionary<string, object> databaseProperties) {
            throw new NotImplementedException();
        }

        protected override void InitializeAllowedObjetTypes() {
            AllowedObjetTypes = new ObjectType[]  {
                ObjectType.Form,
                ObjectType.Report,
                ObjectType.Module,
                ObjectType.Macro,
                ObjectType.Property,
                ObjectType.References
            };
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

        private int GetAccessVersion(string accessVersion) {
            int dotPosition = accessVersion.IndexOf('.');
            if (dotPosition != -1)
                accessVersion = accessVersion.Substring(0, dotPosition);
            return Convert.ToInt32(accessVersion);
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


    }
}
