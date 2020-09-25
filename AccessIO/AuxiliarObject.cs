using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Base class for auxiliar objects like Index or Field that belongs to a main object
    /// </summary>
    public abstract class AuxiliarObject: ICustomObject {
        #region ICustomObject Members

        public abstract string ClassName { get; }

        public abstract object DaoObject { get; set; }

        public virtual string Name { get; set; }

        public virtual OptionsObj Options { get { return null; } }

        public virtual ObjectType ObjectType { get; set; }

        //public virtual string Path {
        //    get {
        //        return String.Concat(AccessApp.GetContainerNameFromObjectType(ObjectType),
        //                             System.IO.Path.DirectorySeparatorChar,
        //                             Name);
        //    }
        //}

        public virtual List<string> Properties {
            get {
                return EmbeddedResources.GetObjectAttributes(ClassName);
            }
        }

        public abstract void SaveProperties(ExportObject export);

        public abstract object this[string propertyName] { get; }

        #endregion

        public static ICustomObject CreateInstance(AccessApp app, object daoObject, AccessIO.ObjectType objectType) {
            string typeName = String.Concat(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, ".", objectType.ToString());
            Type t = Type.GetType(typeName);
            return Activator.CreateInstance(t, daoObject) as ICustomObject;
        }
    }
}
