using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;

namespace AccessIO {
    public class AccessAdp : AccessApp {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileName">Path and file name of the .mdb file</param>
        public AccessAdp(string fileName) {
            this.FileName = fileName;
            this.ProjectType = AccessProjectType.Adp;
            this.AllowedContainers = new ContainersAdp();
        }

        /// <summary>
        /// Creates an array with the valid object types for this access file
        /// </summary>
        protected override void InitializeAllowedObjetTypes() {
            AllowedObjetTypes = new ObjectType[]  {
                ObjectType.Form,
                ObjectType.Report,
                ObjectType.DataAccessPage,  //Partially supported because SaveAsText export it to binary format and this object is deprecatted begining with Office 2007
                ObjectType.Module,
                ObjectType.Macro,
                ObjectType.Property,
                ObjectType.References
            };
        }


        public override List<AccessIO.IObjecOptions> LoadObjectNames(string containerInvariantName) {
            ContainerNames container = AllowedContainers.Find(containerInvariantName);
            if (container == null)
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");

            List<IObjecOptions> lst = new List<IObjecOptions>();
            if (containerInvariantName == ObjectType.General.ToString()) {
                lst.Add(new ObjectOptions(Properties.Resources.DatabaseProperties, ObjectType.DatabasePrj));
                lst.Add(new ObjectOptions(Properties.Resources.References, ObjectType.References));
            } else {
                foreach (Access.AccessObject obj in GetObjectCollectionFromObjectType(container.DefaultObjectType)) {
                    lst.Add(new ObjectOptions(obj.Name, container.DefaultObjectType));
                }
            }
            return lst;
        }

        private Access.AllObjects GetObjectCollectionFromObjectType(ObjectType objectType) {
            switch (objectType) {
                case ObjectType.DataAccessPage:
                    return Application.CurrentProject.AllDataAccessPages;
                case ObjectType.Form:
                    return Application.CurrentProject.AllForms;
                case ObjectType.Macro:
                    return Application.CurrentProject.AllMacros;
                case ObjectType.Module:
                    return Application.CurrentProject.AllModules;
                case ObjectType.Report:
                    return Application.CurrentProject.AllReports;
                default:
                    throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");
            }
        }

        public override void CreateDatabase() {
            Application.NewCurrentDatabase(FileName);
        }

        public override void CreateDatabase(Dictionary<string, object> databaseProperties) {
            //TODO: Check to create access project with connection string
            Application.NewAccessProject(FileName, databaseProperties["connectionString"].ToString());
        }

        public override void OpenDatabase() {
            Application.OpenAccessProject(FileName);
            if ((int)Application.CurrentProject.FileFormat < 10)
                throw new Exception(Properties.ImportRes.InvalidFileFormat);
        }


    }
}
