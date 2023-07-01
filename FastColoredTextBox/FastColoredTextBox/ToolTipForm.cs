using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastColoredTextBoxNS
{
    public partial class ToolTipForm : Form
    {
        public int MaxToolTipWidth { get; set; }

        public ToolTipForm()
        {
            InitializeComponent();
        }

        public void Show(IWin32Window owner, string title, string text, Point p, bool isLeftAnchor)
        {
            if (!Visible) ShowInactiveTopmost(this); // Show(owner);

            lblText.MaximumSize = new Size(MaxToolTipWidth, 0);

            if (text == null)
            {
                lblTitle.Visible = false;
                lblText.Text = title;
            }
            else
            {
                lblTitle.Visible = true;
                lblTitle.MaximumSize = new Size(MaxToolTipWidth, 0);
                lblTitle.Text = title;

                lblText.Text = text;
            }

            Width = flp.Width;
            Height = flp.Height;

            Top = p.Y;
            if (isLeftAnchor)
            {
                Left = p.X;
            }
            else
            {
                Left = p.X - Width;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;

                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
             int hWnd,             // Window handle
             int hWndInsertAfter,  // Placement-order handle
             int X,                // Horizontal position
             int Y,                // Vertical position
             int cx,               // Width
             int cy,               // Height
             uint uFlags);         // Window positioning flags

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void ShowInactiveTopmost(Form frm)
        {
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST,
            frm.Left, frm.Top, frm.Width, frm.Height,
            SWP_NOACTIVATE);
        }
    }
}
