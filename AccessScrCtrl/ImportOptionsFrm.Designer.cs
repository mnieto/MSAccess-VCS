namespace AccessScrCtrl {
    partial class ImportOptionsFrm {
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
            this.overwriteCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteNotLoadedCheckBox = new System.Windows.Forms.CheckBox();
            this.overwritePromptCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tablesGrid = new AccessScrCtrlUI.ExpressionGrid();
            this.excludesGrid = new AccessScrCtrlUI.ExpressionGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // overwriteCheckBox
            // 
            this.overwriteCheckBox.AutoSize = true;
            this.overwriteCheckBox.Location = new System.Drawing.Point(7, 35);
            this.overwriteCheckBox.MinimumSize = new System.Drawing.Size(156, 17);
            this.overwriteCheckBox.Name = "overwriteCheckBox";
            this.overwriteCheckBox.Size = new System.Drawing.Size(156, 17);
            this.overwriteCheckBox.TabIndex = 0;
            this.overwriteCheckBox.Text = "Overwrite existing database";
            this.toolTip1.SetToolTip(this.overwriteCheckBox, "Delete current database and create a new one. Then, import the selected objects");
            this.overwriteCheckBox.UseVisualStyleBackColor = true;
            this.overwriteCheckBox.CheckedChanged += new System.EventHandler(this.overwriteCheckBox_CheckedChanged);
            // 
            // deleteNotLoadedCheckBox
            // 
            this.deleteNotLoadedCheckBox.AutoSize = true;
            this.deleteNotLoadedCheckBox.Location = new System.Drawing.Point(26, 58);
            this.deleteNotLoadedCheckBox.Name = "deleteNotLoadedCheckBox";
            this.deleteNotLoadedCheckBox.Size = new System.Drawing.Size(147, 17);
            this.deleteNotLoadedCheckBox.TabIndex = 1;
            this.deleteNotLoadedCheckBox.Text = "Delete not loaded objects";
            this.toolTip1.SetToolTip(this.deleteNotLoadedCheckBox, "Existing objects in the database are deleted if they are not imported. Imported o" +
        "bjects will overwrite existing objects");
            this.deleteNotLoadedCheckBox.UseVisualStyleBackColor = true;
            // 
            // overwritePromptCheckBox
            // 
            this.overwritePromptCheckBox.AutoSize = true;
            this.overwritePromptCheckBox.Checked = true;
            this.overwritePromptCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overwritePromptCheckBox.Location = new System.Drawing.Point(7, 12);
            this.overwritePromptCheckBox.Name = "overwritePromptCheckBox";
            this.overwritePromptCheckBox.Size = new System.Drawing.Size(163, 17);
            this.overwritePromptCheckBox.TabIndex = 4;
            this.overwritePromptCheckBox.Text = "Prompt for overwrited objects";
            this.toolTip1.SetToolTip(this.overwritePromptCheckBox, "Display a confirmation list of objects to be overwritten");
            this.overwritePromptCheckBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(455, 12);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(85, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Visible = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(455, 41);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(26, 95);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tablesGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.excludesGrid);
            this.splitContainer1.Size = new System.Drawing.Size(513, 163);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 10;
            // 
            // tablesGrid
            // 
            this.tablesGrid.Caption = "Allow data lost in these tables";
            this.tablesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesGrid.Location = new System.Drawing.Point(0, 0);
            this.tablesGrid.Name = "tablesGrid";
            this.tablesGrid.Size = new System.Drawing.Size(252, 163);
            this.tablesGrid.TabIndex = 0;
            // 
            // excludesGrid
            // 
            this.excludesGrid.Caption = "Do not import objects that match with these";
            this.excludesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excludesGrid.Location = new System.Drawing.Point(0, 0);
            this.excludesGrid.Name = "excludesGrid";
            this.excludesGrid.Size = new System.Drawing.Size(257, 163);
            this.excludesGrid.TabIndex = 0;
            // 
            // ImportOptionsFrm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(551, 270);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.overwritePromptCheckBox);
            this.Controls.Add(this.deleteNotLoadedCheckBox);
            this.Controls.Add(this.overwriteCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 244);
            this.Name = "ImportOptionsFrm";
            this.Text = "Import options";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox overwriteCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox deleteNotLoadedCheckBox;
        private System.Windows.Forms.CheckBox overwritePromptCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private AccessScrCtrlUI.ExpressionGrid tablesGrid;
        private AccessScrCtrlUI.ExpressionGrid excludesGrid;
    }
}