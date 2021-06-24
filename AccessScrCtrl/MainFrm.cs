using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccessIO;
using AccessIO.Properties;
using AccessScrCtrl.Profiles;

namespace AccessScrCtrl {
    public partial class MainFrm : Form {

        private Profile Profile { get; set; }
        private Configuration Config { get; set; }
        private bool workingCopyTextBoxChanged = false;

        private string StatusInfo {
            get {
                return infoToolStrip.Text;
            }
            set {
                infoToolStrip.Text = value;
            }
        }

        public MainFrm(string[] args) : this() {
            if (args.Length > 0) {
                LoadProfile(args[0]);
            }
        }

        public MainFrm() {
            InitializeComponent();
            try {
                Config = Configuration.LoadConfiguration();
            } catch {
                //If any error occurs loading the configuration,create a default one
                Config = new Configuration();
            }
            DisplayMRU();
        }

        private void selectFileButton_Click(object sender, EventArgs e) {
            if (openDlg.ShowDialog() == DialogResult.OK) {
                fileNameTextBox.Text = openDlg.FileName;
                if (!string.IsNullOrWhiteSpace(workingCopyTextBox.Text)) {
                    Attach(fileNameTextBox.Text, workingCopyTextBox.Text);
                }
            } else {
                fileNameTextBox.Text = String.Empty;
                saveButton.Enabled = false;
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e) {
            if (!String.IsNullOrWhiteSpace(workingCopyTextBox.Text) && Directory.Exists(workingCopyTextBox.Text)) {
                folderDlg.SelectedPath = workingCopyTextBox.Text;
            }
            if (folderDlg.ShowDialog() == DialogResult.OK) {
                workingCopyTextBox.Text = folderDlg.SelectedPath;
                workingCopyTextBoxChanged = false;      //Change event is raised even if Text property is changed programaticaly
                if (!string.IsNullOrWhiteSpace(fileNameTextBox.Text) && !string.IsNullOrWhiteSpace(workingCopyTextBox.Text)) {
                    Attach(fileNameTextBox.Text, workingCopyTextBox.Text);
                }
            }
        }

        private void Attach(string accessFileName, string workingFolderName) {
            var tempProfile = new Profile {
                AccessFile = accessFileName,
                WorkingCopy = workingFolderName
            };
            Cursor = Cursors.WaitCursor;
            StatusInfo = Properties.Resources.LoadingObjectsTree;
            backgroundAttach.RunWorkerAsync(tempProfile);
        }

        private void DisplayMRU() {
            var profiles = Config.LastProfiles();

            UpperSeparatorMenu.Visible = profiles.Count() != 0;
            int index = FileMenu.DropDownItems.IndexOfKey(nameof(UpperSeparatorMenu)) + 1;
            int number = 1;
            foreach (var profile in profiles) { 
                var menuItem = new ToolStripMenuItem {
                    Name = $"MRU{number}",
                    Text = $"&{number} {profile.Name}",
                    Tag = profile.FileName
                };
                menuItem.Click += new EventHandler(MRU_Profile_Click);
                FileMenu.DropDownItems.Insert(index, menuItem);
                index++;
                number++;
            }
        }

        private void RefreshMRU() {
            var toBeRemoved = new List<ToolStripItem>();
            foreach (ToolStripItem menuItem in FileMenu.DropDownItems) {
                if (menuItem.Name.StartsWith("MRU")) {
                    toBeRemoved.Add(menuItem);
                }
            }
            foreach(var menuItem in toBeRemoved) {
                FileMenu.DropDownItems.Remove(menuItem);
            }
            DisplayMRU();
        }
        
        private void LoadProfile(string profilePath) {
            if (!File.Exists(profilePath)) {
                var response = MessageBox.Show(string.Format(Properties.Resources.ProfileNotFound, profilePath),
                    Properties.Resources.Error, 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);
                if (response == DialogResult.Yes) {
                    Config.RemoveProfile(profilePath);
                    RefreshMRU();
                }
            } else {
                Profile = Config.LoadProfile(profilePath);
                workingCopyTextBox.Text = Profile.WorkingCopy;
                folderDlg.SelectedPath = workingCopyTextBox.Text;
                
                fileNameTextBox.Text = Profile.AccessFile;
                openDlg.FileName = Profile.AccessFile;
                Attach(Profile.AccessFile, Profile.WorkingCopy);

                Config.AddProfile(Profile, profilePath);
                RefreshMRU();
            }
        }


        private void loadButton_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(objectTree.App.WorkingCopyPath))
                MessageBox.Show(Properties.Resources.WorkingCopyMissing, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            filesTree.Focus();
            loadButton.Enabled = false;
            filesTree.LoadSelectedObjectsAsync(objectTree.App);

            //Another way of load selected objects: syncronous method
            //AccessIO.BackupHelper backup = new AccessIO.BackupHelper(objectTree.App.FileName);
            //backup.DoBackUp();
            //string currentObjectName = null;
            //try {
            //    List<IObjecOptions> selectedObjects = filesTree.SelectedNodes;
            //    foreach (IObjecOptions currentObject in selectedObjects) {
            //        currentObjectName = currentObject.Name;
            //        AccessObject accessObject = AccessObject.CreateInstance(objectTree.App, currentObject.ObjectType, currentObject.ToString());
            //        accessObject.Options = currentObject.Options;
            //        accessObject.Load(currentObject.Name);
            //    }
            //    backup.Commit();
            //    MessageBox.Show(String.Format(Properties.Resources.ObjectsLoaded, selectedObjects.Count), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //} catch (Exception ex) {
            //    string msg = String.Format(Properties.Resources.ErrorLoadingObject, currentObjectName, ex.Message);
            //    MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    objectTree.App = null;              //1.- exit MS Access to unlock the file and restore it
            //    backup.RollBack();                  //2.- restore the file
            //    AttachFile();                       //3.- load again MS Access
            //}
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

        private void objectTree_SaveSelectecObjectsProgress(object sender, AccessScrCtrlUI.SelectedObjectsProgressEventArgs e) {
            StatusInfo = e.ObjectName.Name;
        }

        private void objectTree_SaveSelectedObjectsCompleted(object sender, AccessScrCtrlUI.SelectedObjectsCompletedEventArgs e) {
            StatusInfo = string.Empty;
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else {
                MessageBox.Show(String.Format(Properties.Resources.ObjectsSaved, e.TotalOjectsSaved), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                filesTree.RefreshList();
            }
            saveButton.Enabled = true;
        }

        private void filesTree_LoadSelectecObjectsProgress(object sender, AccessScrCtrlUI.SelectedObjectsProgressEventArgs e) {
            StatusInfo = e.ObjectName.ToString();
        }

        private void filesTree_LoadSelectedObjectsCompleted(object sender, AccessScrCtrlUI.SelectedObjectsCompletedEventArgs e) {
            StatusInfo = string.Empty;
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                MessageBox.Show(String.Format(Properties.Resources.ObjectsLoaded, e.TotalOjectsSaved), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                objectTree.RefreshList();
            }
            loadButton.Enabled = true;
        }

        private void workingCopyTextBox_TextChanged(object sender, EventArgs e) {
            workingCopyTextBoxChanged = true;
        }

        private void workingCopyTextBox_Leave(object sender, EventArgs e) {
            if (workingCopyTextBoxChanged && 
                !string.IsNullOrWhiteSpace(workingCopyTextBox.Text) && 
                Directory.Exists(workingCopyTextBox.Text) &&
                !string.IsNullOrWhiteSpace(fileNameTextBox.Text))
            {
                workingCopyTextBoxChanged = false;
                folderDlg.SelectedPath = workingCopyTextBox.Text;
                Attach(fileNameTextBox.Text, workingCopyTextBox.Text);
            }
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e) {
            objectTree.Dispose();
        }

        private void newMenu_Click(object sender, EventArgs e) {
            var profileFrm = new ProfileFrm(null);
            if (profileFrm.ShowDialog() == DialogResult.OK) {
                Config.SaveProfile(profileFrm.Profile, profileFrm.ProfileFileName);
                Config.AddProfile(profileFrm.Profile, profileFrm.ProfileFileName);
                LoadProfile(profileFrm.ProfileFileName);
            }
        }

        private void ExitMenu_Click(object sender, EventArgs e) {
            Close();
        }

        private void MRU_Profile_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem) {
                var profileMenu = (ToolStripMenuItem)sender;
                if (profileMenu.Tag != null) {
                    string fileName = profileMenu.Tag.ToString();
                    try {
                        LoadProfile(fileName);
                    } catch (Exception ex) {
                        MessageBox.Show(string.Format(Properties.Resources.UnexpectedError, ex.Message),
                            Properties.Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void backgroundAttach_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            StatusInfo = string.Empty;
            Cursor = Cursors.Default;
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                saveButton.Enabled = true;
            }
        }

        private void backgroundAttach_DoWork(object sender, DoWorkEventArgs e) {
            var profile = (Profile)e.Argument;
            objectTree.Attach(profile.AccessFile, profile.WorkingCopy);
            filesTree.WorkingCopyPath = folderDlg.SelectedPath;
        }
    }
}
