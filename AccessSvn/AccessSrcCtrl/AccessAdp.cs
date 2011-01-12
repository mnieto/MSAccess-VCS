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
        }

        /// <summary>
        /// Creates an array with the valid object types for this access file
        /// </summary>
        protected override void InitializeAllowedObjetTypes() {
            AllowedObjetTypes = new ObjectType[]  {
                ObjectType.Form,
                ObjectType.Report,
                ObjectType.DataAccessPage,
                ObjectType.Module,
                ObjectType.Macro,
                ObjectType.Property,
                ObjectType.References
            };
        }


        public override System.Collections.Generic.List<AccessIO.IObjectName> LoadObjectNames(ObjectType objectType) {
            if (!IsAllowedType(objectType))
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");

            List<IObjectName> lst = new List<IObjectName>();
            if (objectType == ObjectType.Default) {
                lst.Add(new ObjectName(Properties.Resources.DatabaseProperties, ObjectType.DatabasePrj));
                lst.Add(new ObjectName(Properties.Resources.References, ObjectType.References));
            } else {
                foreach (AccessObject obj in GetObjectCollectionFromObjectType(objectType)) {
                    lst.Add(new ObjectName(obj.Name, objectType));
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
