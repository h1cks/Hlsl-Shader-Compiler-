using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Strafe
{
    public partial class PopupErrorBox : Form
    {
        static PopupErrorBox FormShown;

        public static void Show(string errorMessage1, string errorMessage2, string errorMessage3)
        {
            FormShown = new PopupErrorBox(errorMessage1, errorMessage2, errorMessage3);

            FormShown.BringToFront();

            FormShown.ShowDialog();
        }

        static void DisposeItem()
        {
            if (FormShown != null)
                FormShown.Close();

            FormShown = null;

        }

        public PopupErrorBox(string errorMessage1, string errorMessage2, string errorMessage3)
        {
            InitializeComponent();

            lb_errorDetails.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            lb_errorDetails.MeasureItem += Lb_errorDetils_MeasureItem;
            lb_errorDetails.DrawItem += Lb_errorDetils_DrawItem;

            lb_errorDetails.Items.Add(errorMessage1);
            lb_errorDetails.Items.Add(errorMessage3);
            lb_errorDetails.Items.Add(errorMessage2);
        }

        private void Pb_ok_Click(object sender, EventArgs e)
        {


            DisposeItem();
        }


        private void Lb_errorDetils_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lb_errorDetails.Items[e.Index].ToString(), lb_errorDetails.Font, lb_errorDetails.Width).Height;
        }

        private void Lb_errorDetils_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(lb_errorDetails.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }
    }
}
