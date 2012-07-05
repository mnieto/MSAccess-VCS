using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AccessIO;
using System.IO;

namespace asvn {
    internal class CommandLineImport : CommandLine {

        private Containers containers = null;

        private Dictionary<string, bool> optionParams = new Dictionary<string, bool>()  {
                { "allowdatalost", false},
                { "overwritedatabase", false}
        };

        internal CommandLineImport() {
            Objects = new List<IObjecOptions>();
        }
        
        internal override CommandLine Parse(string[] args) {
            if (args.Length < 1)
                throw new CommandLineException(Properties.Resources.InvalidNumberOfOptions);

            int currentArg = ProcessOptions(args, optionParams);

            //we know that there is, at least, one more parameters: databasefile
            CheckDatabase(args[currentArg]);
            DatabaseFile = args[currentArg];
            containers = AccessApp.ContainersFactory(DatabaseFile);

            if (++currentArg < args.Length) {
                if (args[currentArg][0] == '@')
                    ProcessResponseFile(args[currentArg].Substring(1));
                else
                    ProcessFiles(args, currentArg);
            } else
                ProcessResponseFile(Console.In);
            
            return this;
        }

        internal override CommandLine Run() {
            if (App == null && Objects.Count > 0)
                InitializeAccessApplication();
            foreach (IObjecOptions currentObject in Objects) {
                Console.Write("Loading {0}", currentObject);
                AccessObject accessObject = AccessObject.CreateInstance(App, currentObject.ObjectType, currentObject.ToString());
                accessObject.Options = currentObject.Options;
                accessObject.Load(currentObject.Name);
                Console.WriteLine(": Ok");
            }
            return this;
        }

        protected override void ProcessResponseFile(TextReader sr) {
            int i = 1;
            try {
                string line = sr.ReadLine();
                string[] args = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                while (!String.IsNullOrEmpty(line)) {
                    int currentArg = ProcessOptions(args, optionParams);
                    ProcessFiles(args, currentArg);
                    line = sr.ReadLine();
                    i++;
                }
            } catch (Exception ex) {
                throw new CommandLineException(String.Format(Properties.Resources.ErrorProcessingResponseFile, i) + ": " +  ex.Message, ex);
            }
        }

        private void ProcessFiles(string[] args, int currentArg) {
            //extra parameters, if any, are files to import
            int i = currentArg;
            while (i < args.Length) {
                //If there is only a file and has wildcards, process all the files that match the pattern
                string directory = Path.GetDirectoryName(args[i]);
                string file = Path.GetFileName(args[i]);
                if (i == args.Length - 1 && file.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    ProccessFileWithWildcards(args[i]);
                else
                    AddAccessObject(ProcessSingleFile(args[i]));
                i++;
            }
        }

        private void ProccessFileWithWildcards(string fileName) {
            string directory = Path.GetDirectoryName(fileName);
            string filePattern = Path.GetFileName(fileName);
            DirectoryInfo di = new DirectoryInfo(directory);
            IEnumerable<FileInfo> files = di.EnumerateFiles(filePattern, SearchOption.AllDirectories);
            foreach (FileInfo fi in files) {
                AddAccessObject(ProcessSingleFile(fi.FullName));
            }

        }

        private IObjecOptions ProcessSingleFile(string fileName) {
            CheckFile(fileName);
            FileExtensions fileExtension = Containers.GetFileExtension(fileName);
            IObjecOptions objectOptions = new ObjectOptions(fileName, containers.Find(fileExtension).ObjectType);
            foreach (var item in optionParams) {
                if (item.Value) {
                    try {
                        objectOptions.Options.SetProperty(item.Key);
                    } catch (ArgumentException ex) {
                        string message = String.Format(Properties.Resources.InvalidOption, item.Key);
                        throw new CommandLineException(String.Format("{0} {1}",
                                                       message,
                                                       Properties.Resources.ForThisObjectType),
                                                       ex);
                    }
                }
            }
            return objectOptions;
        }

        /// <summary>
        /// Process optional options for import command
        /// </summary>
        /// <param name="args">command line parameters</param>
        /// <param name="possibleOptions">list of possible options. Options not in this list will generate CommandLineException</param>
        /// <returns>next parameter to be processed</returns>
        private int ProcessOptions(string[] args, Dictionary<string, bool> possibleOptions) {
            //while parameters are options (at least, next parameter after options is databasefile; options begin with - or /
            int i = 0;
            while (i < args.Length - 2 && "-/".IndexOf(args[i][0], 0) != -1) {

                //Initialize options for each iteration
                List<string> keys = new List<string>(optionParams.Keys);
                foreach (string key in keys) {
                    optionParams[key] = false;
                }

                if (!Regex.IsMatch(args[i], @"[-/]\w+"))
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

        /// <summary>
        /// Check the existence and permissions
        /// </summary>
        /// <param name="fileName">Full or relative path to check</param>
        /// <exception cref="CommandLineException">If do not exists or cannot access for read</exception>
        private void CheckFile(string fileName) {
            bool isValid = true;

            if (!File.Exists(fileName)) {
                isValid = false;
            } else {
                FileStream fs = null;
                try {
                    fs = File.OpenRead(fileName);
                } catch {
                    isValid = false;
                } finally {
                    if (fs != null)
                        fs.Close();
                }
            }
            if (!isValid)
                throw new CommandLineException(String.Format(Properties.Resources.InvalidFileName, Path.GetFileName(fileName)));

        }

        private void AddAccessObject(IObjecOptions objectOptions) {

            //database objects must be the first element in the collecion
            if ((objectOptions.ObjectType == ObjectType.DatabaseDao ||
                 objectOptions.ObjectType == ObjectType.DatabasePrj) &&
                 Objects.Count > 0) {
                throw new ArgumentException(Properties.Resources.DatabaseObjectMustBeFirst);
            }
            Objects.Add(objectOptions);
        }
    }
}
