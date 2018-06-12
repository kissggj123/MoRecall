using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiRecall.patch
{
    public partial class changelogs : Form
    {
        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);


        public changelogs()
        {
            InitializeComponent();
        }

        private void changelogs_Load(object sender, EventArgs e)
        {
            this.textBox1.Select(0, 0);
            this.textBox1.Focus();
        }

        void textBox1_GotFocus(object sender, EventArgs e)
        {
            HideCaret((sender as TextBox).Handle);
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret((sender as TextBox).Handle);
        }
    }
}

   