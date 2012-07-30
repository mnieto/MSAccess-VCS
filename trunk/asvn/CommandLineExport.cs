using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using AccessIO;

namespace asvn {
    internal class CommandLineExport: CommandLine {

        private CommandLine baseCommandLine;

        internal CommandLineExport(CommandLine baseCommandLine) {
            this.baseCommandLine = baseCommandLine;
            Objects = new List<IObjecOptions>();
        }

        internal override CommandLine Parse(string[] args) {
            if (args.Length < 2) {
                throw new CommandLineException(Properties.Resources.InvalidNumberOfOptions);
            }

            CheckDatabase(args[0]);
            DatabaseFile = args[0];

            CheckRootPath(args[1]);
            RootPath = args[1];

            if (args.Length >= 3) {
                //3rd argument can be an Access object or a script file
                if (args[2][0] == '@') {
                    if (args.Length > 3)
                        throw new CommandLineException(Properties.Resources.InvalidNumberOfOptions);
                    ProcessResponseFile(args[2].Substring(1));
                } else {
                    if (args.Length == 3 && args[2].IndexOf('*') >= 0)
                        ProcessObjectsWithWildcards(args[2]);
                    else {
                        for (int i = 2; i < args.Length; i++) {
                            ProcessAccessObject(args[i]);
                        }
                    }
                }
            } else {
                //3rd argument is stdin
                Console.WriteLine(Properties.Resources.TypeObjectName);
                ProcessResponseFile(Console.In);
            }
            return this;
        }

        internal override CommandLine Run() {
            if (App == null && Objects.Count > 0)
                InitializeAccessApplication();
            foreach (IObjecOptions currentObject in Objects) {
                ObjectTypeExtension ote = App.AllowedContainers.Find(currentObject.ObjectType);

                Console.Write(Properties.Resources.Exporting, ote.FileExtension, currentObject);
                AccessObject accessObject = AccessObject.CreateInstance(App, currentObject.ObjectType, currentObject.ToString());
                accessObject.Options = currentObject.Options;
                string outputFile = Path.Combine(RootPath, ote.Container.InvariantName, currentObject.ToString()) + 
                                    String.Concat(".", ote.FileExtension, ".txt");
                accessObject.Save(outputFile);
                Console.WriteLine(Properties.Resources.ColonOk);
            }
            App.Dispose();
            return this;
        }

        /// <summary>
        /// Find and process matched objects
        /// </summary>
        /// <param name="namePattern"></param>
        /// <remarks>
        /// patterns have 2 parts: xxx.nnn, where xxx is the object type (equivalent to file extension)
        /// and nnn is the name.
        /// <list type="bullet">
        /// <item>
        /// <description><c>object type</c>: can be * or a supported file extension for the Access file format</description>
        /// <description><c>name</c>: the name of an object or name with one or more asterix</description>
        /// </item>
        /// </list>
        /// </remarks>
        private void ProcessObjectsWithWildcards(string namePattern) {
            Regex regex = new Regex(@"(\*|\w{3})\.(.+)", RegexOptions.IgnoreCase);
            Match match = regex.Match(namePattern);
            if (match == null) {
                throw new CommandLineException(Properties.Resources.InvalidObjectName);
            }

            InitializeAccessApplication();

            string pattern = match.Groups[2].Value;
            if (pattern == "*")
                pattern = "^" + pattern.Replace("*", ".*") + "$";

            if (match.Groups[1].Value == "*") {
                foreach (ContainerNames container in App.AllowedContainers) {
                    Objects.AddRange(App.LoadObjectNames(container.InvariantName).
                                            Where(x => Regex.IsMatch(x.Name, pattern, RegexOptions.IgnoreCase)));
                }
            } else {
                FileExtensions fileExtension;
                Enum.TryParse<FileExtensions>(match.Groups[1].Value, true, out fileExtension);
                string containerInvariantName = App.AllowedContainers.Find(fileExtension).Container.InvariantName;
                Objects.AddRange(App.LoadObjectNames(containerInvariantName).
                                            Where(x => Regex.IsMatch(x.Name, pattern, RegexOptions.IgnoreCase)));
            }
            
        }

        private void ProcessAccessObject(string accessObjectName) {
            Regex regex = new Regex(@"([a-z]{3})\.(.+)", RegexOptions.IgnoreCase);
            Match match = regex.Match(accessObjectName);
            if (!match.Success)
                throw new CommandLineException(Properties.Resources.InvalidObjectName);

            AccessIO.FileExtensions fileExtension;
            if (!Enum.TryParse<AccessIO.FileExtensions>(match.Groups[1].Value, out fileExtension))
                throw new CommandLineException(Properties.Resources.InvalidObjectName);

            Containers containers = AccessApp.ContainersFactory(DatabaseFile);
            Objects.Add(new AccessIO.ObjectOptions(match.Groups[2].Value, containers.Find(fileExtension).ObjectType));

        }

        protected override void ProcessResponseFile(TextReader sr) {
            int i = 1;
            try {
                string line = sr.ReadLine();
                while (!String.IsNullOrEmpty(line)) {
                    ProcessAccessObject(line);
                    line = sr.ReadLine();
                    i++;
                }
            } catch (Exception ex) {
                throw new CommandLineException(String.Format(Properties.Resources.ErrorProcessingResponseFile, i, ex.Message), ex);
            }
        }

        /// <summary>
        /// Check existence for the path
        /// </summary>
        /// <param name="rootPath">absolute or relative path w/out wildcards</param>
        private void CheckRootPath(string rootPath) {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
        }

    }
}
