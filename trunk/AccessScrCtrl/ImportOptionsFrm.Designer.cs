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
            this.allowDataLostCheckBox = new System.Windows.Forms.CheckBox();
            this.tablesList = new System.Windows.Forms.CheckedListBox();
            this.overwritePromptCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            // allowDataLostCheckBox
            // 
            this.allowDataLostCheckBox.AutoSize = true;
            this.allowDataLostCheckBox.Location = new System.Drawing.Point(26, 81);
            this.allowDataLostCheckBox.Name = "allowDataLostCheckBox";
            this.allowDataLostCheckBox.Size = new System.Drawing.Size(142, 17);
            this.allowDataLostCheckBox.TabIndex = 2;
            this.allowDataLostCheckBox.Text = "Allow data loss for tables";
            this.toolTip1.SetToolTip(this.allowDataLostCheckBox, "If table is not empty, if allow datalost is not checked, table structure is not m" +
        "odified during import operation");
            this.allowDataLostCheckBox.UseVisualStyleBackColor = true;
            this.allowDataLostCheckBox.CheckedChanged += new System.EventHandler(this.allowDataLostCheckBox_CheckedChanged);
            // 
            // tablesList
            // 
            this.tablesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablesList.Enabled = false;
            this.tablesList.FormattingEnabled = true;
            this.tablesList.Location = new System.Drawing.Point(35, 104);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(378, 94);
            this.tablesList.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tablesList, "Please, check those tables that you allow to loss data");
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
            this.okButton.Location = new System.Drawing.Point(328, 12);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(85, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(328, 41);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // ImportOptionsFrm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 208);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.overwritePromptCheckBox);
            this.Controls.Add(this.tablesList);
            this.Controls.Add(this.allowDataLostCheckBox);
            this.Controls.Add(this.deleteNotLoadedCheckBox);
            this.Controls.Add(this.overwriteCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 244);
            this.Name = "ImportOptionsFrm";
            this.Text = "Import options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox overwriteCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox deleteNotLoadedCheckBox;
        private System.Windows.Forms.CheckBox allowDataLostCheckBox;
        private System.Windows.Forms.CheckedListBox tablesList;
        private System.Windows.Forms.CheckBox overwritePromptCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}