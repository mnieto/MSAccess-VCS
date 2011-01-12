using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Base class for objets that do not have an implmentation for LoadFromText and SaveAsText
    /// </summary>
    public abstract class CustomObject : AccessObject, ICustomObject {

        public abstract string ClassName { get; }

        public abstract object DaoObject { get; set; }

        private List<string> properties;
        public List<string> Properties {
            get {
                if (this.properties == null) {
                    properties = EmbeddedResources.GetObjectAttributes(ClassName);
                }
                return this.properties;
            }
        }

        public abstract object this[string propertyName] { get; }

        public CustomObject(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) { }

        public abstract void SaveProperties(ExportObject export);

    }
}
