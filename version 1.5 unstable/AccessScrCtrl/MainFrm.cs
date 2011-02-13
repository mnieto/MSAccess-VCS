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

        private void MainFrm_Load(object sender, EventArgs e) {

            //Some tests for 
            //mdb file
            //fileNameTextBox.Text = @"C:\Users\Miguel\Documents\Trabajos\Irumold\CRM\CONTACTOS.mdb";
            //workingCopyTextBox.Text = @"C:\Users\Miguel\Documents\Curriculums";
            //objectTree.FileName = fileNameTextBox.Text;
            //if (objectTree.App != null)
            //  objectTree.App.WorkingCopyPath = workingCopyTextBox.Text;
            //filesTree.WorkingCopyPath = workingCopyTextBox.Text;

            //adp file
            //fileNameTextBox.Text = @"C:\Users\Miguel\Documents\Progs\AccessSVN\TestAccessProjects\Adp\RolesSAP.adp";
            //workingCopyTextBox.Text = @"C:\Users\Miguel\Documents\Progs\AccessSVN\TestAccessProjects\Adp";
            //objectTree.FileName = fileNameTextBox.Text;
            //if (objectTree.App != null)
            //    objectTree.App.WorkingCopyPath = workingCopyTextBox.Text;
            //filesTree.WorkingCopyPath = workingCopyTextBox.Text;
        }

        private void selectFileButton_Click(object sender, EventArgs e) {
            if (openDlg.ShowDialog() == DialogResult.OK) {
                Cursor = Cursors.WaitCursor;
                try {
                    progressInfoLabel.Text = Properties.Resources.LoadingObjectsTree;
                    fileNameTextBox.Text = openDlg.FileName;
                    objectTree.FileName = fileNameTextBox.Text;
                    saveButton.Enabled = true;

                } finally {
                    progressInfoLabel.Text = string.Empty;
                    Cursor = Cursors.Default;
                }
            } else {
                fileNameTextBox.Text = String.Empty;
                saveButton.Enabled = false;
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e) {
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                workingCopyTextBox.Text = folderDlg.SelectedPath;
                objectTree.App.WorkingCopyPath = folderDlg.SelectedPath;
                filesTree.WorkingCopyPath = folderDlg.SelectedPath;
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(objectTree.App.WorkingCopyPath))
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            try {
                List<IObjectName> selectedObjects = objectTree.SelectedNodes;
                foreach (IObjectName name in selectedObjects) {
                    AccessObject accessObject = AccessObject.CreateInstance(objectTree.App, name.ObjectType, name.Name);
                    accessObject.Load(name.Name);
                }
                MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, selectedObjects.Count), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(workingCopyTextBox.Text) || string.IsNullOrWhiteSpace(fileNameTextBox.Text)) {
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (objectTree.App == null || String.IsNullOrEmpty(objectTree.App.WorkingCopyPath)) {
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            objectTree.SaveSelectedObjectsAsync();

            //Another way of saving selected object: syncronous method
            //try {
            //    List<IObjectName> selectedObjects = objectTree.SelectedNodes;
            //    foreach (IObjectName name in selectedObjects) {
            //        progressInfoLabel.Text = String.Format(Properties.Resources.Saving, name.Name);
            //        AccessObject accessObject = AccessObject.CreateInstance(objectTree.App, name.ObjectType, name.Name);
            //        accessObject.Save();
            //    }
            //    MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, selectedObjects.Count), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //} catch (Exception ex) {
            //    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //} finally {
            //    progressInfoLabel.Text = String.Empty;
            //}

        }

        private void objectTree_SaveSelectecObjectsProgress(object sender, AccessScrCtrlUI.SaveSelectedObjectsProgressEventArgs e) {
            progressInfoLabel.Text = e.ObjectName.Name;
        }

        private void objectTree_SaveSelectedObjectsCompleted(object sender, AccessScrCtrlUI.SaveSelectedObjectsCompletedEventArgs e) {
            progressInfoLabel.Text = string.Empty;
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else {
                MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, e.TotalOjectsSaved), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                filesTree.RefreshList();
            }
        }

    }
}
