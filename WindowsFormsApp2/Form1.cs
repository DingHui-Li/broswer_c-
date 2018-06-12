using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public static List<Button> btnlist = new List<Button>();
        public List<ComboBox> cmlist = new List<ComboBox>();
        public List<Control> controls = new List<Control>();
        public MainForm mf;
        public Form1(MainForm mf)
        {
            InitializeComponent();
            this.TopLevel = false;
            webBrowser1.Navigate(Environment.CurrentDirectory + "\\data\\Home.html");
            webBrowser1.Navigate(Environment.CurrentDirectory + "\\data\\Home.html");
            webBrowser1.ScriptErrorsSuppressed = true;
            this.mf = mf;
            addBookMark();
        }
        public void resize(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }
        public Form1(string url, MainForm mf)
        {
            InitializeComponent();
            this.TopLevel = false;
            this.mf = mf;
            webBrowser1.Navigate(url);
            webBrowser1.ScriptErrorsSuppressed = true;
            addBookMark();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)//搜索/转到
        {
            if (e.KeyCode == Keys.Enter)
            {
                search();
            }
        }
        private void search()
        {
            string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            if (textBox1.Text != null)
            {
                if (Regex.IsMatch(textBox1.Text, Url))
                {
                    webBrowser1.Navigate(textBox1.Text);
                }
                else
                {
                    string Search = selectSearchEngine(Program.searchEngine);
                    if (Search != null) webBrowser1.Navigate(Search + textBox1.Text);
                }
            }
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            if (Program.homeButton == "new")
                webBrowser1.Navigate(Environment.CurrentDirectory + "\\data\\home.html");
            else
            {
                if (Program.homeUrl != null&& Program.homeUrl!=string.Empty)
                    webBrowser1.Navigate(Program.homeUrl);
                else webBrowser1.Navigate(Environment.CurrentDirectory + "\\data\\home.html");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            addBookMark();
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)//在新窗口中打开链接
        {
            e.Cancel = true;
            // string url=webBrowser1.Document.ActiveElement.GetAttribute("href");
            string url = webBrowser1.StatusText;
            if (url != null && url != string.Empty)
            {
                Form1 form = new Form1(url, mf);
                mf.addPage(form);
            }
        }
   
        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Dictionary<string, string> his = Program.getHistory();
            Dictionary<string, string> bm = Program.getBookMark();
            if (his != null)//获取历史记录最后一个即当前页面
            {
                if (his.Count >= 1)
                {
                    var item = his.ElementAt(his.Count - 1);
                    if (!bm.ContainsKey(item.Key))
                    {
                        Point p = PointToScreen(button3.Location);
                        new bookMark(p,this,item.Key,item.Value).Show(); 
                    }
                    else
                    { MessageBox.Show("已存在", "提示"); }
                }
              else MessageBox.Show("不能添加！","警告");
            }
           
           
        }
        public void addbook(string title,string url)//bookMark窗口调用
        {
            if (title != String.Empty)
            {
                Program.setBookMark(title, url);
                addBookMark();
            }
        }
     
        private void Btn_Click(object sender, EventArgs e)
        {
            //webBrowser1.Navigate();
        }
        public void addBookMark()//创建书签按钮及事件------------------------------------------------------------
        {
            Dictionary<string, string> bm = Program.getBookMark();
            panel1.Controls.Clear();
            if (bm != null)//创建书签按钮
            {
               controls.Clear();
                for (int i = 0; i < bm.Count; i++)
                {
                    var item = bm.ElementAt(i);
                    Button btn = new Button();
                    btn.BackColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Text = item.Key;
                    toolTip1.SetToolTip(btn,item.Key+"\n"+item.Value);
                   // btnlist.Add(btn);
                    controls.Add(btn);
                }
            }
            addFolder();//创建文件夹按钮
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("打开");
            cms.Items.Add("新标签页中打开");
            cms.Items.Add("删除");
            cms.ItemClicked += Cms_itemClick;
            ContextMenuStrip cms2 = new ContextMenuStrip();
            cms2.Items.Add("删除");
            cms2.ItemClicked += Cms2_ItemClicked;
            for (int i = 0; i < controls.Count; i++) //将按钮添加
            {
                if (i != 0) controls[i].Location = new Point(controls[i - 1].Right, controls[1 - 1].Top);
                panel1.Controls.Add(controls[i]);
                if (controls[i].GetType().ToString() == "System.Windows.Forms.Button")
                {
                    controls[i].Click += new EventHandler(buttons_Click);//添加按钮事件---跳转到相应url
                    controls[i].ContextMenuStrip = cms;
                }
                else
                {
                   controls[i].Click +=new EventHandler(folder_Click);
                    controls[i].ContextMenuStrip = cms2;
                }
            }
        }
        private void Cms2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            folderButton fbtn = ((folderButton)((ContextMenuStrip)sender).SourceControl); ;
            Program.delfolder(fbtn.Text);
            addBookMark();
        }

        private void folder_Click(object sender, EventArgs e)
        {
            folderButton fbtn = (folderButton)sender;
            Dictionary<string, Dictionary<string, string>> dict = Program.getFolder();
            Dictionary<string,string> dic = dict[fbtn.Text];
            new bookFolder(dic, PointToScreen(fbtn.Location),webBrowser1,fbtn.Text,mf).Show();
        }

        private void Cms_itemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            Button btn = ((Button)((ContextMenuStrip)sender).SourceControl);//获取点击源
            string url = Program.geturl(btn.Text);
            if (e.ClickedItem.Text == "打开")
            {
                webBrowser1.Navigate(url);
            }
            else if (e.ClickedItem.Text == "新标签页中打开")
            {
                Form1 form = new Form1(url,mf);
                mf.addPage(form);
            }
            else if (e.ClickedItem.Text == "删除")
            {
                Program.delbookMark(btn.Text);
            }
                
            addBookMark();
        }
        private void buttons_Click(object sender, EventArgs e)
        {
            addBookMark();
            Button b = (Button)sender;
            Dictionary<string, string> bm = Program.getBookMark();
            if (bm.ContainsKey(b.Text))
            {
                string url = bm[b.Text];
                webBrowser1.Navigate(url);
            }
        }
        public void addFolder()
        {
            Dictionary<string, Dictionary<string, string>> folder = Program.getFolder();
            foreach (var i in folder)
            {
                folderButton cm = new folderButton();
                cm.Text = i.Key;
                controls.Add(cm);
            }
        }//--------------------------------------------------------------------------------------------------

        private void button6_Click(object sender, EventArgs e)
        {
            int height = button6.Height;
            Point p=PointToScreen(button6.Location);
            history hs = new history(p, height,this);
            hs.Show();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Dictionary<string, string> his = Program.getHistory();//页面加载完成后添加到历史记录
            string url=webBrowser1.Url.ToString();
            string title = webBrowser1.Document.Title;
            if ((url.IndexOf("Home.html")) < 0)
            {
                textBox1.Text = url;
                button7.Visible = true;
                if (title != null && title != "")
                {
                    string time = System.DateTime.Now.ToString();
                    mf.set(title);
                    title += " " + time;
                    if (!his.ContainsKey(title))
                    {
                        Program.setHistory(title, url);
                    }
                }
            }
            else
            {
                mf.set("Home");
                textBox1.Text = "http://";
                button7.Visible = false;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.writeData();
        }
        public void selectHis(string url)
        {
            webBrowser1.Navigate(url);
        }
     

        private void button7_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            if (url != null )
            {
                if (Regex.IsMatch(url, Url))
                {
                    qrcode qc = new qrcode(PointToScreen(button7.Location), mf);
                    qc.Show();
                    downQRcode(url, qc);
                }
            }
        }
        public void downQRcode(string url,qrcode qc)//生成二维码---------------------------------------------------------------
        {
           
            WebBrowser web = new WebBrowser();
            web.ScriptErrorsSuppressed = true;
            web.Navigate("http://www.wwei.cn/");
            web.DocumentCompleted +=(sender,e)=> Web_DocumentCompleted(web,url,qc);
        }

        private void Web_DocumentCompleted(WebBrowser web, string url,qrcode qc)
        {
            web.Document.GetElementById("text_qrtext").SetAttribute("value",url);
            //panel1.Controls.Add(web);
            web.Document.GetElementById("wwei_qrcode_create").InvokeMember("Click");
            timer1.Enabled = true;
            timer1.Tick += (sender, e) => timer1_Tick(web,qc);
        }

        private void timer1_Tick(WebBrowser web,qrcode qc)
        {
            timer1.Enabled = false;
           // string imgpath = "data//img.png";
           // if (File.Exists(imgpath)) File.Delete(imgpath);
            string url = web.Document.GetElementById("view-qrcode-img").Children[0].GetAttribute("src");
           // WebClient wc = new WebClient();
           // wc.DownloadFile(url, imgpath);
           // wc.Dispose();
            qc.set(url);
        }//------------------------------------------------------------------------------------------------------

        private void 打开新标签页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mf.newHome();
        }

        private void 打开新窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           new MainForm(false).Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mf.Dispose();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("V2018.5.16");
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting setform = new setting();
            mf.addPage(setform);
        }
        public string selectSearchEngine(string searchEngine)
        {
            switch (searchEngine)
            {
                case "百度":return "www.baidu.com/s?wd="; 
                case "搜狗":return "https://www.sogou.com/web?query="; 
                case "360":return "https://www.so.com/s?ie=utf-8&fr=none&src=360sou_newhome&q="; 
                case "必应":return "https://cn.bing.com/search?q="; 
                default:return null;
            }
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void 粘贴并搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
            search();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
           textBox1.Text = "等待...";
        }
        public string getUrl()
        {
            return textBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Program.bgColor;
        }
    }
}
