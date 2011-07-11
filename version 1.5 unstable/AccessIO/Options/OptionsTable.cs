using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AccessIO {

    /// <summary>
    /// Options for Table objects
    /// </summary>
    public class OptionsTable : OptionsObj {

        /// <summary>
        /// If <c>true</c> and table has data, <see cref="Load"/> method will overwrite the table structure and data will lost
        /// If <c>false</c> and table has data, <see cref="Load"/> method will raise an exception
        /// </summary>
        public bool AllowDataLost { get; set; }

        public OptionsTable() { 
        }

        public OptionsTable(Table tableObj):this() {
            accessObj = tableObj;
        }

        protected override ToolStripItem[] GetMenuItems() {
            List<ToolStripItem> collection = new List<ToolStripItem>();
            collection.AddRange(base.GetMenuItems());
            ToolStripMenuItem menuItem = new ToolStripMenuItem(Properties.Options.AllowDataLost);
            menuItem.Checked = AllowDataLost;
            menuItem.Click += new EventHandler(menuItem_Click);
            collection.Add(menuItem);
            return collection.ToArray();
        }

        void menuItem_Click(object sender, EventArgs e) {
            AllowDataLost = !AllowDataLost;
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            menuItem.Checked = AllowDataLost;
        }
    }
}
