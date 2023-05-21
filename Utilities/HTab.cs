using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Inspection
{
    public partial class HTab : TabControl
    {
        public HTab()
        {
            InitializeComponent();

            InitComponent();

        }
        
        //private TabAlignment _Alignment = TabAlignment.Left;
        //[Browsable(true), DefaultValue(TabAlignment.Left)]
        //public TabAlignment Alignment
        //{
        //    get { return _Alignment; }
        //    set { _Alignment = value; Invalidate(); }
        //}

        private void InitComponent()
        {
            this.DrawItem += new DrawItemEventHandler(HTab_DrawItem);
            //this.SizeChanged += new EventHandler(HTab_SizeChanged);

            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.SizeMode = TabSizeMode.Fixed;
            //this.Alignment = TabAlignment.Left;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            
        }

        //private Color _backColor;
        //[BrowsableAttribute(true)]
        //[DefaultValue("Color.Transparent")]
        //public override Color BackColor
        //{
        //    get
        //    {
        //        return _backColor;
        //    }
        //    set
        //    {
        //        _backColor = value;
        //    }
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    //base.OnPaintBackground(e);


        //    GraphicsPath graphPath = new GraphicsPath();
        //    graphPath.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
        //    e.Graphics.FillPath(Brushes.Transparent, graphPath);

        //}

        void HTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = this.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = this.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                //_textBrush = new SolidBrush(Color.Black);
                _textBrush = new SolidBrush(this.TabPages[e.Index].ForeColor);
                g.FillRectangle(Brushes.Wheat, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font. 
            Font _tabFont = ((TabControl)sender).Font;

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
