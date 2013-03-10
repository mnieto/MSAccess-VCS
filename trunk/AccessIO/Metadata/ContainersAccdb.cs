using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    class ContainersAccdb: Containers {

        protected override void InitializeAllowedContainers() {
            Add("Tables", ObjectType.Table, FileExtensions.tbl);
            Add("Queries", ObjectType.Query, FileExtensions.qry);
            Add("Forms", ObjectType.Form, FileExtensions.frm);
            Add("Reports", ObjectType.Report, FileExtensions.rpt);
            Add("Modules", ObjectType.Module, FileExtensions.bas);
            Add("Scripts", ObjectType.Macro, FileExtensions.mcr);

            ContainerNames names = new ContainerNames("General");
            names.ObjectTypes.Add(new ObjectTypeExtension(names, ObjectType.DatabaseDao, FileExtensions.dbp));
            names.ObjectTypes.Add(new ObjectTypeExtension(names, ObjectType.Relations, FileExtensions.rel));
            names.ObjectTypes.Add(new ObjectTypeExtension(names, ObjectType.References, FileExtensions.lib));
            Add(names);
        }
    }
}
