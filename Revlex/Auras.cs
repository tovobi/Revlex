using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class Auras
	{
		public string Name { get; set; }
		public uint Id { get; set; }
		public float Duration { get; set; }
		public uint Stacks { get; set; }
		public Auras(string _name, uint _id, float _duration = 1, uint _stacks = 1)
		{
			Name = _name;
			Id = _id;
			Duration = _duration;
			Stacks = _stacks;
		}
	}
}
