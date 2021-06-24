using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AccessScrCtrl.Helpers {
    class ValidationHelper<T> where T : Form {

        private ErrorProvider ErrorProvider { get; set; }
        public ValidationHelper(ErrorProvider errorProvider) {
            ErrorProvider = errorProvider ?? throw new ArgumentNullException(nameof(errorProvider));
        }

        public bool Validate(Control ctrl, Func<bool> validation, string message)  {
            bool result = validation();
            if (result) {
                ErrorProvider.SetError(ctrl, string.Empty);
            } else {
                ErrorProvider.SetError(ctrl, message);
            }
            return !result;
        }
        public bool Validate(Control ctrl, Func<string> validation) {
            string result = validation();
            ErrorProvider.SetError(ctrl, result ?? string.Empty);
            return !string.IsNullOrEmpty(result);
        }



    }
}
