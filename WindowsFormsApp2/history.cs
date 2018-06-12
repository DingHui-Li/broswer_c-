using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class history : Form
    {
        private Form1 form1;
        public history(Point p,int h, Form1 f)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Location =new Point(p.X,p.Y+h);
            this.Deactivate += new EventHandler(history_Deactivate);
            Dictionary<string,string> his = Program.getHistory();
            foreach (var i in his)
            {
                listBox1.Items.Add(i.Key);
            }
            form1 = f;
        }
        private void history_Deactivate(object sender, EventArgs e)//失去焦点后关闭
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Program.hisclear();
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> his = Program.getHistory();
            if (listBox1.SelectedItem != null)
            {
                string title = listBox1.SelectedItem.ToString();
                form1.selectHis(his[title]);
            }
            this.Close();
        }

        private void history_Paint(object sender, PaintEventArgs e)
        {
          
        }
    }
}
