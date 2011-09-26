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

        private ImportOptions importOptions;
        private bool workingCopyTextBoxChanged = false;

        private string StatusInfo {
            get {
                return infoToolStrip.Text;
            }
            set {
                infoToolStrip.Text = value;
            }
        }

        public MainFrm() {
            InitializeComponent();
            importOptions = new ImportOptions();
        }

        private void selectFileButton_Click(object sender, EventArgs e) {
            if (openDlg.ShowDialog() == DialogResult.OK) {
                Cursor = Cursors.WaitCursor;
                try {
                    StatusInfo = Properties.Resources.LoadingObjectsTree;
                    fileNameTextBox.Text = openDlg.FileName;
                    objectTree.FileName = fileNameTextBox.Text;
                    workingCopyTextBox.Text = System.IO.Path.GetDirectoryName(fileNameTextBox.Text);
                    if (!String.IsNullOrEmpty(workingCopyTextBox.Text))
                        objectTree.App.WorkingCopyPath = workingCopyTextBox.Text;
                    saveButton.Enabled = true;
                    workingCopyTextBox.Focus();

                } finally {
                    StatusInfo = string.Empty;
                    Cursor = Cursors.Default;
                }
            } else {
                fileNameTextBox.Text = String.Empty;
                saveButton.Enabled = false;
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e) {
            if (!String.IsNullOrWhiteSpace(workingCopyTextBox.Text) && System.IO.Directory.Exists(workingCopyTextBox.Text)) {
                folderDlg.SelectedPath = workingCopyTextBox.Text;
            }
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                workingCopyTextBox.Text = folderDlg.SelectedPath;
                workingCopyTextBoxChanged = false;      //Change event is raised even if Text property is changed programaticaly
                DoSetWorkingCopyPath();
            }
        }

        private void DoSetWorkingCopyPath() {
            if (objectTree.App != null)
                objectTree.App.WorkingCopyPath = folderDlg.SelectedPath;
            try {
                Cursor = Cursors.WaitCursor;
                StatusInfo = Properties.Resources.LoadingObjectsTree;
                statusStrip.Refresh();
                filesTree.WorkingCopyPath = folderDlg.SelectedPath;
            } finally {
                StatusInfo = string.Empty;
                Cursor = Cursors.Default;
            }
            optionsButton.Enabled = true;
        }

        private void loadButton_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(objectTree.App.WorkingCopyPath))
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            AccessIO.BackupHelper backup = new AccessIO.BackupHelper(objectTree.App.FileName);
            backup.DoBackUp();
            string currentObjectName = null;
            try {
                List<IObjecOptions> selectedObjects = filesTree.SelectedNodes;
                foreach (IObjecOptions currentObject in selectedObjects) {
                    currentObjectName = currentObject.Name;
                    AccessObject accessObject = AccessObject.CreateInstance(objectTree.App, currentObject.ObjectType, currentObject.ToString());
                    accessObject.Options = currentObject.Options;
                    accessObject.Load(currentObject.Name);
                }
                backup.Commit();
                MessageBox.Show(String.Format(Properties.Resources.ObjectsLoaded, selectedObjects.Count), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                string msg = String.Format(Properties.Resources.ErrorLoadingObject, currentObjectName, ex.Message);
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                backup.RollBack();
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {

            if (string.IsNullOrWhiteSpace(workingCopyTextBox.Text) || string.IsNullOrWhiteSpace(fileNameTextBox.Text)) {
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (objectTree.App != null)
                objectTree.App.WorkingCopyPath = workingCopyTextBox.Text;
            filesTree.WorkingCopyPath = folderDlg.SelectedPath;

            if (objectTree.App == null || String.IsNullOrEmpty(objectTree.App.WorkingCopyPath)) {
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            objectTree.Focus();
            saveButton.Enabled = false;
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
            StatusInfo = e.ObjectName.Name;
        }

        private void objectTree_SaveSelectedObjectsCompleted(object sender, AccessScrCtrlUI.SaveSelectedObjectsCompletedEventArgs e) {
            StatusInfo = string.Empty;
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else {
                MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, e.TotalOjectsSaved), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                filesTree.RefreshList();
            }
            saveButton.Enabled = true;
        }

        private void optionsButton_Click(object sender, EventArgs e) {
            ImportOptionsFrm frm = new ImportOptionsFrm(filesTree.ProjectType, importOptions, filesTree.ObjectNames(ObjectType.Table));
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                importOptions = frm.Options;
        }

        private void workingCopyTextBox_TextChanged(object sender, EventArgs e) {
            workingCopyTextBoxChanged = true;
        }

        private void workingCopyTextBox_Leave(object sender, EventArgs e) {
            if (workingCopyTextBoxChanged && 
                !String.IsNullOrWhiteSpace(workingCopyTextBox.Text) && 
                System.IO.Directory.Exists(workingCopyTextBox.Text)) {

                workingCopyTextBoxChanged = false;
                folderDlg.SelectedPath = workingCopyTextBox.Text;
                DoSetWorkingCopyPath();
            }

        }

    }
}
