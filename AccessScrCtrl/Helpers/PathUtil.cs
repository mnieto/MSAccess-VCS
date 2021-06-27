using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AccessScrCtrl.Helpers {
    static class PathUtil {

        [DllImport("shlwapi.dll", EntryPoint = "PathRelativePathTo")]
        static extern bool PathRelativePathTo(StringBuilder lpszDst, string from, UInt32 attrFrom, string to, UInt32 attrTo);

        public static string GetRelativePath(string from, string to) {
            //on net standard 2.1 and net core 2.0 is available Path.GetRelativePath
            StringBuilder builder = new StringBuilder(1024);
            bool result = PathRelativePathTo(builder, from, 0, to, 0);
            return builder.ToString();
        }

        public static string GetFullPath(string path, string basePath) {
            //on net standard 2.1 and net core 2.1 is available Path.GetFullPath(string, string)
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (Path.IsPathRooted(path))
                return path;
            if (basePath == null)
                throw new ArgumentNullException(nameof(basePath));
            if (!Path.IsPathRooted(basePath))
                throw new ArgumentException($"{nameof(basePath)} must be an absolute path");

            if (!string.IsNullOrEmpty(Path.GetExtension(basePath)))
                basePath = Path.GetDirectoryName(basePath);
            if (basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                basePath = basePath.Substring(0, basePath.Length - 1);

            //This tries real access to the file/folder. If the file doesn't exist yet, will raise an exception
            //return Path.GetFullPath(Path.Combine(basePath, path));

            string relativePath = null;
            string[] tokens = path.Split(Path.DirectorySeparatorChar);
            for (int i = 0; i < tokens.Length; i++) { 
                if (tokens[i] == "..") {
                    basePath = Path.GetDirectoryName(basePath);
                    if (string.IsNullOrEmpty(basePath))
                        throw new ArgumentException($"{nameof(path)} is not relative to {nameof(basePath)}");
                } else if (tokens[i] != ".") {
                    relativePath = string.Join(Path.DirectorySeparatorChar.ToString(), tokens.Skip(i));
                    break;
                }
            }
            return string.Concat(basePath, Path.DirectorySeparatorChar, relativePath);
        }
    }
}
