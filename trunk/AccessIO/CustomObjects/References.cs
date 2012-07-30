using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Access = Microsoft.Office.Interop.Access;

namespace AccessIO {
    /// <summary>
    /// References wrapper for <see cref="dao.References"/> object
    /// </summary>
    public class References : CustomObject {

        private Access.References references;

        public References(AccessApp app, string name, ObjectType objectType) : base(app, name, objectType) { }

        /// <summary>
        /// Save to <paramref name="fileName"/> the list of references
        /// </summary>
        /// <param name="fileName">output file name</param>
        /// <remarks>Do not write out the builtin references neither broken references. If you need to export a broken reference, first fix it</remarks>
        public override void Save(string fileName) {
            //Make sure the path exists
            MakePath(System.IO.Path.GetDirectoryName(fileName));

            using (StreamWriter sw = new StreamWriter(fileName)) {
                ExportObject export = new ExportObject(sw);
                
                export.WriteBegin(ClassName);
                foreach (Access.Reference reference in App.Application.References) {
                    if (!reference.BuiltIn && !reference.IsBroken) {
                        export.WriteBegin("Reference", reference.Name);
                        export.WriteProperty("FullPath", reference.FullPath);
                        if (reference.Kind == Microsoft.Vbe.Interop.vbext_RefKind.vbext_rk_TypeLib) {
                            export.WriteProperty("Guid", reference.Guid);
                            export.WriteProperty("Major", reference.Major);
                            export.WriteProperty("Minor", reference.Minor);
                            export.WriteEnd();
                        }
                    }
                }
                export.WriteEnd();
            }
        }

        /// <summary>
        /// Ovewrite the current references with the references from <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">Name of the file with the references to load into the database</param>
        public override void Load(string fileName) {
            using (StreamReader sr = new StreamReader(fileName)) {
                ImportObject import = new ImportObject(sr);

                //Remove any previous reference
                Access.Reference[] referencesList = new Access.Reference[App.Application.References.Count];
                int i = 0;
                foreach (Access.Reference reference in App.Application.References) {
                    if (!reference.BuiltIn && !reference.IsBroken)
                        referencesList[i++] = reference;
                }
                for (int j = 0; j < i; j++) {
                    App.Application.References.Remove(referencesList[j]);
                }

                //Add new references from file
                //if reference is dll or typelib, it will have guid and version
                //if reference is an access file (mde,mdb,mda,adp,...) we need add the reference by filename
                import.ReadLine();
                import.ReadLine();  //Read the Begin Reference (or End References if there is no references)
                while (!import.IsEnd) {
                    import.ReadLine();  //Read FullPath
                    string fullPath = import.PropertyValue();
                    import.ReadLine();  //Read Gui or End Reference
                    if (import.IsEnd) {
                        fullPath = ResolveFileName(fullPath);
                        App.Application.References.AddFromFile(fullPath);
                    }  else {
                        string guid = import.PropertyValue();
                        import.ReadLine();  //Read Major
                        int major = Convert.ToInt32(import.PropertyValue());
                        import.ReadLine();  //Read Minor
                        int minor = Convert.ToInt32(import.PropertyValue());
                        import.ReadLine();  //Read End Reference
                        App.Application.References.AddFromGuid(guid, major, minor);
                        import.ReadLine();  //Read the next Begin Reference or End References
                    }
                }
            }
        }

        /// <summary>
        /// Try to resolve the location of the filename
        /// </summary>
        /// <param name="fullPath">path of the file to find</param>
        /// <returns>
        /// if exists <paramref name="fullPath"/>, returns fullPath, else, 
        /// it will find it in the same folder than the access file
        /// If not found, return fullPath
        /// </returns>
        private string ResolveFileName(string fullPath) {
            if (System.IO.File.Exists(fullPath))
                return fullPath;
            else {
                string fileName = System.IO.Path.GetFileName(fullPath);
                string folder = System.IO.Path.GetDirectoryName(App.FileName);
                string newFullPath = System.IO.Path.Combine(folder, fileName);
                if (System.IO.File.Exists(newFullPath))
                    return newFullPath;
                else
                    return fullPath;
            }
        }

        public override object this[string propertyName] {
            get { return App.Application.References.Item(propertyName); }
        }

        public override string ClassName {
            get { return "References"; }
        }

        public override object DaoObject {
            get {
                return this.references;
            }
            set {
                if (value != null && !(value is Access.References))
                    throw new ArgumentException(String.Format(AccessIO.Properties.Resources.DaoObjectIsNotAValidType, typeof(Access.References).Name));
                this.references = (Access.References)value;
            }
        }

    }
}
