using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// Possible names for a container
    /// </summary>
    public class ContainerNames {

        /// <summary>
        /// Gets or sets the languaje independent name, used for serialization and deserialization
        /// </summary>
        public string InvariantName { get; set; }

        /// <summary>
        /// Gets the display name for the container, languaje dependent
        /// </summary>
        public string DisplayName {
            get {
                return Properties.Names.ResourceManager.GetString(InvariantName);
            }
        }

        /// <summary>
        /// Gets the display name for the plural of the container, languaje dependent
        /// </summary>
        public string DisplayPluralName {
            get {
                return Properties.Names.ResourceManager.GetString(String.Concat(InvariantName,"Plural"));
            }
        }

        /// <summary>
        /// Gets or sets the object type of the objects that contains the container
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Gets or set the file extension for this object type
        /// </summary>
        public FileExtensions FileExtension { get; set; }
    }
}
