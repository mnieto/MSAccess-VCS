using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessScrCtrl.Helpers {
    class CommonDialogs {
        public static OpenFileDialog OpenAccess(string selectedFile) {
            var result = new OpenFileDialog {
                Title = Properties.Resources.OpenAccessFile,
                Filter = Properties.Resources.OpenAccessFilter,
                FileName = selectedFile
            };
            return result;
        }

        public static OpenFileDialog OpenProfile(string selectedFile) {
            var result = new OpenFileDialog {
                Title = Properties.Resources.OpenProfileFile,
                Filter = Properties.Resources.OpenProfileFilter,
                FileName = selectedFile
            };
            return result;
        }
    }
}
