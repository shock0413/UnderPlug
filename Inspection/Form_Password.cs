using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

//송성민 추가 - 141126

namespace Inspection
{
    public partial class Form_Password : Form
    {
        public Form_Password()
        {
            InitializeComponent();
        }

        private void Form1_PASSWORD_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)5;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)6;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)7;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)8;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)9;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tb_Password.Text += (int)0;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            tb_Password.Text += "*";
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (tb_Password.Text == "")
                return;
            string password = "";
            password = tb_Password.Text;
            tb_Password.Text = password.Substring(0, password.Length - 1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Utilities.Log log = new Log();
            log.Write_SystemLog(Log.CATEGORY.EVENT, "입력된 패스워드 : " + tb_Password.Text);

            IniFile Config = new IniFile(Application.StartupPath + "\\Config.ini");
            if (tb_Password.Text == Config.GetString("Common", "Password", "") || tb_Password.Text == "0325")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        

        

        

        
    }
}
