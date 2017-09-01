using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class ActionButtonAndSpell
	{
		public string SpellName { get; set; }
		public string Key { get; set; }
		private string _Modifier;
		public string Modifier
		{
			get
			{
				return _Modifier;
			}
			set
			{
				_Modifier = value;
				if (value == "SHIFT")
				{
					ModifierDecoded = "+";
				}
				else if (value == "CTRL")
				{
					ModifierDecoded = "^";
				}
				else if (value == "ALT")
				{
					ModifierDecoded = "%";
				}
				else
				{
					ModifierDecoded = "";
				}
			}
		}
		public string ModifierDecoded { get; set; }
		public uint Stance { get; set; }
		public uint SpellId;
		public uint MacroId;
		public uint ItemId;
		private uint _Id;
		public uint Id // in "Id" we display any type of id, even its macro, spell or item
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
				if (value > 0x80000000)
				{
					ItemId = value - 0x80000000;
				}
				else if (value > 0x40000000)
				{
					MacroId = value - 0x40000000;
				}
				else
				{
					SpellId = value;
				}
			}
		}
		public string ActionButton { get; set; }
		public ActionButtonAndSpell(string _spellName = "-", string _key = "-", string _modifier = "None", uint _stance = 0,  uint _id = 0, string _actionButton = "None")
		{
			SpellName = _spellName;
			Key = _key;
			Modifier = _modifier;
			Stance = _stance;
			Id = _id;
			ActionButton = _actionButton;
		}

	}
}
