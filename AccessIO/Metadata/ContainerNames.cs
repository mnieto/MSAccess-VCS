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
        /// Default file extension for this container. Useful for containers with only one object type
        /// </summary>
        /// <remarks>
        /// For contaniers with multiple object types use the <see cref="Find"/> method
        /// </remarks>
        public FileExtensions DefaultExtension {
            get {
                return ObjectTypes[0].FileExtension;
            }
        }

        /// <summary>
        /// Default object type for this container. Useful for containers with only one object type
        /// </summary>
        /// <remarks>
        /// For contaniers with multiple object types use the <see cref="Find"/> method
        /// </remarks>
        public ObjectType DefaultObjectType {
            get {
                return ObjectTypes[0].ObjectType;
            }
        }

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

        public List<ObjectTypeExtension> ObjectTypes { get; private set; }

        /// <summary>
        /// Gets or set the file extension for this object type
        /// </summary>
        public FileExtensions FileExtension { get; set; }


        /// <summary>
        /// Default contructor
        /// </summary>
        public ContainerNames() {
            ObjectTypes = new List<ObjectTypeExtension>();
        }

        /// <summary>
        /// Create a new instance of ContainerNames and assign its name
        /// </summary>
        /// <param name="invariantName"></param>
        public ContainerNames(string invariantName) : this() {
            InvariantName = invariantName;
        }

        /// <summary>
        /// Find the <paramref name="objectType"/> in the supported object types for this container
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public ObjectTypeExtension Find(ObjectType objectType) {
            return ObjectTypes.Find(x => x.ObjectType == objectType);
        }

        /// <summary>
        /// Find by <paramref name="extension"/> in the supported object types for this container
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public ObjectTypeExtension Find(FileExtensions extension) {
            return ObjectTypes.Find(x => x.FileExtension == extension);
        }

    }
}
