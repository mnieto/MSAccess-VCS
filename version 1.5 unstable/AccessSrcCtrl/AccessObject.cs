using System;
using System.Collections.Generic;
using System.Text;
using Access = Microsoft.Office.Interop.Access;
using System.IO;

namespace AccessIO {
    /// <summary>
    /// Base class for all wrappers for Access objects (dao and access objects)
    /// </summary>
    public abstract class AccessObject: IObjectName {
        /// <summary>
        /// Create an Access object
        /// </summary>
        /// <param name="name">Object's name in Access database</param>
        /// <param name="objectType">Object type</param>
        public AccessObject(AccessApp app, string name, ObjectType objectType) {
            this.App = app;
            this.Name = name;
            this.ObjectType = objectType;
        }

        /// <summary>
        /// Access object name
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// Gets Container\Object name
        ///// </summary>
        //public string Path {
        //    get {
        //        return String.Concat(AccessApp.GetContainerNameFromObjectType(ObjectType),
        //                             System.IO.Path.DirectorySeparatorChar,
        //                             Name);
        //    }
        //}

        /// <summary>
        /// Get access to <see cref="AccessApp"/> object
        /// </summary>
        protected AccessApp App { get; private set; }

        /// <summary>
        /// Access object type
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Save an object to a file
        /// </summary>
        /// <param name="fileName">File name containing the object's definition</param>
        public abstract void Save(string fileName);

        /// <summary>
        /// Save an object to a file
        /// </summary>
        /// <remarks>The file name is calculated based on object name and type</remarks>
        public virtual void Save() {
            if (String.IsNullOrEmpty(App.WorkingCopyPath))
                throw new InvalidOperationException(Properties.Resources.WorkingCopyMissing);

            string filePath = null;
            ObjectTypeExtension ote = App.AllowedContainers.Find(ObjectType);
            string fileName = string.Format("{0}.{1}.txt", NormalizeObjectName(this.Name), ote.FileExtension);
            filePath = System.IO.Path.Combine(App.WorkingCopyPath, String.Concat(ote.Container.InvariantName, System.IO.Path.DirectorySeparatorChar, fileName));
            Save(filePath);
        }

        /// <summary>
        /// Make sure the directory exits
        /// </summary>
        /// <param name="path"></param>
        protected virtual void MakePath(string path) {
            System.IO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Gets an Access object's name and make sure that is a valid file name
        /// </summary>
        /// <param name="name">Access object's name</param>
        /// <returns>Object's name without invalid file name chars</returns>
        protected virtual string NormalizeObjectName(string name) {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) { 
                name = name.Replace(c.ToString(), String.Empty);
            }
            return name;
        }

        /// <summary>
        /// Load an object from a file
        /// </summary>
        /// <param name="fileName">File name containing the object's definition</param>
        public abstract void Load(string fileName);

        /// <summary>
        /// Create a inherited object instance
        /// </summary>
        /// <param name="objectType">Access object type</param>
        /// <param name="name">Object's name</param>
        public static AccessObject CreateInstance(AccessApp app, ObjectType objectType, string name) {
            AccessObject newObject;
            if (IsStandardContainerName(objectType.ToString()))
                newObject = new StandardObject(app, name, objectType);
            else {
                if (IsStandardObjectType(objectType))
                    newObject = new StandardObject(app, name, objectType);
                else {
                    string typeName = String.Concat(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, ".", objectType.ToString());
                    Type t = Type.GetType(typeName);
                    newObject = (AccessObject)Activator.CreateInstance(t, app, name, objectType);
                }
            }
            return newObject;

        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="containerName"/> is supported by LoadFromText and SaveAsText
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        protected static bool IsStandardContainerName(string containerName) {
            switch (containerName) {
                case "DataAccessPages":
                case "Forms":
                case "Queries":
                case "Modules":
                case "Reports":
                case "Scripts":
                    return true;
                default:
                    return false;
            }
        }

        protected static bool IsStandardObjectType(ObjectType objectType) {
            switch (objectType) {
                case ObjectType.DataAccessPage:
                case ObjectType.Form:
                case ObjectType.Query:
                case ObjectType.Module:
                case ObjectType.Report:
                case ObjectType.Macro:
                    return true;
                default:
                    return false;
            }
        }
    }
}
