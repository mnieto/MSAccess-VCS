using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessIO;

namespace AccessScrCtrlUI {

    /// <summary>
    /// Contains information about the completion of the task
    /// </summary>
    public class SelectedObjectsCompletedEventArgs : EventArgs {

        //Constructor
        public SelectedObjectsCompletedEventArgs(Exception error, int totalObjectsSaved)
            : base() {
            TotalOjectsSaved = totalObjectsSaved;
            Error = error;
        }

        /// <summary>
        /// Gets the total number of saved objects
        /// </summary>
        public int TotalOjectsSaved { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; private set; }
    }

    /// <summary>
    /// Contains information about the progress and current object
    /// </summary>
    public class SelectedObjectsProgressEventArgs : EventArgs {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="progressPercentaje">asyncronous progress percentaje</param>
        /// <param name="objectName">current Access Object to be processes</param>
        public SelectedObjectsProgressEventArgs(int progressPercentaje, IObjecOptions objectName)
            : base() {
            ProgressPercentaje = progressPercentaje;
            ObjectName = objectName;
        }

        /// <summary>
        /// Gets the asyncronous progress percentaje
        /// </summary>
        public int ProgressPercentaje { get; private set; }

        /// <summary>
        /// gets the Current Access Object to be processes
        /// </summary>
        public IObjecOptions ObjectName { get; private set; }
    }
}
