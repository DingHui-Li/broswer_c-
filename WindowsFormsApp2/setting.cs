using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
            this.TopLevel = false;
            comboBox1.SelectedItem = Program.searchEngine;
            switch (Program.startUp)
            {
                case "new":radioButton1.Checked = true;
                     break;
                case "keep":radioButton2.Checked = true;
                     break;
                case "custom":radioButton3.Checked = true;
                     break;
            }
            if (Program.homeButton == "new") radioButton4.Checked = true;
            else radioButton5.Checked = true;
            if (Program.customUrl != null&& Program.customUrl!=string.Empty)
            {
                label6.Text = Program.customUrl;
                textBox1.Visible = false;
                panel5.Visible = true;
            }
            if (Program.homeUrl != null&&Program.homeUrl!=string.Empty)
            {
                label7.Text = Program.homeUrl;
                textBox2.Visible = false;
                panel6.Visible = true;
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//搜索引擎设置
        {
            Program.searchEngine = comboBox1.SelectedItem.ToString();
        }
        //============================启动时设置====================================
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            selectStartMode();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            selectStartMode();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            selectStartMode();
        }
        private void selectStartMode()
        {
            if (radioButton1.Checked) Program.startUp = "new";
            else if (radioButton2.Checked) Program.startUp = "keep";
            else if (radioButton3.Checked) Program.startUp = "custom";
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                if (Regex.IsMatch(textBox1.Text, Url))
                {
                    Program.customUrl = textBox1.Text;
                    label6.Text = textBox1.Text;
                    textBox1.Visible = false;
                    panel5.Visible = true;
                }
                else MessageBox.Show("不符合格式");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = label6.Text;
            textBox1.Visible = true;
            panel5.Visible = false;
        }
        //===================================================================================
        //========================home按钮设置============================================
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                Program.homeButton = "new";
                label5.Text = radioButton4.Text;
            }
            else if (radioButton5.Checked)
            {
                label5.Text = "自定义";
                Program.homeButton = "custom";
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                if (Regex.IsMatch(textBox2.Text, Url))
                {
                    Program.homeUrl = textBox2.Text;
                    label7.Text = textBox2.Text;
                    textBox2.Visible = false;
                    panel6.Visible = true;
                    label5.Text = textBox2.Text;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = label7.Text;
            textBox2.Visible = true;
            panel6.Visible = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                Program.homeButton = "new";
                label5.Text = radioButton4.Text;
            }
            else if (radioButton5.Checked)
            {
                label5.Text ="自定义";
                Program.homeButton = "custom";
            }
        }

        private void setting_Load(object sender, EventArgs e)
        {
            this.Focus();
            //this.BackColor = Program.bgColor;
           // panel1.BackColor = Program.bgColor;
           // panel2.BackColor = Program.bgColor;
           // panel4.BackColor = Program.bgColor;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,this.ClientRectangle,Program.bgColor,ButtonBorderStyle.Solid);
            //addShadow(panel1);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,this. ClientRectangle, Program.bgColor, ButtonBorderStyle.Solid);
           // addShadow(panel2);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Program.bgColor, ButtonBorderStyle.Solid);
           // addShadow(panel4);
        }
        private void addShadow(Panel panel)
        {

            Rectangle rect = panel.DisplayRectangle;
            rect.Location = new Point(panel.Location.X, panel.Location.Y + panel.Height/2);
            Rectangle rect1 = rect;
            rect1.Location = new Point(panel.Location.X + panel.Width/2, panel.Location.Y);
            rect1.Width -= panel.Width / 2-12;
            rect.Height -= panel.Height/2-9;
            LinearGradientBrush lgb = new LinearGradientBrush(rect, Color.Black, Color.Transparent, LinearGradientMode.Vertical);
            LinearGradientBrush lgb1 = new LinearGradientBrush(rect1, Color.Black, Color.Transparent, LinearGradientMode.Horizontal);
            Graphics g = this.CreateGraphics();
            g.FillRectangle(lgb, rect);
            g.FillRectangle(lgb1, rect1);
            
        }

        private void setting_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.Clear(BackColor);
            addShadow(panel1);
            addShadow(panel2);
            addShadow(panel4);
        }
        //===================================================================================
    }
}
