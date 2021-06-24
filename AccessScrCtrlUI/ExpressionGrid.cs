using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessScrCtrlUI {
    public partial class ExpressionGrid : UserControl {
        public ExpressionGrid() {
            InitializeComponent();
        }

        [Browsable(true)]
        public string Caption {
            get => captionLabel.Text;
            set => captionLabel.Text = value;
        }

        public void AddValues(IEnumerable<string> expressions) {
            foreach(string expression in expressions) {
                var row = (DataGridViewRow)gridView.RowTemplate.Clone();
                row.CreateCells(gridView, expression);
                gridView.Rows.Add(row);
            }
        }

        public IEnumerable<string>GetValues() {
            return gridView.Rows.
                Cast<DataGridViewRow>().
                Select(x => x.Cells[0].ToString());
        }
    }
}
