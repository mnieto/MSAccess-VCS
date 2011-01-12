using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;

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
        public DatabasePrj(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) {
            DaoObject = app.Application.CurrentProject;
        }

        public override object this[string propertyName] {
            get { return this.project.Properties[propertyName].Value; }
        }

        public override void Load(string fileName) {
            throw new NotImplementedException();
        }

        public override void SaveProperties(ExportObject export) {
            throw new NotImplementedException();
        }
    }
}
