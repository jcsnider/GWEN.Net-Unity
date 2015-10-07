using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class SaveFileDialog
    {
        public string Title { get; set; }
        public string InitialDirectory { get; set; }
        public string DefaultExt { get; set; }
        public string Filter { get; set; }
        public bool CheckPathExists { get; set; }
        public bool Multiselect { get; set; }
        public string FileName { get; set; }
        public bool OverwritePrompt { get; set; }

        public SaveFileDialog()
        {

        }

        public DialogResult ShowDialog()
        {
            return DialogResult.OK;
        }
    }
}
