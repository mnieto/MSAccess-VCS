using System;
using System.Collections.Generic;
using System.Text;

namespace AccessIO {
    class ContainersMdb: Containers {

        public override void InitializeAllowedTypes() {
            Add("Tables", ObjectType.Table, FileExtensions.tbl);
            Add("Queries", ObjectType.Query, FileExtensions.qry);
            Add("Forms", ObjectType.Form, FileExtensions.frm);
            Add("Reports", ObjectType.Report, FileExtensions.rpt);
            Add("DataAccessPages", ObjectType.DataAccessPage, FileExtensions.dap);    //Partially supported because SaveAsText export it to binary format and this object is deprecatted begining with Office 2007
            Add("Modules", ObjectType.Module, FileExtensions.bas);
            Add("Scripts", ObjectType.Macro, FileExtensions.mcr);
            Add("Default", ObjectType.Default, FileExtensions.dbp);
        }
    }
}
