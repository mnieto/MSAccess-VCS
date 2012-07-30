using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace AccessIO {

    /// <summary>
    /// Helper class to load custom objects from text files to Access
    /// </summary>
    public class ImportObject {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sw">Stream where write to</param>
        public ImportObject(StreamReader sr) {
            TabSize = 4;
            this.sr = sr;
            this.theBegin = Properties.Resources.Begin;
            this.theEnd = Properties.Resources.End;
            CurrentLine = String.Empty;
            LineNumber = 0;
        }

        /// <summary>
        /// Tab size for indentation. Tabs are filled with spaces (Default 4)
        /// </summary>
        public static int TabSize { get; set; }

        /// <summary>
        /// Current identation level. Initial value is 0
        /// </summary>
        /// <remarks>
        /// Each WriteBegin increment the identation level and each WriteEnd decrement it
        /// </remarks>
        public int Indent { get; private set; }

        /// <summary>
        /// Get the last line readed from the file
        /// </summary>
        /// <remarks>
        /// If there is not readed line returns <c>String.Empty</c>
        /// If reads reach the end of file, returns the <c>End</c> string
        /// </remarks>
        public string CurrentLine { get; protected set; }

        /// <summary>
        /// Gets the line number of the current line
        /// </summary>
        public int LineNumber { get; protected set; }

        /// <summary>
        /// Read a new line from the file and returns it. Updates the <see cref="CurrentLine"/> property
        /// </summary>
        /// <returns>line of text readed from the file or, if EOF, <c>End</c></returns>
        public string ReadLine() {
            string line = null;
            if (sr.EndOfStream)
                line = theEnd;
            else {
                line = sr.ReadLine();
                if (line == null)
                    line = theEnd;
                else {
                    Regex regex = new Regex(" *");
                    Indent = regex.Match(line).Value.Length / TabSize;
                    line = line.Trim();
                    LineNumber++;
                }
            }
            CurrentLine = line;
            return CurrentLine;
        }

        /// <summary>
        /// Read <paramref name="linesCount"/> lines from the file.
        /// </summary>
        /// <param name="linesCount">Number of lines to count</param>
        /// <returns>Returns the contents of the last readed line</returns>
        public string ReadLine(int linesCount) {
            string line = null;
            for (int i = 0; i < linesCount; i++) {
                line = ReadLine();
            }
            return line;
        }

        /// <summary>
        /// Read a line of type: Begin &lt;ObjectType&gt; &lt;ObjectName&gt;
        /// </summary>
        /// <returns>Returns the ObjectName part</returns>
        /// <exception cref="WrongFileFormatException"/> if ObjectName is not found
        public string ReadObjectName() { 
            ReadLine();
            return PeekObjectName();
        }

        public string PropertyName() {
            //property name (including -_ and space), the first ocurrence of ": ", the rest of the line
            //the second group do not match
            Regex regex = new Regex(@" *([\w _-]+)(: ?)");
            Match match = regex.Match(CurrentLine);
            if (!match.Success)
                throw new AccessIOException(Properties.Resources.PropertyNameError);
            return match.Groups[1].Value.Trim();
        }

        public string PropertyValue() {
            //property name (including -_ and space), the first ocurrence of ": ", the rest of the line
            //the second group do not match
            Regex regex = new Regex(@"(?:: *)+?(.*)$");
            Match match = regex.Match(CurrentLine);
            if (!match.Success)
                throw new AccessIOException(Properties.Resources.PropertyValueError);
            if (match.Groups[1].Value == String.Empty)
                return null;
            else
                return match.Groups[1].Value.Trim();
        }

        /// <summary>
        /// Analize, but don't read, a line of type: Begin ObjectType ObjectName
        /// </summary>
        /// <returns>Returns the ObjectName part</returns>
        /// <exception cref="WrongFileFormatException">if ObjectName is not found</exception> 
        public string PeekObjectName() {
            int pos = CurrentLine.IndexOf(' ', Math.Min(CurrentLine.Length, theBegin.Length + 1));
            if (pos == -1)
                throw new WrongFileFormatException(String.Format(Properties.ImportRes.ObjectNameNotFound, LineNumber),
                                                   (sr.BaseStream as FileStream).Name,
                                                   LineNumber);
            return CurrentLine.Substring(pos + 1);        
        }

        public bool IsBegin {
            get { return CurrentLine.Trim().StartsWith(theBegin);  }
        }
        public bool IsEnd {
            get { return CurrentLine == theEnd; }
        }

        private StreamReader sr;
        private string theEnd;
        private string theBegin;


        /// <summary>
        /// Read all the properties of an object, until the first End is found
        /// </summary>
        /// <returns>A dictionary with the property names and theirs values</returns>
        internal Dictionary<string, object> ReadProperties() {
            Dictionary<string, object> result = new Dictionary<string, object>();
            ReadLine();
            while (!IsBegin && !IsEnd) {
                result.Add(PropertyName(), PropertyValue());
                ReadLine();
            }
            return result;
        }
    }
}
