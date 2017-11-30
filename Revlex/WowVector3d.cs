using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revlex
{
	public class WowVector3d
	{
		public double x { get; set; }
		public double y { get; set; }
		public double z { get; set; }

		public WowVector3d(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
        public WowVector3d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double Distance(WowVector3d vector)
		{
			return Math.Sqrt(Math.Pow((this.x - vector.x), 2) + Math.Pow((this.y - vector.y), 2) + Math.Pow((this.z - vector.z), 2));
		}
        public double Distance2d(WowVector3d vector)
        {
            return Math.Sqrt(Math.Pow((this.x - vector.x), 2) + Math.Pow((this.y - vector.y), 2));
        }


        public static WowVector3d operator +(WowVector3d vector1, WowVector3d vector2)
		{
			WowVector3d vector3 = new WowVector3d(0, 0, 0);
			vector3.x = vector1.x + vector2.x;
			vector3.y = vector1.y + vector2.y;
			vector3.z = vector1.z + vector2.z;
			return vector3;
		}
		public static WowVector3d operator -(WowVector3d vector1, WowVector3d vector2)
		{
			WowVector3d vector3 = new WowVector3d(0, 0, 0);
			vector3.x = vector1.x - vector2.x;
			vector3.y = vector1.y - vector2.y;
			vector3.z = vector1.z - vector2.z;
			return vector3;
		}

	}
}
