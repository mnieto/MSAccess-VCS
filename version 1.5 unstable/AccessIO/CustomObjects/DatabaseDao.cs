using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;

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

        public DatabaseDao(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) {
            DaoObject = app.Application.CurrentDb();
        }

        public override object this[string propertyName] {
            get { return this.daoDatabase.Properties[propertyName].Value; }
        }
        
        public override void Load(string fileName) {
            throw new NotImplementedException();
        }

    }
}
