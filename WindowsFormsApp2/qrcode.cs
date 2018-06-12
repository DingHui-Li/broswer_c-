using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class qrcode : Form
    {
        MainForm mf;
        public qrcode(Point p,MainForm mf)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Location = new Point(p.X - this.Right + 20, p.Y + 25);
            this.Deactivate += new EventHandler(qrcode_Deactivate);
            this.mf = mf;
        }
        public void set(string url)
        {
            pictureBox1.ImageLocation = url;
        }
        private void qrcode_Deactivate(object sender, EventArgs e)//失去焦点后关闭
        {
            this.Dispose();
        }

        private void qrcode_Load(object sender, EventArgs e)
        {
            this.BackColor = Program.bgColor;
        }
    }
}
