using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class Cursor
    {
        private int type = 0;
        public static Cursor Current { get; set; }

        public Cursor()
        {

        }

        public Cursor(Cursors.CursorTypes t)
        {
            type = (int)t;
        }
    }

    public static class Cursors
    {
        public enum CursorTypes
        {
            Default = 0, //default
            SizeAll,
            SizeNS,
            SizeWE,
            SizeNWSE,
            SizeNESW,
            No
        }
        public static Cursor Default
        {
            get { return new Cursor(CursorTypes.Default); }
        }
        public static Cursor SizeNS
        {
            get { return new Cursor(CursorTypes.SizeNS); }
        }
        public static Cursor SizeWE
        {
            get { return new Cursor(CursorTypes.SizeWE); }
        }
        public static Cursor SizeAll
        {
            get { return new Cursor(CursorTypes.SizeAll); }
        }
        public static Cursor SizeNWSE
        {
            get { return new Cursor(CursorTypes.SizeNWSE); }
        }
        public static Cursor SizeNESW
        {
            get { return new Cursor(CursorTypes.SizeNESW); }
        }
        public static Cursor No
        {
            get { return new Cursor(CursorTypes.No); }
        }
    }
}
