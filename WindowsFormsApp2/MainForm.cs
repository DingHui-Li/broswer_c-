using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
   
    public partial class MainForm : Form
    {
        private Boolean isParent;
        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;
        private Point pt;
        public MainForm(Boolean flag)
        {
            InitializeComponent();
            isParent = flag;
            newHome();
        }
        public MainForm(TabPage tp)
        {
            InitializeComponent();
            tabControl1.TabPages.Add(tp);
        }
        public MainForm(string url,Boolean flag)
        {
            InitializeComponent();
            isParent = flag;
            Form1 form = new Form1(url, this);
            addPage(form);
        }
        /*public MainForm(Form1 form)
        {
            InitializeComponent();
            TabPage tb = new TabPage();
            tb.Controls.Add(form);
            tabControl1.TabPages.Add(tb);
            form.resize(this.Width, tabControl1.Height);
            form.Show();
            button1.Location = new Point(tabControl1.ItemSize.Width * tabControl1.TabPages.Count, 0);
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }*/
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.ItemSize = new Size(tabControl1.ItemSize.Width, 21);//选项卡大小
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            button1.BackColor = Program.bgColor;
            button2.BackColor = Program.bgColor;
            button3.BackColor = Program.bgColor;
            button4.BackColor = Program.bgColor;
            button5.BackColor = Program.bgColor;
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)//form大小自适应
        {
            Form1 form = null;
            if (tabControl1.TabPages.Count != 0)
            {
                foreach (Control i in tabControl1.SelectedTab.Controls)
                {
                    if (i is Form1) form = (Form1)i;
                }
                if (form != null) form.resize(this.Width, tabControl1.Height);
            }
        }
        public void newHome()
        {
            Form1 form = new Form1(this);
            TabPage tb = new TabPage();
            tb.Text = "Home";
            tb.Controls.Add(form);
            tabControl1.TabPages.Add(tb);
            tabControl1.SelectedTab = tb;
            form.resize(this.Width, tabControl1.Height);
            form.Show();
            button1.Location = new Point(tabControl1.ItemSize.Width * tabControl1.TabPages.Count, 0);
        }
        public void addPage(Form1 form)
        {
            TabPage tb = new TabPage();
            tb.Controls.Add(form);
            tabControl1.TabPages.Add(tb);
            form.resize(this.Width, tabControl1.Height);
            form.Show();
            tabControl1.SelectedTab = tb;
            button1.Location = new Point(tabControl1.ItemSize.Width * tabControl1.TabPages.Count, 0);
        }
        public void addPage(setting form)
        {
            TabPage tb = new TabPage();
            tb.Text = "设置";
            tb.Controls.Add(form);
            tabControl1.TabPages.Add(tb);
            form.Width = this.Width; form.Height = tabControl1.Height;
            form.Show();
            tabControl1.SelectedTab = tb;
            button1.Location = new Point(tabControl1.ItemSize.Width * tabControl1.TabPages.Count, 0);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { button3.Image = Resource2.还原; }
            else button3.Image = Resource2.最大化;
            Form1 form = null;
            setting form2 = null;
            foreach (Control i in tabControl1.SelectedTab.Controls)
            {
                if (i is Form1) form = (Form1)i;
                if (i is setting) form2 = (setting)i;
            }
            if (form != null) form.resize(this.Width, tabControl1.Height);
            if (form2 != null)
            {
                form2.Width = this.Width; form2.Height = tabControl1.Height;
            }
        }
        public void set(string text)//设置选项卡文字
        {
            tabControl1.SelectedTab.Text = text;
            tabControl1.SelectedTab.ToolTipText = text;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.writeData();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)//鼠标中键事件删除标签页
        {
            if (e.Button == MouseButtons.Middle)
            {
                Point p = new Point(e.X, e.Y);
                Rectangle recTab = new Rectangle();
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    recTab = tabControl1.GetTabRect(i);
                    if (recTab.Contains(p))
                    {
                        foreach (Control j in tabControl1.TabPages[i].Controls)
                        {
                            if (j is Form1)
                            {
                                Form1 form = (Form1)j;
                                form.Dispose();
                            }
                        }
                        tabControl1.TabPages.RemoveAt(i);
                        if (tabControl1.TabPages.Count == 0) { this.Dispose(); }
                        break;
                    }
                }
            }
            TabPage tb = tabControl1.SelectedTab;
            button1.Location = new Point(tabControl1.ItemSize.Width * tabControl1.TabPages.Count, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newHome();
        }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush gray = new SolidBrush(Color.Gray);
            SolidBrush tabBg = new SolidBrush(Color.FromArgb(253, 133, 10));
            SolidBrush lgray = new SolidBrush(Color.FromArgb(240,240,240));
            SolidBrush bgcolor = new SolidBrush(Program.bgColor);
            Pen Pbgc = new Pen(Program.bgColor,4);
            Pen pen = new Pen(Color.White, 4);
         //   Pen pen1 = new Pen(Color.FromArgb(240, 240, 240), 4);
            StringFormat sf = new StringFormat();//标签文字位置
            sf.Alignment = StringAlignment.Center;
            Rectangle rect = tabControl1.ClientRectangle;//tabcontrol主控件的工作区域
            e.Graphics.FillRectangle(bgcolor, rect);
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {

                Rectangle rec = tabControl1.GetTabRect(i);//标签
                if (i != this.tabControl1.SelectedIndex)//非选中情况下
                {
                    e.Graphics.DrawRectangle(Pbgc, rec);//边框
                    e.Graphics.FillRectangle(gray, rec);//填充
                    e.Graphics.DrawString(tabControl1.TabPages[i].Text, new Font("微软雅黑", 10), white, rec, sf);
                }
                else//选中情况下
                {
                   // e.Graphics.DrawRectangle(pen1, rec);
                    e.Graphics.FillRectangle(bgcolor, rec);
                    e.Graphics.DrawString(tabControl1.TabPages[i].Text, new Font("微软雅黑", 10), white, rec, sf);
                }
            }
            
        }
      private void tabControl1_MouseMove(object sender, MouseEventArgs e)//拖动打开新窗口
        { /* 
            if (e.Button == MouseButtons.Left)
            {
                MainForm mf;
                Point p = new Point(e.X, e.Y);
                Rectangle recTab = new Rectangle();
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    recTab = tabControl1.GetTabRect(i);
                    if (recTab.Contains(p))
                    {
                        if (tabControl1.TabPages[i].Controls[0] is Form1)
                        {
                            string url = ((Form1)tabControl1.TabPages[i].Controls[0]).getUrl();
                            string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                            if (Regex.IsMatch(url, Url))
                            {
                                mf = new MainForm(url);
                                mf.Show();
                            }
                            else
                            {
                                mf = new MainForm();
                                mf.Show();
                            }
                        }
                        break;
                    }
                }
            }*/
        }

        private void button2_Click(object sender, EventArgs e)//关闭
        {
            if (isParent)
            {
                DialogResult choice = MessageBox.Show("将关闭所有窗口", "提示", MessageBoxButtons.YesNo);
                if (choice == DialogResult.Yes) { Application.Exit(); }
                else if (choice == DialogResult.No) { }
            }
            else this.Dispose();
        }
        private void button3_Click(object sender, EventArgs e)//最大化
        {
            if (this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            else this.WindowState = FormWindowState.Normal;
        }

        private void button4_Click(object sender, EventArgs e)//最小化
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            pt = Cursor.Position;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//窗口移动
            {
                int px = Cursor.Position.X - pt.X;
                int py = Cursor.Position.Y - pt.Y;
                this.Location = new Point(this.Location.X + px, this.Location.Y + py);
                pt = Cursor.Position;
                if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal;
            }
        }
        protected override void WndProc(ref Message m)//窗口缩放
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201:                //鼠标左键按下的消息   
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标   
                    m.LParam = IntPtr.Zero; //默认值   
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内   
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        { 
            DialogResult dr= colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (colorDialog1.Color != Color.Black)
                {
                    Program.bgColor = colorDialog1.Color;
                    MessageBox.Show("重启浏览器以生效", "提示");
                }
                else MessageBox.Show("请重新选择");
            }
        }
    }
}
