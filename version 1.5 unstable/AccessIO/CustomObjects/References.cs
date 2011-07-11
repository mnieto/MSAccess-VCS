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
                        export.WriteProperty("Guid", reference.Guid);
                        export.WriteEnd();
                    }
                }
                export.WriteEnd();
            }
        }

        public override void Load(string fileName) {
            throw new NotImplementedException();
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
