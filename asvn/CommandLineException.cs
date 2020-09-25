using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asvn {

    [Serializable]
    public class CommandLineException : Exception {
        public CommandLineException() { }
        public CommandLineException(string message) : base(message) { }
        public CommandLineException(string message, Exception inner) : base(message, inner) { }
        protected CommandLineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
