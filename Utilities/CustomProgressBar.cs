using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public class CustomProgressBar : ProgressBar
    {
        public Color color = Color.FromArgb(43, 43, 43);
        
        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }



        protected override void OnPaint(PaintEventArgs e)
        {   
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 1;
            e.Graphics.FillRectangle(new SolidBrush(color), 2, 2, rec.Width, rec.Height);

            int minValue = 6;
            if (Value.ToString().Length > 1)
            {
                minValue += 6;
            }

            e.Graphics.DrawString(this.Value.ToString() + "%", new Font("돋움", 13), new SolidBrush(Color.Black), new PointF(this.Size.Width / 2 - minValue, this.Size.Height / 2 - 6));
               
        }

        public void SetOrangeRedColor() {
            color = Color.OrangeRed;
        }

        public void SetRedColor() {
            color = Color.Red;
        }

        public void SetNormalColor() {
            color = Color.Lime;
        }
    }

}

