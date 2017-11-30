using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revlex
{
    public partial class CustomForm : Form
    {
        public CustomForm()
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;

            // Other stuff
        }

        protected override void OnPaintBackground(PaintEventArgs e) { /* Ignore */ }
    }
}
