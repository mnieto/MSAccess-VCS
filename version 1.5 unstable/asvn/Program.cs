using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asvn {
    class Program {
        static void Main(string[] args) {

            CommandLine cmd = new CommandLine();
            int phase = 1;
            try {
                cmd.Parse(args);
                phase = 2;
                cmd.Run();
            } catch (Exception ex) {
                Console.WriteLine(CommandExecuted(args));
                Console.WriteLine(Properties.Resources.AsvnError);
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(Properties.Resources.AshkForHelp);
                Environment.ExitCode = phase;
            }

        }

        static string CommandExecuted(string[] args) {
            return "asvn " + String.Join(" ", args);
        }
    }
}
