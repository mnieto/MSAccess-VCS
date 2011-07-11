using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessIO {

    /// <summary>
    /// Establishes a relationship between object type and corresponding file extensión
    /// </summary>
    public class ObjectTypeExtension {
        public ObjectType ObjectType {get; set; }
        public FileExtensions FileExtension { get; set; }
        public ContainerNames Container { get; private set; }

        public ObjectTypeExtension(ContainerNames container, ObjectType objectType, FileExtensions fileExtension) {
            Container = container;
            ObjectType = objectType;
            FileExtension = fileExtension;
        }
    }
}
