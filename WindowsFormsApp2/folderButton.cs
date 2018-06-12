using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class folderButton : UserControl
    {
        public folderButton()
        {
            InitializeComponent();
        }
        public override string Text
        {
            set { button1.Text = value; }
            get { return button1.Text; }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

    }
}
