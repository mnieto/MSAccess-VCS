using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace asvnTest {
    public class TestHelper {

        public TestContext TestContext { get; private set; }

        public string ProcessOutput { get; private set; }

        public string CommandLine {
            get {
                string[] pathParts = TestContext.TestRunDirectory.Split(Path.DirectorySeparatorChar);
                return String.Join(Path.DirectorySeparatorChar.ToString(), pathParts, 0, pathParts.Length - 2) + @"\asvn\bin\Release\asvn.exe";
            }
        }

        public string DatabasePath {
            get {
                string[] pathParts = TestContext.TestRunDirectory.Split(Path.DirectorySeparatorChar);
                return String.Join(Path.DirectorySeparatorChar.ToString(), pathParts, 0, pathParts.Length - 3) + @"\TestAccessProjects\asvnTest2003.mdb";            
            }
        }

        public TestHelper(TestContext testContext) {
            TestContext = testContext;
        }

        public int StartProcess(string commandLine, string arguments) {
            ProcessStartInfo si = new ProcessStartInfo(commandLine, arguments);
            si.RedirectStandardOutput = true;
            si.UseShellExecute = false;
            Process process = Process.Start(si);
            process.WaitForExit();
            ProcessOutput = process.StandardOutput.ReadToEnd();
            return process.ExitCode;
        }


    }
}
