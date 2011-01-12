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
    public partial class MainFrm : Form {
        public MainFrm() {
            InitializeComponent();
        }

        private void selectFileButton_Click(object sender, EventArgs e) {
            if (openDlg.ShowDialog() == DialogResult.OK) {
                fileNameTextBox.Text = openDlg.FileName;
                objectTree.FileName = fileNameTextBox.Text;
                saveButton.Enabled = true;
            } else {
                fileNameTextBox.Text = String.Empty;
                saveButton.Enabled = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(objectTree.App.WorkingCopyPath))
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            try {
                List<IObjectName> selectedObjects = objectTree.SelectedNodes;
                foreach (IObjectName name in selectedObjects) {
                    AccessObject accessObject = AccessObject.CreateInstance(objectTree.App, name.ObjectType, name.Name);
                    accessObject.Save();
                }
                MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, selectedObjects.Count), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e) {
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                workingCopyTextBox.Text = folderDlg.SelectedPath;
                objectTree.App.WorkingCopyPath = folderDlg.SelectedPath;
            }
        }

        private void MainFrm_Load(object sender, EventArgs e) {
            fileNameTextBox.Text = @"C:\Users\Miguel\Documents\Trabajos\Irumold\CRM\CONTACTOS.mdb";
            workingCopyTextBox.Text = @"C:\Users\Miguel\Documents\Curriculums";
            objectTree.FileName = fileNameTextBox.Text;
            objectTree.App.WorkingCopyPath = workingCopyTextBox.Text;
        }
    }
}
