using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public enum DialogResult
    {
        OK = 0,
    }
    public class OpenFileDialog
    {
        public string Title { get; set; }
        public string InitialDirectory { get; set; }
        public string DefaultExt { get; set; }
        public string Filter { get; set; }
        public bool CheckPathExists { get; set; }
        public bool Multiselect { get; set; }
        public string FileName { get; set; }

        public OpenFileDialog()
        {

        }

        public DialogResult ShowDialog()
        {
            return DialogResult.OK;
        }
    }
}
