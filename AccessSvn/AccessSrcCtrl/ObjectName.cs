using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Interface necesary to Access objects be enumerated by <see cref="AccessApp"/>
    /// </summary>
    public class ObjectName : AccessIO.IObjectName {

        /// <summary>
        /// Name of the object
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Container name\Object name
        /// </summary>
        public string Path {
            get {
                return String.Concat(AccessApp.GetContainerNameFromObjectType(ObjectType),
                                     System.IO.Path.DirectorySeparatorChar,
                                     Name);
            }
        }
        
        /// <summary>
        /// Object type
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Constructur
        /// </summary>
        /// <param name="name">name of the object</param>
        /// <param name="objectType">object type clasification</param>
        public ObjectName(string name, ObjectType objectType) {
            Name = name;
            ObjectType = objectType;
        }
    }
}
