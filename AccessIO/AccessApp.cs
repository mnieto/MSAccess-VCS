using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using System.IO;

namespace AccessIO {

    /// <summary>
    /// Base class for both access file formats: mdb and adp
    /// </summary>
    /// <remarks>
    /// Allow to get access uniformly to the different object types of an access file
    /// </remarks>
    public abstract class AccessApp : IDisposable {

        /// <summary>
        /// Access.Application interop object
        /// </summary>
        public Access.Application Application { get; protected set; }

        /// <summary>
        /// Project type (mdb or adp)
        /// </summary>
        protected AccessProjectType ProjectType { get; set; }

        /// <summary>
        /// File name of the Access database/project file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// List of ObjectType object allowed depending on the file type: database or project
        /// </summary>
        public Containers AllowedContainers { get; protected set; }
        
        /// <summary>
        /// List of ObjectType object allowed depending on the file type: database or project
        /// </summary>
        public ObjectType[] AllowedObjetTypes {
            get;
            protected set;
        }

        /// <summary>
        /// Create a new instance of an AccessApp derived class, depending on the file name extension of <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">File name of the Access database/project file</param>
        /// <returns>Returns an initialized AccessApp derived class</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability","CA2000:DisposeObjectsBeforeLosingScope")]
        public static AccessApp AccessAppFactory(string fileName) {
            AccessApp app = null;
            switch (Path.GetExtension(fileName).ToUpperInvariant()) { 
                case ".MDE":
                case ".MDB":
                    app = new AccessMdb(fileName);
                    break;
                case ".ADP":
                case ".ADE":
                    app = new AccessAdp(fileName);
                    break;
                case ".ACCDB":
                    app = new AccessAccdb(fileName);
                    break;
            }
            app.IntanceAccessApplication();
            app.InitializeAllowedObjetTypes();
            return app;

        }

        /// <summary>
        /// Create a new instance of <see cref="Containers"/> derived class, depending on the file name extension of <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">File name of the Access database/project file</param>
        /// <returns>specialiced Containers object</returns>
        public static Containers ContainersFactory(string fileName) {
            Containers containers = null;
            switch (Path.GetExtension(fileName).ToUpperInvariant()) {
                case ".MDE":
                case ".MDB":
                case ".MDA":
                    containers = new ContainersMdb();
                    break;
                case ".ADP":
                case ".ADE":
                    containers = new ContainersAdp();
                    break;            
                case ".ACCDB":
                    containers = new ContainersAccdb();
                    break;
            }
            return containers;
        }

        /// <summary>
        /// Creates a new instance of Access.Application interop object and loads the database or project file
        /// </summary>
        /// <exception cref="COMException">when user cancel security message in databases with VBA code or when Macros security level is high</exception>
        protected virtual void IntanceAccessApplication() {

            Application = new Access.Application();
            Application.UserControl = false;
            Application.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityLow;
            Application.Visible = false;
        }

        /// <summary>
        /// Opens the database specified by <see cref="FileName"/>
        /// </summary>
        public abstract void OpenDatabase();

        /// <summary>
        /// Create a new database of the appropiate type
        /// </summary>
        public abstract void CreateDatabase();

        /// <summary>
        /// Create a new database of the appropiate type and use database properties to initialize it
        /// </summary>
        /// <param name="databaseProperties">List of properties readed from file. Not all properties are used to create the database</param>
        public abstract void CreateDatabase(Dictionary<string, object> databaseProperties);

        /// <summary>
        /// Close the current database
        /// </summary>
        public virtual void CloseDatabase() {
            if (Application != null)
                Application.CloseCurrentDatabase();
        }

        /// <summary>
        /// Free the MS Access instance
        /// </summary>
        public void QuitApplication() {
            if (Application != null) {
                Application.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Application);
                Application = null;
            }
            
        }

        /// <summary>
        /// Initialize the allowed ObjectType depending on the file type: database or project
        /// </summary>
        protected abstract void InitializeAllowedObjetTypes();

        /// <summary>
        /// Returns a friendly contanier name for each Access object type
        /// </summary>
        /// <param name="objectType">Access object Type</param>
        /// <returns>string with the container name</returns>
        [Obsolete("Use Containers object", true)]
        public static string GetContainerNameFromObjectType(ObjectType objectType) {
            switch (objectType) {
                case ObjectType.DatabaseDao:
                case ObjectType.DatabasePrj:
                    //return "Database";
                    return string.Empty;
                case ObjectType.DataAccessPage:
                    return "DataAccessPages";
                case ObjectType.Diagram:
                case ObjectType.Relations:
                    //return "Relations";
                    return String.Empty;
                case ObjectType.Form:
                    return "Forms";
                case ObjectType.Macro:
                    return "Scripts";
                case ObjectType.Module:
                    return "Modules";
                case ObjectType.Query:
                    return "Queries";
                case ObjectType.Report:
                    return "Reports";
                case ObjectType.Table:
                    return "Tables";
                case ObjectType.References:
                    //return "References";
                    return String.Empty;
                default:
                    throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException, "objectType");
            }
        }

        #region IDisposable Members

        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispoing) {
            if (!this.disposed) {
                if (dispoing) {
                    QuitApplication();
                }
                disposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Returns a list with the object names of a determined type
        /// </summary>
        /// <param name="objectType">Object type to query</param>
        public abstract List<IObjecOptions> LoadObjectNames(string containerInvariantName);

        /// <summary>
        /// Returns if a ObjectType is valid or not for the project type
        /// </summary>
        /// <param name="objectType">ObjectType to query</param>
        /// <returns><c>true</c> if is an allowed type, else returns <c>false</c></returns>
        [Obsolete("Use AllowedContainers.Find: if returns null, then the object type is not allowed")]
        protected virtual bool IsAllowedType(ObjectType objectType) {
            foreach (ObjectType ot in AllowedObjetTypes) {
                if (ot == objectType)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of <see cref="ObjectOptions"/> 
        /// </summary>
        /// <returns></returns>
        protected virtual List<ObjectOptions> GetReferences() {
            List<ObjectOptions> lst = new List<ObjectOptions>();
            foreach (Access.Reference reference in Application.References) {
                if (!reference.BuiltIn) {
                    lst.Add(new ObjectOptions(reference.Name, ObjectType.References));
                }
            }
            return lst;
        }

        /// <summary>
        /// Absolute or relative path to the base directory of svn working copy
        /// </summary>
        public string SvnBasePath { get; set; }

        /// <summary>
        /// Root local path for the working copy
        /// </summary>
        public string WorkingCopyPath { get; set; }

    }
}
