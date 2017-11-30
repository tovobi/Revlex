using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex.RadarOverlay
{
    public class RadarEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color IconColor;
        public Bitmap IconType;
        public RadarEntity(int _x, int _y, System.Drawing.Color _iconColor, Bitmap _iconType)
        {
            X = _x;
            Y = _y;
            IconColor = _iconColor;
            IconType = _iconType;
        }
    }
}

