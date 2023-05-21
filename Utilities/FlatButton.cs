using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public partial class FlatButton : Button
    {
        Color defaultColor = Color.FromArgb(68, 68, 68);

        public FlatButton()
        {
            this.Font = new Font("돋움", 20.25f, FontStyle.Bold);
            this.ForeColor = Color.White;
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.FromArgb(68, 68, 68);


            this.FlatAppearance.BorderColor = defaultColor;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.MouseOverBackColor = Color.Red;
        }
    }
}