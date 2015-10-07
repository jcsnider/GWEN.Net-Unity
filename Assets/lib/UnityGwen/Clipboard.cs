using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public static class Clipboard
    {
        private static string value = null;

        public static bool ContainsText()
        {
            return value != null;
        }

        public static string GetText()
        {
            if (ContainsText())
            {
                return value;
            }
            return "";
        }

        public static void SetText(string text)
        {
            value = text;
        }
    }
}
