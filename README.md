# MSAccess-SVN
Export MSAccess object to text files and help to integrate with any source control management


An MS Access solution is compound by any of the following components:
* VBA code in modules and classes
* Forms: design and VBA code
* Reports: design and VBA code
* Queries: Query definition. Only for mdb files
* Macros
* Access Pages
* Tables: only for mdb file format
* Database properties
  * Database properties, including startup form options
  * Table relations: only for mdb files
  * References: References to other libraries or MS Access files
  
## Suported file formats
* mdb: Classic file format (Access 2002-2003 format onwards)
* adp: Access projects: in this files, queries and tables are stored in a sql server database, which has tools enought to export and create sql scripts.
* accdb: New file format for Access 2007 and following

## Tools
* GUI application: User friendly usage
* Command line application: for CI/CD servers and automated taks
* MS Access Add-on (on a future relase)
