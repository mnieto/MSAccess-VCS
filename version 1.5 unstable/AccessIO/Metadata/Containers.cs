using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            //TypesList = new Dictionary<ObjectType, ContainerNames>();
            //ExtensionsList = new Dictionary<FileExtensions, ContainerNames>();
            InitializeAllowedContainers();
        }
        
        /// <summary>
        /// Abstract method. Inherited classes must initialize the lists of allowed containers
        /// </summary>
        protected abstract void InitializeAllowedContainers();

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
        public ObjectTypeExtension Find(ObjectType type) {
            foreach (ContainerNames item in NamesList.Values) {
                ObjectTypeExtension result = item.Find(type);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Find a container by <see cref="ObjectType"/>
        /// </summary>
        /// <param name="name">file extension</param>
        /// <returns>Searched <see cref="ContainerNames"/> or null if thery is no container of that extension</returns>
        public ObjectTypeExtension Find(FileExtensions fileExtension) {
            foreach (ContainerNames item in NamesList.Values) {
                ObjectTypeExtension result = item.Find(fileExtension);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the corresponding <see cref="FileExtensions"/> extension from a file name
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns>A FileExtensions enum value</returns>
        /// <exception cref="ArgumentException">If file name extension do not correspond with any FileExtensions value</exception> 
        /// <remarks>
        ///  file name is expected in the format name.ext.txt, where ext correspond with a FileExtensions value
        /// </remarks>
        public static FileExtensions GetFileExtension(string fileName) {
            string ext = Path.GetExtension(Path.GetFileNameWithoutExtension(fileName)).Substring(1);
            FileExtensions fileExtension;
            if (Enum.TryParse<FileExtensions>(ext, true, out fileExtension))
                return fileExtension;
            else
                throw new ArgumentException(Properties.Resources.NotAllowedObjectTypeException);
        }


        /// <summary>
        /// Gets the corresponding object's name from a file name
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <remarks>
        ///  file name is expected in the format name.ext.txt, where ext correspond with a FileExtensions value
        /// </remarks>
        public static string GetObjectName(string fileName) {
            return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
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
        /// List of <see cref="ContainerNames"/> accesible by <see cref="FileExtensions"/>
        /// </summary>
        protected Dictionary<FileExtensions, ContainerNames> ExtensionsList = null;

        /// <summary>
        /// Creates a new <see cref="ContainerNames"/> and adds it to the lists
        /// </summary>
        /// <param name="containerName">container name</param>
        /// <param name="type"><see cref="ObjectType"/> corresponding to that container</param>
        /// <param name="fileExtension"> file extension for this object type</param>
        /// <returns>initialized <see cref="ContainerNames"/></returns>
        protected ContainerNames Add(string containerName, ObjectType objectType, FileExtensions fileExtension) {
            ContainerNames names = new ContainerNames(containerName);
            names.ObjectTypes.Add(new ObjectTypeExtension(names, objectType, fileExtension));
            NamesList.Add(containerName, names);
            return names;
        }

        /// <summary>
        /// Adds a ContainerNames to the lists
        /// </summary>
        /// <param name="container">container</param>
        /// <param name="type"><see cref="ObjectType"/> corresponding to that container</param>
        /// <param name="fileExtension"> file extension for this object type</param>
        /// <returns>initialized <see cref="ContainerNames"/></returns>
        protected ContainerNames Add(ContainerNames container) {
            NamesList.Add(container.InvariantName, container);
            return container;
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
