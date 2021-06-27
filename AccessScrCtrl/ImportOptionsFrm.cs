using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccessIO;
using AccessScrCtrl.Profiles;

namespace AccessScrCtrl {
    public partial class ImportOptionsFrm : Form {

        private ImportOptions options;


        /// <summary>
        /// Default constructor
        /// </summary>
        public ImportOptionsFrm() {
            InitializeComponent();
            options = new ImportOptions();
        }

        /// <summary>
        /// Gets <see cref="ImportOptions"/> configured in this dialog
        /// </summary>
        public ImportOptions Options {
            get {
                return new ImportOptions {
                    OverwriteDatabase = overwriteCheckBox.Checked,
                    ConfirmImportedObjects = overwritePromptCheckBox.Checked,
                    RemoveNotLoaded = deleteNotLoadedCheckBox.Checked,
                    AllowDataLostTables = tablesGrid.GetValues().ToList(),
                    Excludes = excludesGrid.GetValues().ToList(),
                };
            } set {
                if (value == null) {
                    overwriteCheckBox.Checked = false;
                    overwritePromptCheckBox.Checked = false;
                    deleteNotLoadedCheckBox.Checked = false;
                    tablesGrid.Clear();
                    excludesGrid.Clear();
                } else {
                    overwriteCheckBox.Checked = value.OverwriteDatabase;
                    overwritePromptCheckBox.Checked = value.ConfirmImportedObjects;
                    deleteNotLoadedCheckBox.Checked = value.RemoveNotLoaded;
                    tablesGrid.AddValues(value.AllowDataLostTables);
                    excludesGrid.AddValues(value.Excludes);
                }
            }

        }
        private void overwriteCheckBox_CheckedChanged(object sender, EventArgs e) {
            bool value = !overwriteCheckBox.Checked;
            deleteNotLoadedCheckBox.Enabled = value;
        }

    }
}
