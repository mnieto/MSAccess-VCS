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
        /// Constructor. Initializes the form and fill the list of tables that will be available to configure their AllowDataLost property
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="options"></param>
        public ImportOptionsFrm(ImportOptions options) : this() {
            this.options = options;
            overwriteCheckBox.Checked = options.OverwriteDatabase;
            overwritePromptCheckBox.Checked = options.ConfirmImportedObjects;
            deleteNotLoadedCheckBox.Checked = options.RemoveNotLoaded;
            tablesGrid.AddValues(options.AllowDataLostTables);
            excludesGrid.AddValues(options.Excludes);
            okButton.Visible = true;
            cancelButton.Visible = true;
        }

        /// <summary>
        /// Gets <see cref="ImportOptions"/> configured in this dialog
        /// </summary>
        public ImportOptions Options {
            get { return options; }
        }

        private void overwriteCheckBox_CheckedChanged(object sender, EventArgs e) {
            bool value = !overwriteCheckBox.Checked;
            deleteNotLoadedCheckBox.Enabled = value;
        }

        private void okButton_Click(object sender, EventArgs e) {
            options.OverwriteDatabase = overwriteCheckBox.Checked;
            options.ConfirmImportedObjects = overwritePromptCheckBox.Checked;
            options.RemoveNotLoaded = deleteNotLoadedCheckBox.Checked;
            options.AllowDataLostTables = tablesGrid.GetValues().ToList();
            options.Excludes = excludesGrid.GetValues().ToList();
        }
    }
}
