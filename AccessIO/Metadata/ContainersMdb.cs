using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    class ContainersMdb: Containers {

        protected override void InitializeAllowedContainers() {
            Add("Tables", ObjectType.Table, FileExtensions.tbl);
            Add("Queries", ObjectType.Query, FileExtensions.qry);
            Add("Forms", ObjectType.Form, FileExtensions.frm);
            Add("Reports", ObjectType.Report, FileExtensions.rpt);
            Add("DataAccessPages", ObjectType.DataAccessPage, FileExtensions.dap);    //Partially supported because SaveAsText export it to binary format and this object is deprecatted begining with Office 2007
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
