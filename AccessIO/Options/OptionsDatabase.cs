using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessIO {
    class OptionsDatabase: OptionsObj {

        /// <summary>
        /// If <c>true</c> the database a new empty database is created and then the objects are restored to the database.
        /// If <c>false</c> the diferent objects are overwrite over the existing database.
        /// </summary>
        public bool OverwriteDatabase { get; set; }

        public OptionsDatabase() { }

        public OptionsDatabase(Database database) : this() {
            accessObj = database;
        }

        protected override ToolStripItem[] GetMenuItems() {
            List<ToolStripItem> collection = new List<ToolStripItem>();
            collection.AddRange(base.GetMenuItems());
            using (ToolStripMenuItem menuItem = new ToolStripMenuItem(Properties.Options.OverwriteDatabase)) {
                menuItem.Checked = OverwriteDatabase;
                menuItem.Click += new EventHandler(menuItem_Click);
                collection.Add(menuItem);
                return collection.ToArray();
            }
        }

        protected virtual void menuItem_Click(object sender, EventArgs e) {
            OverwriteDatabase = !OverwriteDatabase;
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            menuItem.Checked = OverwriteDatabase;
        }

    }
}
