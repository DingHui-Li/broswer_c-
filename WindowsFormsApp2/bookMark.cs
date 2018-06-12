using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class bookMark : Form
    {
        Form1 form;
        string url;
        public bookMark(Point p,Form1 f,string title,string url)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Deactivate += new EventHandler(bookMark_Deactivate);
            form = f;
            this.Location = new Point(p.X-this.Right+20,p.Y+20);
            textBox1.Text = title;
            this.url = url;
            Dictionary<string, Dictionary<string, string>> folder = Program.getFolder();
            foreach (var i in folder)
            {
                comboBox1.Items.Add(i.Key);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void bookMark_Deactivate(object sender, EventArgs e)//失去焦点后关闭
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, string>> folder = Program.getFolder();
            if ( comboBox1.Text!= String.Empty)
            {
                string title = textBox1.Text;
                string foldername = comboBox1.Text.ToString();
                if (foldername == "书签栏")
                {
                    form.addbook(title, url);
                }
                else
                {
                    if (!folder.ContainsKey(foldername))//若文件夹不存在则创建后添加书签
                    {
                        Dictionary<string, string> temp = new Dictionary<string, string>();
                        temp.Add(title, url);
                       // folder.Add(foldername, temp);
                        Program.setFolder(foldername,temp);
                    }
                    else//若文件夹存在则将书签加入相应文件夹
                    {
                        Dictionary<string, string> temp = Program.getfolderbook(foldername);
                        if (!temp.ContainsKey(title))
                        {
                            temp.Add(title, url);
                            Program.setFolder(foldername, temp);
                        }
                        else MessageBox.Show("已存在");
                    }
                }
            }
            form.addBookMark();
            this.Dispose();
        }

        private void bookMark_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Program.bgColor;
        }

        private void bookMark_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = panel2.DisplayRectangle;
             rect.Location = panel2.Location;
            LinearGradientBrush lgb = new LinearGradientBrush(rect, Color.Black, Color.Transparent, LinearGradientMode.Vertical);
            Graphics g = this.CreateGraphics();
            g.FillRectangle(lgb, rect);
           
        }
    }
}
