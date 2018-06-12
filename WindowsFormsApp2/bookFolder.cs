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
    public partial class bookFolder : Form
    {
        Dictionary<string, string> dict;
        WebBrowser web;
        MainForm mf;
        string name;//文件夹名称
        private int selectIndex = -1;
        public bookFolder(Dictionary<string,string> dict,Point p, WebBrowser web,string name,MainForm mf)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Deactivate += new EventHandler(bookFolder_Deactivate);
            this.dict = dict;
            this.web = web;
            this.name = name;
            this.mf = mf;
            this.Location = new Point(p.X+40, p.Y + 60);
            if (dict != null)
            {
                foreach (var i in dict)
                {
                    listBox1.Items.Add(i.Key);
                }
            }
            
        }
        private void bookFolder_Deactivate(object sender, EventArgs e)//失去焦点后关闭
        {
            this.Dispose();
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            if (index != -1 && index != selectIndex)
            {
                selectIndex = index;
                listBox1.SelectedIndex = index;
                string title = this.listBox1.Items[index].ToString();
                toolTip1.SetToolTip(this.listBox1,title+"\n"+dict[title] );
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string title = listBox1.SelectedItem.ToString();
                if (dict.ContainsKey(title))
                {
                    string url = dict[title];
                    web.Navigate(url);
                }
            }
            this.Dispose();
        }

        private void 删除_Click(object sender, EventArgs e)
        {
            Program.delfolderbook(name, listBox1.SelectedItem.ToString());
            this.Dispose();
        }

        private void 新标签页中打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string title = listBox1.SelectedItem.ToString();
                string url = dict[title];
                Form1 form = new Form1(url,mf);
                mf.addPage(form);
            }
            this.Dispose();
           
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string title = listBox1.SelectedItem.ToString();
                string url = dict[title];
                web.Navigate(url);
            }
            this.Dispose();
        }
    }
}
