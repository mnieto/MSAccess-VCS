using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccessIO;

namespace AccessScrCtrlUI {
    internal class TreeImages {

        /// <summary>
        /// Gets the underlying <see cref="System.Windows.Forms.ImageList">ImageList</see>
        /// </summary>
        internal ImageList ImageList { get; private set; }

        /// <summary>
        /// Constructor. Initialize the image list from the application resources based on the containers
        /// </summary>
        /// <param name="containers">Containers collection of containerNames</param>
        internal TreeImages(Containers containers) {

            ImageList = new ImageList();

            //Load default image for Load and Save trees
            ImageList.Images.Add("db", Properties.Resources.db);
            ImageList.Images.Add("folder", Properties.Resources.folder);

            foreach (ContainerNames container in containers) {
                foreach (ObjectTypeExtension ote in container.ObjectTypes) {
                    string extension = ote.FileExtension.ToString();
                    ImageList.Images.Add(extension, LoadFromResources(extension));
                }
            }
        }

        private System.Drawing.Icon LoadFromResources(string fileExtension) {
            object obj = Properties.Resources.ResourceManager.GetObject(fileExtension);
            return ((System.Drawing.Icon)(obj));
        }
    }
}
