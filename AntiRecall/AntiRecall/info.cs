using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiRecall.patch
{
    public partial class info : Form
    {
        public info()
        {
            InitializeComponent();
        }

        private void info_Load(object sender, EventArgs e)
        {
            Version ApplicationVersion = new Version(Application.ProductVersion);
            string ver = Application.ProductVersion.ToString();
            label4.Text = "版本@"+ver;   
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //System.Windows.MessageBox.Show("感谢某只小奶喵的吐槽工具的UI丑陋","美工是不可能美工的这辈子都不可能会美工的");
        }
    }
}
