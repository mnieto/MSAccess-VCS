using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using AccessIO;

namespace asvn {


    internal class CommandLine {

        public bool ShowHelp { get; private set; }
        public bool Import { get; private set; }
        public bool Export { get; private set; }

        public string DatabaseFile { get; protected set; }
        public AccessApp App { get; protected set; }
        public List<AccessIO.IObjecOptions> Objects { get; protected set; }

        protected string RootPath { get; set; }
        protected CommandLine Arguments { get; set; }

        internal virtual CommandLine Parse(string[] args) {

            if (args.Length == 0) {
                throw new CommandLineException(Properties.Resources.ExpectedValidCommand);
            }

            //process the main command
            const string subcommands = @"([-/]h)|([-/]\?)|([-/]*help)|i|(import)|e|(export)";
            Regex regex = new Regex(subcommands, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(args[0]);

            if (matches.Count != 1) {
                throw new CommandLineException(Properties.Resources.ExpectedValidCommand);
            }

            char c = matches[0].Value[0];
            if (c == '-' || c == '/')
                c = matches[0].Value[1];
            c = Char.ToLower(c); 

            switch (c) { 
                case 'h':
                case '?':
                    ShowHelp = true;
                    break;
                case 'i':
                    Import = true;
                    break;
                case 'e':
                    Export = true;
                    break;
            }

            if (ShowHelp) {
                ShowUsage();
                return this;
            }


            if (Import) {
                Arguments = new CommandLineImport();
            } else if (Export) {
                Arguments = new CommandLineExport();
            }
            Arguments.Parse(args.Skip(1).ToArray());
            //Objects = Arguments.Objects;
            //DatabaseFile = Arguments.DatabaseFile;
            //App = Arguments.App;

            return this;
        }

        protected void InitializeAccessApplication() {
            Console.WriteLine("Conecting");
            App = AccessApp.AccessAppFactory(DatabaseFile);
            App.OpenDatabase();
        }

        protected void ProcessResponseFile(string responseFile) {
            ProcessResponseFile(new StreamReader(responseFile));
        }

        protected virtual void ProcessResponseFile(TextReader sr) { }
        

        /// <summary>
        /// Check existence and access rights for the file
        /// </summary>
        /// <param name="databaseFile"></param>
        protected void CheckDatabase(string databaseFile) {
            
            try {
                FileStream s = File.Open(databaseFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Close();
            } catch (ArgumentException ex) {
                throw new CommandLineException(Properties.Resources.InvalidPathForDatabase, ex);
            } catch (PathTooLongException ex) {
                throw new CommandLineException(Properties.Resources.InvalidPathForDatabase, ex);
            } catch (DirectoryNotFoundException ex) {
                throw new CommandLineException(Properties.Resources.InvalidPathForDatabase, ex);
            } catch (NotSupportedException ex) {
                throw new CommandLineException(Properties.Resources.InvalidPathForDatabase, ex);
            } catch (UnauthorizedAccessException ex) {
                throw new CommandLineException(Properties.Resources.AccessDenied, ex);
            } catch (FileNotFoundException ex) {
                throw new CommandLineException(Properties.Resources.InvalidPathForDatabase, ex);
            }
        }

        public void ShowUsage() {
            Console.Write(Properties.Resources.Usage);
        }


        internal virtual CommandLine Run() {
            if (Arguments != null)
                Arguments.Run();
            return this;
        }
    }
}
