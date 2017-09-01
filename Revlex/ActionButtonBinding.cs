using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class ActionButtonBinding
	{

		public ActionButtonBinding(uint _actionButtonOffset)
		{
			ActionButtonOffset = _actionButtonOffset;
			//Index = (ActionButtonOffset - (uint)Pointers.ActionButtons.ACTIONBUTTON1) /4;
			ActionButtonName = Enum.GetName(typeof(Pointers.ActionButtons), ActionButtonOffset);
		}
		//public uint Index;
		public string SpellName = "";
		public uint SpellId = 0;
		public string ActionButtonName = "";
		public uint ActionButtonOffset = 0;
		public string Keybinding;
		public uint Stance;
	}

}
