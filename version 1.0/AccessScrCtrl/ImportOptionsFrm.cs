using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccessIO;

namespace AccessScrCtrl {
    public partial class ImportOptionsFrm : Form {

        private AccessProjectType projectType;
        private ImportOptions options;

        /// <summary>
        /// Constructor. Initializes the form and fill the list of tables that will be available to configure their AllowDataLost property
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="options"></param>
        public ImportOptionsFrm(AccessProjectType projectType, ImportOptions options) {
            InitializeComponent();
            this.projectType = projectType;
            this.options = (ImportOptions)options.Clone();
            overwriteCheckBox.Checked = options.OverwriteDatabase;
            overwritePromptCheckBox.Checked = options.Prompt;
            deleteNotLoadedCheckBox.Checked = options.DeleteNotLoaded;
            if (projectType == AccessProjectType.Adp) {
                allowDataLostCheckBox.Enabled = false;
                tablesList.Enabled = false;
            }
        }

        /// <summary>
        /// Constructor. Initializes the form and fill the list of tables that will be available to configure their AllowDataLost property
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="options"></param>
        /// <param name="tableNames"></param>
        public ImportOptionsFrm(AccessProjectType projectType, ImportOptions options, string[] tableNames): this(projectType, options) {
            tablesList.Items.AddRange(tableNames);
        }

        /// <summary>
        /// Gets <see cref="ImportOptions"/> configured in this dialog
        /// </summary>
        public ImportOptions Options {
            get { return options; }
        }

        private void allowDataLostCheckBox_CheckedChanged(object sender, EventArgs e) {
            tablesList.Enabled = allowDataLostCheckBox.Checked;
        }

        private void overwriteCheckBox_CheckedChanged(object sender, EventArgs e) {
            bool value = !overwriteCheckBox.Checked;
            deleteNotLoadedCheckBox.Enabled = value;
            if (this.projectType != AccessProjectType.Adp) {
                allowDataLostCheckBox.Enabled = value;
                tablesList.Enabled = value && allowDataLostCheckBox.Checked;
            }
        }

        private void okButton_Click(object sender, EventArgs e) {
            options.OverwriteDatabase = overwriteCheckBox.Checked;
            options.Prompt = overwritePromptCheckBox.Checked;
            options.DeleteNotLoaded = deleteNotLoadedCheckBox.Checked;
            if (projectType == AccessProjectType.Adp && tablesList.CheckedItems.Count > 0) {
                options.AllowDataLost.AddRange(tablesList.CheckedItems.Cast<string>());
            }
        }
    }
}
