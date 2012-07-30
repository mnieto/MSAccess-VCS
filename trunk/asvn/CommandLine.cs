using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using AccessIO;

namespace asvn {


    internal class CommandLine: IDisposable {

        /// <summary>Show usage help</summary>
        public bool ShowHelp { get; private set; }

        /// <summary>Import command</summary>
        public bool Import { get; private set; }

        /// <summary>Export command</summary>
        public bool Export { get; private set; }

        /// <summary>Verbose output to stdout</summary>
        public bool Verbose { get; private set; }

        /// <summary>Wait a keystroke to terminate the program</summary>
        public bool WaitAtEnd { get; private set; }

        /// <summary>Database file name</summary>
        public string DatabaseFile { get; protected set; }

        /// <summary>AccessIO.AccessApp application</summary>
        public AccessApp App { get; protected set; }

        /// <summary>List of objects to be imported or exported</summary>
        public List<AccessIO.IObjecOptions> Objects { get; protected set; }

        protected string RootPath { get; set; }
        protected CommandLine Arguments { get; set; }

        /// <summary>List of allowed optional parameters</summary>
        private Dictionary<string, bool> optionParams = new Dictionary<string, bool>()  {
                { "v", false},
                { "w", false},
                { "h", false},
                { "?", false}
        };

        internal virtual CommandLine Parse(string[] args) {

            if (args.Length == 0) {
                throw new CommandLineException(Properties.Resources.ExpectedValidCommand);
            }

            //process general options
            int currentArg = ProcessOptions(args, optionParams);
            ShowHelp = (optionParams["?"] || optionParams["h"]);
            Verbose = optionParams["v"];
            WaitAtEnd = optionParams["w"];

            //process the main command
            const string subcommands = @"h|(help)|i|(import)|e|(export)";
            Regex regex = new Regex(subcommands, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(args[currentArg++]);

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
                Arguments = new CommandLineImport(this);
            } else if (Export) {
                Arguments = new CommandLineExport(this);
            }
            Arguments.Parse(args.Skip(currentArg).ToArray());
            //Objects = Arguments.Objects;
            //DatabaseFile = Arguments.DatabaseFile;
            //App = Arguments.App;

            return this;
        }

        protected void InitializeAccessApplication() {
            Console.WriteLine(Properties.Resources.Conecting);
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

        /// <summary>
        /// Process optional options
        /// </summary>
        /// <param name="args">command line parameters</param>
        /// <param name="possibleOptions">list of possible options. Options not in this list will generate CommandLineException</param>
        /// <returns>next parameter to be processed</returns>
        protected int ProcessOptions(string[] args, Dictionary<string, bool> optionParams) {
            //while parameters are options: options begin with - or / and will be a parameter (no option) that begins with letter
            int i = 0;
            while (i < args.Length - 2 && "-/".IndexOf(args[i][0], 0) != -1) {

                //Initialize options for each iteration
                List<string> keys = new List<string>(optionParams.Keys);
                foreach (string key in keys) {
                    optionParams[key] = false;
                }

                if (!Regex.IsMatch(args[i], @"[-/]\w+", RegexOptions.IgnoreCase))
                    throw new CommandLineException(String.Format(Properties.Resources.InvalidOption, args[i]));
                string option = args[i].Substring(1).ToLower();
                if (optionParams.ContainsKey(option))
                    optionParams[option] = true;
                else
                    throw new CommandLineException(String.Format(Properties.Resources.InvalidOption, args[i]));
                i++;
            }
            return i;
        }

        public void ShowUsage() {
            Console.Write(Properties.Resources.Usage);
        }


        internal virtual CommandLine Run() {
            if (Arguments != null)
                Arguments.Run();
            return this;
        }



        #region IDisposable Members

        private bool disposed = false;

        protected void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    if (App != null) {
                        App.Dispose();
                    }
                }
                App = null;
                disposed = false;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
