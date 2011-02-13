namespace AccessScrCtrlUI {
    partial class ObjectTree {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tree = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toogleNodesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.CheckBoxes = true;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(226, 274);
            this.tree.TabIndex = 1;
            this.tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            this.tree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllMenu,
            this.unselectAllMenu,
            this.toogleNodesMenu});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(147, 70);
            // 
            // selectAllMenu
            // 
            this.selectAllMenu.Name = "selectAllMenu";
            this.selectAllMenu.Size = new System.Drawing.Size(146, 22);
            this.selectAllMenu.Text = "&Select all";
            this.selectAllMenu.Click += new System.EventHandler(this.selectAllMenu_Click);
            // 
            // unselectAllMenu
            // 
            this.unselectAllMenu.Name = "unselectAllMenu";
            this.unselectAllMenu.Size = new System.Drawing.Size(146, 22);
            this.unselectAllMenu.Text = "&Unselect all";
            this.unselectAllMenu.Click += new System.EventHandler(this.unselectAllMenu_Click);
            // 
            // toogleNodesMenu
            // 
            this.toogleNodesMenu.Name = "toogleNodesMenu";
            this.toogleNodesMenu.Size = new System.Drawing.Size(146, 22);
            this.toogleNodesMenu.Text = "&Toogle nodes";
            this.toogleNodesMenu.Click += new System.EventHandler(this.toogleNodesMenu_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // ObjectTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tree);
            this.Name = "ObjectTree";
            this.Size = new System.Drawing.Size(226, 274);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem selectAllMenu;
        private System.Windows.Forms.ToolStripMenuItem unselectAllMenu;
        private System.Windows.Forms.ToolStripMenuItem toogleNodesMenu;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}
