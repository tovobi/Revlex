using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Revlex.RadarOverlay
{
    public class GfxPanel : Panel
    {
        public GfxPanel()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.SupportsTransparentBackColor |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
        }
    }
}
