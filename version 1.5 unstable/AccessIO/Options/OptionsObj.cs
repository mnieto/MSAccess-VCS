using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AccessIO {

    /// <summary>
    /// Base class for specific options for each MSAccss object type. Each instance has its own OptionsObj object
    /// </summary>
    /// <remarks>
    /// Descendant objects must have the OptionsXXXXX signature, where XXXX is the name of the object type as <see cref="ObjectType"/>
    /// If can not find a descendant, a generic OptionsObj is created with basic common functionality
    /// </remarks>
    public class OptionsObj {

        protected AccessObject accessObj;

        /// <summary>
        /// Full file name that contains the object definition
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Returns a options set according to the object type
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        /// <remarks>
        /// Not all objectType must have a custom OptionsObj descendant, but if it has OptionsObj descendants must have the 
        /// OptionsXXXXX signature, where XXXX is the name of the object type as <see cref="ObjectType"/> If there is not a descendant
        /// a generic OptionsObj is created.
        /// </remarks>
        public static OptionsObj OptionsObjFactory(string name, ObjectType objectType) {

            try {
                Type t = Type.GetType("AccessIO.Options" + objectType.ToString(), true, true);
                OptionsObj opt = (OptionsObj)Activator.CreateInstance(t);
                opt.Name = name;
                return opt;
            } catch (TypeLoadException) {
                OptionsObj opt = new OptionsObj();
                opt.Name = name;
                return opt;
            }
        }

        /// <summary>
        /// Triggers the contextual menu with specific options for each object type
        /// </summary>
        /// <param name="parentCtrl">control where the <paramref name="location"/> param is relative to</param>
        /// <param name="contextMenu">contextual menu to customize and show</param>
        /// <param name="location">point where the user do click</param>
        public virtual void DisplayOptions(Control parentCtrl, ContextMenuStrip contextMenu, Point location) {
            contextMenu.Items.AddRange(GetMenuItems());
            contextMenu.Show(parentCtrl, location);
        }

        protected virtual ToolStripItem[] GetMenuItems() {
            ToolStripItem[] collection = new ToolStripItem[1];
            collection[0] = new ToolStripMenuItem(Properties.Options.Open);
            collection[0].Click += new EventHandler(menuItem1_Click);
            return collection;
        }

        void menuItem1_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(this.Name);
        }

    }
}
