
namespace AccessScrCtrlUI {
    partial class ExpressionGrid {
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
            this.gridView = new System.Windows.Forms.DataGridView();
            this.ExpressionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.captionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // gridView
            // 
            this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExpressionColumn});
            this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView.Location = new System.Drawing.Point(0, 13);
            this.gridView.Name = "gridView";
            this.gridView.Size = new System.Drawing.Size(334, 179);
            this.gridView.TabIndex = 0;
            // 
            // ExpressionColumn
            // 
            this.ExpressionColumn.HeaderText = "Expression";
            this.ExpressionColumn.Name = "ExpressionColumn";
            this.ExpressionColumn.Width = 180;
            // 
            // captionLabel
            // 
            this.captionLabel.AutoSize = true;
            this.captionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.captionLabel.Location = new System.Drawing.Point(0, 0);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(0, 13);
            this.captionLabel.TabIndex = 1;
            // 
            // ExpressionGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridView);
            this.Controls.Add(this.captionLabel);
            this.Name = "ExpressionGrid";
            this.Size = new System.Drawing.Size(334, 192);
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpressionColumn;
        private System.Windows.Forms.Label captionLabel;
    }
}
