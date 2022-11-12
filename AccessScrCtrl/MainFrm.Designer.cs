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
            this.label2 = new System.Windows.Forms.Label();
            this.workingCopyTextBox = new System.Windows.Forms.TextBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.saveTab = new System.Windows.Forms.TabPage();
            this.saveButton = new System.Windows.Forms.Button();
            this.objectTree = new AccessScrCtrlUI.ObjectTree();
            this.loadTab = new System.Windows.Forms.TabPage();
            this.filesTree = new AccessScrCtrlUI.FilesTree();
            this.loadButton = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.infoToolStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.UpperSeparatorMenu = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.profileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundAttach = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.saveTab.SuspendLayout();
            this.loadTab.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Access &file name";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Location = new System.Drawing.Point(12, 52);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(322, 20);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // selectFileButton
            // 
            this.selectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFileButton.Location = new System.Drawing.Point(340, 50);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(24, 23);
            this.selectFileButton.TabIndex = 2;
            this.selectFileButton.Text = "···";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
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
            this.workingCopyTextBox.Location = new System.Drawing.Point(15, 96);
            this.workingCopyTextBox.Name = "workingCopyTextBox";
            this.workingCopyTextBox.Size = new System.Drawing.Size(319, 20);
            this.workingCopyTextBox.TabIndex = 6;
            this.workingCopyTextBox.TextChanged += new System.EventHandler(this.workingCopyTextBox_TextChanged);
            this.workingCopyTextBox.Leave += new System.EventHandler(this.workingCopyTextBox_Leave);
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderButton.Location = new System.Drawing.Point(339, 94);
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
            this.tabControl1.Location = new System.Drawing.Point(19, 141);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(348, 346);
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
            this.saveTab.Size = new System.Drawing.Size(340, 319);
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
            this.objectTree.Location = new System.Drawing.Point(6, 10);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(328, 261);
            this.objectTree.TabIndex = 9;
            this.objectTree.SaveSelectedObjectsCompleted += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsCompletedEventArgs>(this.objectTree_SaveSelectedObjectsCompleted);
            this.objectTree.SaveSelectecObjectsProgress += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsProgressEventArgs>(this.objectTree_SaveSelectecObjectsProgress);
            // 
            // loadTab
            // 
            this.loadTab.Controls.Add(this.filesTree);
            this.loadTab.Controls.Add(this.loadButton);
            this.loadTab.ImageKey = "Import";
            this.loadTab.Location = new System.Drawing.Point(4, 23);
            this.loadTab.Name = "loadTab";
            this.loadTab.Padding = new System.Windows.Forms.Padding(3);
            this.loadTab.Size = new System.Drawing.Size(340, 319);
            this.loadTab.TabIndex = 1;
            this.loadTab.Text = "Load";
            this.loadTab.UseVisualStyleBackColor = true;
            // 
            // filesTree
            // 
            this.filesTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesTree.Location = new System.Drawing.Point(6, 6);
            this.filesTree.Name = "filesTree";
            this.filesTree.Size = new System.Drawing.Size(328, 278);
            this.filesTree.TabIndex = 13;
            this.filesTree.WorkingCopyPath = null;
            this.filesTree.LoadSelectedObjectsCompleted += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsCompletedEventArgs>(this.filesTree_LoadSelectedObjectsCompleted);
            this.filesTree.LoadSelectecObjectsProgress += new System.EventHandler<AccessScrCtrlUI.SelectedObjectsProgressEventArgs>(this.filesTree_LoadSelectecObjectsProgress);
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(259, 290);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 490);
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.editMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(383, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenu,
            this.OpenMenu,
            this.UpperSeparatorMenu,
            this.toolStripMenuItem2,
            this.ExitMenu});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "&File";
            // 
            // newMenu
            // 
            this.newMenu.Name = "newMenu";
            this.newMenu.Size = new System.Drawing.Size(180, 22);
            this.newMenu.Text = "&New";
            this.newMenu.Click += new System.EventHandler(this.newMenu_Click);
            // 
            // OpenMenu
            // 
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(180, 22);
            this.OpenMenu.Text = "&Open...";
            this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
            // 
            // UpperSeparatorMenu
            // 
            this.UpperSeparatorMenu.Name = "UpperSeparatorMenu";
            this.UpperSeparatorMenu.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // ExitMenu
            // 
            this.ExitMenu.Name = "ExitMenu";
            this.ExitMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitMenu.Size = new System.Drawing.Size(180, 22);
            this.ExitMenu.Text = "E&xit";
            this.ExitMenu.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // editMenu
            // 
            this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileMenu});
            this.editMenu.Enabled = false;
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(39, 20);
            this.editMenu.Text = "&Edit";
            // 
            // profileMenu
            // 
            this.profileMenu.Name = "profileMenu";
            this.profileMenu.Size = new System.Drawing.Size(180, 22);
            this.profileMenu.Text = "&Profile...";
            this.profileMenu.Click += new System.EventHandler(this.profileMenu_Click);
            // 
            // backgroundAttach
            // 
            this.backgroundAttach.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundAttach_DoWork);
            this.backgroundAttach.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundAttach_RunWorkerCompleted);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 512);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.selectFolderButton);
            this.Controls.Add(this.workingCopyTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.selectFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFrm";
            this.Text = "Access Source Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.saveTab.ResumeLayout(false);
            this.loadTab.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button selectFileButton;
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
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel infoToolStrip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem newMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenMenu;
        private System.Windows.Forms.ToolStripSeparator UpperSeparatorMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ExitMenu;
        private System.ComponentModel.BackgroundWorker backgroundAttach;
        private System.Windows.Forms.ToolStripMenuItem editMenu;
        private System.Windows.Forms.ToolStripMenuItem profileMenu;
    }
}

