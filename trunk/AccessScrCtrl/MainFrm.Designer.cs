namespace AccessScrCtrl {
    partial class MainFrm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.openDlg = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.workingCopyTextBox = new System.Windows.Forms.TextBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.saveTab = new System.Windows.Forms.TabPage();
            this.saveButton = new System.Windows.Forms.Button();
            this.objectTree = new AccessScrCtrlUI.ObjectTree();
            this.loadTab = new System.Windows.Forms.TabPage();
            this.optionsButton = new System.Windows.Forms.Button();
            this.filesTree = new AccessScrCtrlUI.FilesTree();
            this.loadButton = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.infoToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.saveTab.SuspendLayout();
            this.loadTab.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Access &file name";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Location = new System.Drawing.Point(16, 30);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(322, 20);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // selectFileButton
            // 
            this.selectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFileButton.Location = new System.Drawing.Point(344, 28);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(24, 23);
            this.selectFileButton.TabIndex = 2;
            this.selectFileButton.Text = "···";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // openDlg
            // 
            this.openDlg.Filter = "Access database (*.mdb; *.adp;*.accdb)|*.mdb;*.adp;*.accdb";
            this.openDlg.SupportMultiDottedExtensions = true;
            this.openDlg.Title = "Select Access file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "&Working copy folder";
            // 
            // workingCopyTextBox
            // 
            this.workingCopyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workingCopyTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.workingCopyTextBox.Location = new System.Drawing.Point(19, 74);
            this.workingCopyTextBox.Name = "workingCopyTextBox";
            this.workingCopyTextBox.Size = new System.Drawing.Size(319, 20);
            this.workingCopyTextBox.TabIndex = 6;
            this.workingCopyTextBox.TextChanged += new System.EventHandler(this.workingCopyTextBox_TextChanged);
            this.workingCopyTextBox.Leave += new System.EventHandler(this.workingCopyTextBox_Leave);
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderButton.Location = new System.Drawing.Point(343, 72);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(24, 23);
            this.selectFolderButton.TabIndex = 7;
            this.selectFolderButton.Text = "···";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // folderDlg
            // 
            this.folderDlg.Description = "Root folder for the working copy";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.saveTab);
            this.tabControl1.Controls.Add(this.loadTab);
            this.tabControl1.ImageList = this.imageList;
            this.tabControl1.Location = new System.Drawing.Point(19, 101);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(348, 333);
            this.tabControl1.TabIndex = 9;
            // 
            // saveTab
            // 
            this.saveTab.Controls.Add(this.saveButton);
            this.saveTab.Controls.Add(this.objectTree);
            this.saveTab.ImageKey = "Export";
            this.saveTab.Location = new System.Drawing.Point(4, 23);
            this.saveTab.Name = "saveTab";
            this.saveTab.Padding = new System.Windows.Forms.Padding(3);
            this.saveTab.Size = new System.Drawing.Size(340, 306);
            this.saveTab.TabIndex = 0;
            this.saveTab.Text = "Save";
            this.saveTab.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(259, 277);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // objectTree
            // 
            this.objectTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectTree.App = null;
            this.objectTree.FileName = null;
            this.objectTree.Location = new System.Drawing.Point(6, 10);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(328, 261);
            this.objectTree.TabIndex = 9;
            this.objectTree.SaveSelectedObjectsCompleted += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsCompletedEventArgs>(this.objectTree_SaveSelectedObjectsCompleted);
            this.objectTree.SaveSelectecObjectsProgress += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsProgressEventArgs>(this.objectTree_SaveSelectecObjectsProgress);
            // 
            // loadTab
            // 
            this.loadTab.Controls.Add(this.optionsButton);
            this.loadTab.Controls.Add(this.filesTree);
            this.loadTab.Controls.Add(this.loadButton);
            this.loadTab.ImageKey = "Import";
            this.loadTab.Location = new System.Drawing.Point(4, 23);
            this.loadTab.Name = "loadTab";
            this.loadTab.Padding = new System.Windows.Forms.Padding(3);
            this.loadTab.Size = new System.Drawing.Size(340, 306);
            this.loadTab.TabIndex = 1;
            this.loadTab.Text = "Load";
            this.loadTab.UseVisualStyleBackColor = true;
            // 
            // optionsButton
            // 
            this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsButton.Enabled = false;
            this.optionsButton.Location = new System.Drawing.Point(178, 277);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(75, 23);
            this.optionsButton.TabIndex = 14;
            this.optionsButton.Text = "&Options...";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // filesTree
            // 
            this.filesTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesTree.Location = new System.Drawing.Point(6, 6);
            this.filesTree.Name = "filesTree";
            this.filesTree.Size = new System.Drawing.Size(328, 265);
            this.filesTree.TabIndex = 13;
            this.filesTree.WorkingCopyPath = null;
            this.filesTree.LoadSelectedObjectsCompleted += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsCompletedEventArgs>(this.filesTree_LoadSelectedObjectsCompleted);
            this.filesTree.LoadSelectecObjectsProgress += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsProgressEventArgs>(this.filesTree_LoadSelectecObjectsProgress);
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(259, 277);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 12;
            this.loadButton.Text = "&Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Export");
            this.imageList.Images.SetKeyName(1, "Import");
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStrip});
            this.statusStrip.Location = new System.Drawing.Point(0, 437);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(383, 22);
            this.statusStrip.TabIndex = 10;
            this.statusStrip.Text = "statusStrip1";
            // 
            // infoToolStrip
            // 
            this.infoToolStrip.Name = "infoToolStrip";
            this.infoToolStrip.Size = new System.Drawing.Size(0, 17);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 459);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.selectFolderButton);
            this.Controls.Add(this.workingCopyTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.selectFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.Text = "Access Source Control";
            this.tabControl1.ResumeLayout(false);
            this.saveTab.ResumeLayout(false);
            this.loadTab.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.OpenFileDialog openDlg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox workingCopyTextBox;
        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage saveTab;
        private System.Windows.Forms.Button saveButton;
        private AccessScrCtrlUI.ObjectTree objectTree;
        private System.Windows.Forms.TabPage loadTab;
        private System.Windows.Forms.Button loadButton;
        private AccessScrCtrlUI.FilesTree filesTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel infoToolStrip;
    }
}

