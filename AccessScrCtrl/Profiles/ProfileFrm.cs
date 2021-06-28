using AccessScrCtrl.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessScrCtrl.Profiles {
    public partial class ProfileFrm : Form {
        private string BaseText;
        private ImportOptionsFrm ImportFrm { get; set; }
        private Helpers.ValidationHelper<ProfileFrm> Validator;
        internal Profile Profile { get; private set; }
        internal bool IsNewProfile => Profile == null;

        public ProfileFrm() {
            InitializeComponent();
            BaseText = Text;
            Validator = new Helpers.ValidationHelper<ProfileFrm>(errorProvider1);
        }

        public ProfileFrm(string accessFileName, string workinCopy) : this() {
            Profile = new Profile {
                AccessFile = accessFileName,
                WorkingCopy = workinCopy
            };
            accessNameTextBox.Text = accessFileName;
            workingCopyTextBox.Text = workinCopy;
            absoluteRadioButton.Checked = true;
        }

        public ProfileFrm(Profile profile): this() {
            Profile = profile;
            DataBind();
        }
        
        private void ProfileFrm_Load(object sender, EventArgs e) {
            ImportFrm = EmbedForm(new ImportOptionsFrm(), importTab);
            ImportFrm.Options = Profile?.ImportOptions;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e) {
            Text = nameTextBox.Text.Length == 0 ? BaseText : $"{BaseText} - {nameTextBox.Text}";
        }
        private void nameTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = Validator.Validate(nameTextBox,
                () => !string.IsNullOrEmpty(nameTextBox.Text),
                Properties.Resources.SpecifyFileName);
        }

        private void profileFileNameTextBox_TextChanged(object sender, EventArgs e) {
            bool enabled = !string.IsNullOrEmpty(profileFileNameTextBox.Text);
            accessNameTextBox.Enabled = enabled;
            selectFileButton.Enabled = enabled;
            workingCopyTextBox.Enabled = enabled;
            selectWorkingCopyButton.Enabled = enabled;
        }

        private void profileFileNameTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = Validator.Validate(profileFileNameTextBox,
                () => !string.IsNullOrEmpty(profileFileNameTextBox.Text),
                Properties.Resources.SpecifyFileName);
        }

        private void accessNameTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = Validator.Validate(accessNameTextBox, () => {
                if (string.IsNullOrEmpty(accessNameTextBox.Text))
                    return Properties.Resources.SpecifyFileName;
                if (!string.IsNullOrEmpty(profileFileNameTextBox.Text)) {
                    string path = Helpers.PathUtil.GetFullPath(accessNameTextBox.Text, profileFileNameTextBox.Text);
                    if (!File.Exists(path)) {
                        return Properties.Resources.FileNotFound;
                    }
                }
                return string.Empty;
            });
        }

        private void workingCopyTextBox_Validating(object sender, CancelEventArgs e) {
            e.Cancel = Validator.Validate(workingCopyTextBox, () => {
                if (string.IsNullOrEmpty(workingCopyTextBox.Text))
                    return Properties.Resources.SpecifyFileName;
                if (!string.IsNullOrEmpty(profileFileNameTextBox.Text)) {
                    string path = Helpers.PathUtil.GetFullPath(workingCopyTextBox.Text, profileFileNameTextBox.Text);
                    if (!Directory.Exists(path)) {
                        return Properties.Resources.FolderNotFound;
                    }
                }
                return string.Empty;
            });
        }

        private void selectProfileButton_Click(object sender, EventArgs e) {
            string profileName = profileFileNameTextBox.Text;
            using (FileDialog dialog = IsNewProfile ? (FileDialog)CommonDialogs.OpenProfile(profileName) : CommonDialogs.SaveProfile(profileName)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    profileFileNameTextBox.Text = dialog.FileName;
                    if (string.IsNullOrEmpty(nameTextBox.Text)) {
                        nameTextBox.Text = Path.GetFileNameWithoutExtension(profileFileNameTextBox.Text);
                    }
                }
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e) {
            if (openFileDlg.ShowDialog() == DialogResult.OK) {
                string path = null;
                if (relativeRadioButton.Checked) {
                    path = MakeRelative(profileFileNameTextBox.Text, openFileDlg.FileName);
                } else {
                    path = openFileDlg.FileName;
                }
                accessNameTextBox.Text = path;
            }
        }

        private void selectWorkingCopyButton_Click(object sender, EventArgs e) {
            var selectFolder = new Helpers.FolderPicker();
            if (selectFolder.ShowDialog(this.Handle).GetValueOrDefault(false)) {
                string path = null;
                if (relativeRadioButton.Checked && IsValidProfileFileName()) {
                    path = MakeRelative(profileFileNameTextBox.Text, selectFolder.ResultPath);
                } else {
                    path = selectFolder.ResultPath;
                }
                workingCopyTextBox.Text = path;
            }
        }

        private void ChangePathStyle_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(profileFileNameTextBox.Text))
                return;
            var radioButton = (RadioButton)sender;
            switch (radioButton.Name) {
                case nameof(absoluteRadioButton):
                    if (!string.IsNullOrEmpty(accessNameTextBox.Text))
                        accessNameTextBox.Text = MakeAbsolute(accessNameTextBox.Text, profileFileNameTextBox.Text);
                    if (!string.IsNullOrEmpty(workingCopyTextBox.Text))
                        workingCopyTextBox.Text = MakeAbsolute(workingCopyTextBox.Text, profileFileNameTextBox.Text);
                    break;
                case nameof(relativeRadioButton):
                    if (!string.IsNullOrEmpty(accessNameTextBox.Text))
                        accessNameTextBox.Text = MakeRelative(profileFileNameTextBox.Text, accessNameTextBox.Text);
                    if (!string.IsNullOrEmpty(workingCopyTextBox.Text))
                        workingCopyTextBox.Text = MakeRelative(profileFileNameTextBox.Text, workingCopyTextBox.Text);
                    break;
            }
        }

        private void okButton_Click(object sender, EventArgs e) {
            if (!ValidateChildren()) {
                DialogResult = DialogResult.None;
            } else {
                Profile = BuildProfile();
            }
        }

        private bool IsValidProfileFileName() {
            return
                !string.IsNullOrWhiteSpace(profileFileNameTextBox.Text);
        }

        private string MakeRelative(string relativeTo, string path) {
            return Helpers.PathUtil.GetRelativePath(relativeTo, path);
        }
        private string MakeAbsolute(string path, string basePath) {
            return Helpers.PathUtil.GetFullPath(path, basePath);
        }

        private T EmbedForm<T>(T form, Control parentControl) where T: Form {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Parent = parentControl;
            form.Dock = DockStyle.Fill;
            form.Visible = true;
            return form;
        }

        private Profile BuildProfile() {
            return new Profile {
                Name = nameTextBox.Text,
                AccessFile = accessNameTextBox.Text,
                WorkingCopy = workingCopyTextBox.Text,
                ImportOptions = ImportFrm.Options,
                FileName = profileFileNameTextBox.Text
            };
        }

        private void DataBind() {
            nameTextBox.Text = Profile.Name;
            accessNameTextBox.Text = Profile.AccessFile;
            workingCopyTextBox.Text = Profile.WorkingCopy;
            profileFileNameTextBox.Text = Profile.FileName;
        }

    }
}
