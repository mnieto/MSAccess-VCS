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


        public override List<AccessIO.IObjectName> LoadObjectNames(string containerInvariantName) {
            ContainerNames container = AllowedContainers.Find(containerInvariantName);
            if (container == null)
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");

            List<IObjectName> lst = new List<IObjectName>();
            if (containerInvariantName == ObjectType.General.ToString()) {
                lst.Add(new ObjectName(Properties.Resources.DatabaseProperties, ObjectType.DatabasePrj));
                lst.Add(new ObjectName(Properties.Resources.References, ObjectType.References));
            } else {
                foreach (Access.AccessObject obj in GetObjectCollectionFromObjectType(container.DefaultObjectType)) {
                    lst.Add(new ObjectName(obj.Name, container.DefaultObjectType));
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


    }
}
