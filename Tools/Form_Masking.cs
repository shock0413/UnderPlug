using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools
{
    public partial class Form_Masking : Form
    {
        public Form_Masking()
        {
            InitializeComponent();
        }

        // 검사결과 화면 출력 여부 결정
        public Cognex.VisionPro.ICogImage IMAGE
        {
            get
            {
                return this.cogImageMaskEditV21.Image;
            }
        }

        // 검사결과 화면 출력 여부 결정
        public Cognex.VisionPro.ICogImage MASKIMAGE
        {
            get
            {
                return this.cogImageMaskEditV21.MaskImage;
            }
        }

        public Form_Masking(Cognex.VisionPro.CogImage8Grey display)
        {
            InitializeComponent();
            cogImageMaskEditV21.Image = display;
        }

        private void Form_Masking_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
