
namespace AccessScrCtrl.Profiles {
    partial class ProfileFrm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.accessNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.workingCopyTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.profileFileNameTextBox = new System.Windows.Forms.TextBox();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.selectWorkingCopyButton = new System.Windows.Forms.Button();
            this.savePathAsGroupBox = new System.Windows.Forms.GroupBox();
            this.absoluteRadioButton = new System.Windows.Forms.RadioButton();
            this.relativeRadioButton = new System.Windows.Forms.RadioButton();
            this.selectProfileButton = new System.Windows.Forms.Button();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.importTab = new System.Windows.Forms.TabPage();
            this.exportTab = new System.Windows.Forms.TabPage();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.savePathAsGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(55, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(299, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            this.nameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.nameTextBox_Validating);
            // 
            // accessNameTextBox
            // 
            this.accessNameTextBox.Enabled = false;
            this.accessNameTextBox.Location = new System.Drawing.Point(14, 120);
            this.accessNameTextBox.Name = "accessNameTextBox";
            this.accessNameTextBox.Size = new System.Drawing.Size(340, 20);
            this.accessNameTextBox.TabIndex = 3;
            this.accessNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.accessNameTextBox_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Access database";
            // 
            // workingCopyTextBox
            // 
            this.workingCopyTextBox.Enabled = false;
            this.workingCopyTextBox.Location = new System.Drawing.Point(14, 163);
            this.workingCopyTextBox.Name = "workingCopyTextBox";
            this.workingCopyTextBox.Size = new System.Drawing.Size(340, 20);
            this.workingCopyTextBox.TabIndex = 5;
            this.workingCopyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.workingCopyTextBox_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Working copy";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Profile &File";
            // 
            // profileFileNameTextBox
            // 
            this.profileFileNameTextBox.Location = new System.Drawing.Point(14, 55);
            this.profileFileNameTextBox.Name = "profileFileNameTextBox";
            this.profileFileNameTextBox.Size = new System.Drawing.Size(340, 20);
            this.profileFileNameTextBox.TabIndex = 7;
            this.profileFileNameTextBox.TextChanged += new System.EventHandler(this.profileFileNameTextBox_TextChanged);
            this.profileFileNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.profileFileNameTextBox_Validating);
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.CheckFileExists = true;
            this.saveFileDlg.DefaultExt = "accsvc";
            this.saveFileDlg.Filter = "MS Access Source control version (*.accvcs)|*.accvcs|Json files (*.json)|*.json|A" +
    "ll files (*.*)|*.*";
            this.saveFileDlg.SupportMultiDottedExtensions = true;
            this.saveFileDlg.Title = "Save profile name";
            // 
            // openFileDlg
            // 
            this.openFileDlg.Filter = resources.GetString("openFileDlg.Filter");
            this.openFileDlg.Title = "Select MSAccess database";
            // 
            // selectWorkingCopyButton
            // 
            this.selectWorkingCopyButton.Enabled = false;
            this.selectWorkingCopyButton.Location = new System.Drawing.Point(360, 161);
            this.selectWorkingCopyButton.Name = "selectWorkingCopyButton";
            this.selectWorkingCopyButton.Size = new System.Drawing.Size(24, 23);
            this.selectWorkingCopyButton.TabIndex = 8;
            this.selectWorkingCopyButton.Text = "···";
            this.selectWorkingCopyButton.UseVisualStyleBackColor = true;
            this.selectWorkingCopyButton.Click += new System.EventHandler(this.selectWorkingCopyButton_Click);
            // 
            // savePathAsGroupBox
            // 
            this.savePathAsGroupBox.Controls.Add(this.absoluteRadioButton);
            this.savePathAsGroupBox.Controls.Add(this.relativeRadioButton);
            this.savePathAsGroupBox.Location = new System.Drawing.Point(14, 199);
            this.savePathAsGroupBox.Name = "savePathAsGroupBox";
            this.savePathAsGroupBox.Size = new System.Drawing.Size(200, 47);
            this.savePathAsGroupBox.TabIndex = 9;
            this.savePathAsGroupBox.TabStop = false;
            this.savePathAsGroupBox.Text = "Save path as";
            // 
            // absoluteRadioButton
            // 
            this.absoluteRadioButton.AutoSize = true;
            this.absoluteRadioButton.Location = new System.Drawing.Point(101, 19);
            this.absoluteRadioButton.Name = "absoluteRadioButton";
            this.absoluteRadioButton.Size = new System.Drawing.Size(66, 17);
            this.absoluteRadioButton.TabIndex = 12;
            this.absoluteRadioButton.Text = "Absolute";
            this.absoluteRadioButton.UseVisualStyleBackColor = true;
            this.absoluteRadioButton.Click += new System.EventHandler(this.ChangePathStyle_Click);
            // 
            // relativeRadioButton
            // 
            this.relativeRadioButton.AutoSize = true;
            this.relativeRadioButton.Checked = true;
            this.relativeRadioButton.Location = new System.Drawing.Point(22, 19);
            this.relativeRadioButton.Name = "relativeRadioButton";
            this.relativeRadioButton.Size = new System.Drawing.Size(64, 17);
            this.relativeRadioButton.TabIndex = 11;
            this.relativeRadioButton.TabStop = true;
            this.relativeRadioButton.Text = "Relative";
            this.relativeRadioButton.UseVisualStyleBackColor = true;
            this.relativeRadioButton.Click += new System.EventHandler(this.ChangePathStyle_Click);
            // 
            // selectProfileButton
            // 
            this.selectProfileButton.Location = new System.Drawing.Point(360, 53);
            this.selectProfileButton.Name = "selectProfileButton";
            this.selectProfileButton.Size = new System.Drawing.Size(24, 23);
            this.selectProfileButton.TabIndex = 10;
            this.selectProfileButton.Text = "···";
            this.selectProfileButton.UseVisualStyleBackColor = true;
            this.selectProfileButton.Click += new System.EventHandler(this.selectProfileButton_Click);
            // 
            // selectFileButton
            // 
            this.selectFileButton.Enabled = false;
            this.selectFileButton.Location = new System.Drawing.Point(360, 118);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(24, 23);
            this.selectFileButton.TabIndex = 11;
            this.selectFileButton.Text = "···";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(228, 278);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(309, 278);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.importTab);
            this.tabControl1.Controls.Add(this.exportTab);
            this.tabControl1.Location = new System.Drawing.Point(404, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(514, 288);
            this.tabControl1.TabIndex = 14;
            // 
            // importTab
            // 
            this.importTab.Location = new System.Drawing.Point(4, 22);
            this.importTab.Name = "importTab";
            this.importTab.Padding = new System.Windows.Forms.Padding(3);
            this.importTab.Size = new System.Drawing.Size(506, 262);
            this.importTab.TabIndex = 0;
            this.importTab.Text = "Import options";
            this.importTab.UseVisualStyleBackColor = true;
            // 
            // exportTab
            // 
            this.exportTab.Location = new System.Drawing.Point(4, 22);
            this.exportTab.Name = "exportTab";
            this.exportTab.Padding = new System.Windows.Forms.Padding(3);
            this.exportTab.Size = new System.Drawing.Size(506, 262);
            this.exportTab.TabIndex = 1;
            this.exportTab.Text = "Export options";
            this.exportTab.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ProfileFrm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(927, 316);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.selectProfileButton);
            this.Controls.Add(this.savePathAsGroupBox);
            this.Controls.Add(this.selectWorkingCopyButton);
            this.Controls.Add(this.profileFileNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.workingCopyTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.accessNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ProfileFrm";
            this.ShowInTaskbar = false;
            this.Text = "Profile";
            this.Load += new System.EventHandler(this.ProfileFrm_Load);
            this.savePathAsGroupBox.ResumeLayout(false);
            this.savePathAsGroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox accessNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox workingCopyTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox profileFileNameTextBox;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.Button selectWorkingCopyButton;
        private System.Windows.Forms.GroupBox savePathAsGroupBox;
        private System.Windows.Forms.RadioButton absoluteRadioButton;
        private System.Windows.Forms.RadioButton relativeRadioButton;
        private System.Windows.Forms.Button selectProfileButton;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage importTab;
        private System.Windows.Forms.TabPage exportTab;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}