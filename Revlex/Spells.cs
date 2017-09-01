using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class Spells
	{
		public string Name { get; set; }
		public float Cd { get; set; }
		public uint Id { get; set; } = 0;
		public float CurrentCooldown { get; set; }
		public bool HasGlobalCooldown { get; set; }
		public bool IsDamageSpell { get; set; }
		public long LastCast = 0;

		public Spells(string _name, float _cd, uint _id, float _currentCooldown = 0f, bool _hasGlobalCooldown = true, bool _isDamageSpell = true)
		{
			Name = _name;
			Id = _id;
			Cd = _cd;
			CurrentCooldown = _currentCooldown;
			HasGlobalCooldown = _hasGlobalCooldown;
			IsDamageSpell = _isDamageSpell;
		}
	}

	public class SpellsOnCooldown
	{
		public uint Id { get; set; }
		public float Cd { get; set; }

		public SpellsOnCooldown(uint _id, float _cd)
		{
			Id = _id;
			Cd = _cd;
		}
	}

}
