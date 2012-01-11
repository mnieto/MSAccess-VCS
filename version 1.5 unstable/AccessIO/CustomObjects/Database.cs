using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AccessIO {

    /// <summary>
    /// Base class for Database wrapper objects
    /// </summary>
    public abstract class Database : CustomObject {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app"><see cref="AccessApp"/> object</param>
        /// <param name="name">database file name</param>
        /// <param name="objectType">instance of <see cref="ObjectType"/> with ObjectType.DatabasePrj or ObjectType.DatabaseDao value</param>
        public Database(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) { }

        public override string ClassName {
            get { return "Database"; }
        }


        public override void Load(string fileName) {
            Dictionary<string, object> databaseProperties = null;
            using (StreamReader sr = new StreamReader(fileName)) {
                ImportObject import = new ImportObject(sr);
                import.ReadLine(2);
                databaseProperties = import.ReadProperties();
            }

            if ((Options as OptionsDatabase).OverwriteDatabase) {
                App.CloseDatabase();
                int i = 0;
                do {
                    try {
                        System.Threading.Thread.Sleep(100);
                        File.Delete(App.FileName);
                    } catch {
                        i++;
                    }
                } while (i < 5);
                App.CreateDatabase(databaseProperties);
            } else {
                ClearProperties();
            }
            InternalLoad(databaseProperties);

        }

        /// <summary>
        /// Restores the database properties readed from file to the database
        /// </summary>
        /// <param name="databaseProperties">List of readed properties</param>
        /// <remarks>
        /// <para>
        /// Be aware that not all readed properties exists as database properties. Some can exists, and some not. 
        /// Other can be user properties that are not defined at all by Microsoft
        /// </para>
        /// <para>
        /// Diferent database types (mdb, adp) save properties in diferent collections, 
        /// so inherited classes must treat the properties as corresponding
        /// </para>
        /// </remarks>
        protected abstract void InternalLoad(Dictionary<string, object> databaseProperties);

        /// <summary>
        /// Read the database properties and save them into a collection
        /// </summary>
        /// <returns>A dictionary with the properties to save to</returns>
        /// <remarks>
        /// <para>
        /// Not all properties are suitable for saving. For example, RecordsAffected
        /// can vary each time we save, but does not provide relevant information
        /// </para>
        /// <para>
        /// Each descendant must implement this method and decide which properties save or not
        /// </para>
        /// </remarks>
        protected abstract Dictionary<string, IPropertyTransform> gatherProperties();

        /// <summary>
        /// Remove the current properties of the Access database/project object
        /// </summary>
        /// <remarks>
        /// Not all properties are removed: some properties are read only
        /// </remarks>
        protected abstract void ClearProperties();
    }
}
