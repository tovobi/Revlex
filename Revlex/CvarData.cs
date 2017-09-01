using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public struct CvarData
	{
		public string Name { get; set; }
		public float Cd { get; set; }
		public uint Id { get; set; }
		public CvarData(string _name, float _cd, uint _id)
		{
			this.Name = _name;
			this.Cd = _cd;
			this.Id = _id;
		}
	}
}
