using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
   class Program
    {
        public static Color bgColor = Color.FromArgb(17, 147, 243);
        public static string searchEngine="百度";//搜索引擎
        public static string startUp = "new";//启动时设置
        public static string customUrl = null;//启动时打开的url
        public static string homeUrl = null;//home按钮的url
        public static string homeButton = "new";//home按钮设置
        private static Dictionary<string, string> historylist = new Dictionary<string, string>();//历史记录
        private static Dictionary<string, string> bookmark = new Dictionary<string,string>();//书签
        private static Dictionary<string, Dictionary<string,string>> folder = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            readData();
            setkernelVersion();
            setStartMode(startUp);
        }
        private static void setStartMode(string startUp)
        {
            switch (startUp)
            {
                case "new": Application.Run(new MainForm(true)); break;
                case "keep":
                    if (historylist.Count > 0)
                    {
                        var item = historylist.ElementAt(historylist.Count - 1); string url = item.Value;
                        if (url != null && url != string.Empty)
                        {
                            Application.Run(new MainForm(url, true));
                        }
                        else Application.Run(new MainForm(true));
                    }
                    else Application.Run(new MainForm(true));
                    break;
                case "custom":
                    if (customUrl != null && customUrl != string.Empty)
                    {
                        Application.Run(new MainForm(customUrl,true));
                    }
                    else Application.Run(new MainForm(true));
                    break;
            }
        }
        public static Dictionary<string, string> getHistory()//获取历史记录
        {
            return historylist;
        }
        public static void setHistory(string key,string value)//设置历史记录
        {
            historylist.Add(key, value);
        }
        public static Dictionary<string, string> getBookMark()//获取书签
        {
            return bookmark;
        }
        public static void setBookMark(string b, string url)//设置书签
        {
            if (!bookmark.ContainsKey(b))
                bookmark.Add(b, url);
            else MessageBox.Show("已存在!", "警告");
        }
        public static void delbookMark(string title)//删除书签
        {
            bookmark.Remove(title);
        }
        public static string geturl(string key)
        {
            if(bookmark.ContainsKey(key)) return bookmark[key];
            return "";
        }
        public static Dictionary<string, Dictionary<string, string>> getFolder()
        {
            return folder;
        }
        public static Dictionary<string, string> getfolderbook(string name)
        {
            return folder[name];
        }
        public static void setFolder(string name,Dictionary<string,string> dict)
        {
            if (!folder.ContainsKey(name))
            {
                folder.Add(name, dict);
            }
            else
            {
                folder[name] = dict;
            }
        }
        public static void setfolderbook(Dictionary<string, Dictionary<string, string>> dict)
        {
            folder = dict;
        }
        public static void delfolderbook(string name, string title)//删除文件夹中的书签
        {
            Dictionary<string,string> dict =folder[name];
            dict.Remove(title);
            folder[name] = dict;
        }
        public static void delfolder(string name)//删除文件夹
        {
            folder.Remove(name);
        }
        public static void writeData()//写出历史记录/书签等数据
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            FileStream file = new FileStream("data//history.data", FileMode.Create);
            StreamWriter wr = new StreamWriter(file);
            foreach (var i in historylist)
            {
                wr.WriteLine(i.Key + "url=" + i.Value);
            }
            wr.Close();
            file.Close();
            FileStream file1 = new FileStream("data//bookMark.data", FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);
            foreach (var i in bookmark)
            {
                wr1.WriteLine(i.Key + "url=" + i.Value);
            }
            wr1.Close();
            file1.Close();
            FileStream file2 = new FileStream("data//folderBookMark.data", FileMode.Create);
            StreamWriter wr2 = new StreamWriter(file2);
            foreach (var i in folder)
            {
                foreach(var j in i.Value)
                {
                    wr2.WriteLine("foldername=" + i.Key+"title="+j.Key+"url="+j.Value);
                }
            }
            wr2.Close();file2.Close();
            FileStream file3 = new FileStream("data//setting.data",FileMode.Create);
            StreamWriter wr3 = new StreamWriter(file3);
            wr3.WriteLine("bgColor:" + System.Drawing.ColorTranslator.ToHtml(bgColor));
            wr3.WriteLine("se:"+searchEngine);
            wr3.WriteLine("su:"+startUp);
            wr3.WriteLine("cu:" + customUrl);
            wr3.WriteLine("hb:"+homeButton);
            wr3.WriteLine("hu:" + homeUrl);
            wr3.Close();
        }
        public static void readData()
        {
            if (File.Exists("data//setting.data"))//读取设置
            {
                FileStream file = new FileStream("data//setting.data",FileMode.Open);
                StreamReader read = new StreamReader(file);
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    int index = -1;
                    if ((index = line.IndexOf("se:")) != -1)
                    {
                        searchEngine = line.Substring(index + 3);
                    }
                    else if ((index = line.IndexOf("su:")) != -1)
                    {
                        startUp = line.Substring(index + 3);
                    }
                    else if ((index = line.IndexOf("cu:")) != -1)
                    {
                        customUrl= line.Substring(index + 3);
                    }
                    else if ((index = line.IndexOf("hb:")) != -1)
                    {
                        homeButton = line.Substring(index + 3);
                    }
                    else if ((index = line.IndexOf("hu:")) != -1)
                    {
                        homeUrl = line.Substring(index + 3);
                    }
                    else if ((index = line.IndexOf("bgColor:")) != -1)
                    {
                        bgColor = System.Drawing.ColorTranslator.FromHtml(line.Substring(index + 8));
                    }
                }
                read.Close();
            }
            if (File.Exists("data//history.data"))//读取历史记录
            {
                FileStream file = new FileStream("data//history.data", FileMode.Open);
                StreamReader read = new StreamReader(file);
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    int cut = line.IndexOf("url=");
                    if (cut > 1)
                    {
                        string title = line.Substring(0, cut);
                        string url = line.Substring(cut + 4);
                        historylist.Add(title, url);
                    }
                }
                read.Close();
                file.Close();
            }
            if (File.Exists("data//bookMark.data"))//读取书签栏书签
            {
                FileStream file = new FileStream("data//bookMark.data", FileMode.Open);
                StreamReader read = new StreamReader(file);
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    int cut = line.IndexOf("url=");
                    if (cut > 1)
                    {
                        string title = line.Substring(0, cut);
                        string url = line.Substring(cut + 4);
                        bookmark.Add(title, url);
                    }
                }
                read.Close();
                file.Close();
            }
           if (File.Exists("data//folderBookMark.data"))//读取文件夹书签
            {
                FileStream file = new FileStream("data//folderBookMark.data", FileMode.Open);
                StreamReader read = new StreamReader(file, System.Text.Encoding.UTF8);
                string line;
                List<string> name =new List<string>();
                while ((line = read.ReadLine()) != null)
                {
                    string[] a = line.Split(new[] { "foldername=", "title=", "url=" }, StringSplitOptions.RemoveEmptyEntries);
                    string foldername=a[0];
                    string title=a[1];
                    string url=a[2];
                    if (!folder.ContainsKey(foldername))
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add(title, url);
                        setFolder(foldername, dict);
                    }
                    else
                    {
                        folder[foldername].Add(title,url);
                    }
                }
                read.Close();
                file.Close();
            }
        }
        public static void hisclear()//清空历史记录
        {
            historylist.Clear();
        }
        private static void setkernelVersion()//自动选择ie内核版本
        {
            string  version = new WebBrowser().Version.ToString();
            string v = version.Substring(0, version.IndexOf("."));
            int vers = -1;
            if (v == "11") vers = 11001;
            else if (v == "10") vers = 10001;
            else if(v=="9") vers = 9999;
            else if(v=="8") vers = 8888;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            if (key != null)
            {
                key.SetValue("WindowsFormsApp2.exe", vers, RegistryValueKind.DWord);
                key.SetValue("WindowsFormsApp2.exe", vers, RegistryValueKind.DWord);//调试运行需要加上，否则不起作用
            }

            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            if (key != null)
            {
                key.SetValue("WindowsFormsApp2.exe", vers, RegistryValueKind.DWord);
                key.SetValue("WindowsFormsApp2.vshost.exe", vers, RegistryValueKind.DWord);//调试运行需要加上，否则不起作用
            }
        }
    }
}
