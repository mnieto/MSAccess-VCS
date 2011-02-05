using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// List of allowed containers for an Access file format
    /// </summary>
    public abstract class Containers: IEnumerable<ContainerNames> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public Containers() {
            NamesList = new Dictionary<string, ContainerNames>();
            TypesList = new Dictionary<ObjectType, ContainerNames>();
            InitializeAllowedTypes();
        }
        
        /// <summary>
        /// Abstract method. Inherited classes must initialize the lists of allowed types
        /// </summary>
        public abstract void InitializeAllowedTypes();

        /// <summary>
        /// Find a container by name
        /// </summary>
        /// <param name="name">container name</param>
        /// <returns>Searched <see cref="ContainerNames"/> or null if thery is no container with that name</returns>
        public ContainerNames Find(string name) {
            if (NamesList.ContainsKey(name))
                return NamesList[name];
            else
                return null;
        }

        /// <summary>
        /// Find a container by <see cref="ObjectType"/>
        /// </summary>
        /// <param name="name">container name</param>
        /// <returns>Searched <see cref="ContainerNames"/> or null if thery is no container of that type</returns>
        public ContainerNames Find(ObjectType type) {
            if (TypesList.ContainsKey(type))
                return TypesList[type];
            else
                return null;
        }

        /// <summary>
        /// List of <see cref="ContainerNames"/> accessible by name
        /// </summary>
        protected Dictionary<string, ContainerNames> NamesList = null;

        /// <summary>
        /// List of <see cref="ContainerNames"/> accessible by <see cref="ObjectType"/>
        /// </summary>
        protected Dictionary<ObjectType, ContainerNames> TypesList = null;

        /// <summary>
        /// Creates a new <see cref="ContainerNames"/> and adds it to the lists
        /// </summary>
        /// <param name="name">container name</param>
        /// <param name="type"><see cref="ObjectType"/> corresponding to that container</param>
        /// <param name="fileExtension"> file extension for this object type</param>
        /// <returns>initialized <see cref="ContainerNames"/></returns>
        protected ContainerNames Add(string name, ObjectType type, FileExtensions fileExtension) {
            ContainerNames names = new ContainerNames();
            names.InvariantName = name;
            names.ObjectType = type;
            names.FileExtension = fileExtension;
            NamesList.Add(name, names);
            TypesList.Add(type, names);
            return names;
        }


        #region IEnumerable<ContainerNames> Members

        public IEnumerator<ContainerNames> GetEnumerator() {
            return NamesList.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return NamesList.Values.GetEnumerator();
        }

        #endregion
    }
}
