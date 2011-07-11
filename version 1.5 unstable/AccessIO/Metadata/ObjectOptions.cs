using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    /// <summary>
    /// Interface necesary to Access objects be enumerated by <see cref="AccessApp"/>
    /// </summary>
    public class ObjectOptions : AccessIO.IObjecOptions {

        /// <summary>
        /// Full name of the file containing the object definition
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Object type
        /// </summary>
        public ObjectType ObjectType { get; set; }


        /// <summary>
        /// Options for this object
        /// </summary>
        public OptionsObj Options { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Full name of the file containing the object definition</param>
        /// <param name="objectType">object type clasification</param>
        public ObjectOptions(string name, ObjectType objectType) {
            Name = name;
            ObjectType = objectType;
            Options = OptionsObj.OptionsObjFactory(name, objectType);
        }

        /// <summary>
        /// Returns the object's name. If Name correspond to a file name, ToString() returns the file name without extension
        /// </summary>
        public override string ToString() {
            if (Name.LastIndexOf('.') == -1)
                return Name;
            else {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Name);
                int pos = fileName.IndexOf('.');
                if (pos == -1)
                    pos = fileName.Length;
                return fileName.Substring(0, pos);
            }
        }
    }
}
