
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class WowListObject
	{
		// general properties
		public String Name { get; set; }
		public uint Level { get; set; }
		public double Distance { get; set; }
		public uint HealthPercent { get; set; }
		public short Type { get; set; }
		public uint GameObjectType { get; set; }
		public float FactionTemplate { get; set; }
		public float FactionOffset { get; set; }

		// LOOL -> you need a constructor to use a class as data source. UPDATE: we also need the fields with get and set
		public WowListObject(string _name = "-", uint _level = 0, double _distance = 0, uint _healthPercent = 0, short _type = 0, uint _gameObjectType = 0, float _factionTemplate = 0, float _factionOffset = 0)
		{
			Name = _name;
			Distance = _distance;
			HealthPercent = _healthPercent;
			Type = _type;
			GameObjectType = _gameObjectType;
			FactionTemplate = _factionTemplate;
			FactionOffset = _factionOffset;
		}


		// more specialised properties (player or mob)
		//public uint CurrentHealth = 0;
		//public uint MaxHealth = 0;
		//public uint CurrentEnergy = 0; // mana, rage and energy will all fall under energy.
		//public uint MaxEnergy = 0;


	}

}
