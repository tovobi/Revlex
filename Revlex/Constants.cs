/*
Copyright 2012 DaCoder
This file is part of daCoders Tool for WoW 1.12.1.
daCoders Tool for WoW 1.12.1 is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
daCoders Tool for WoW 1.12.1 is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revlex
{
	class Constants
	{

		/* 
         * This Constants are from Cataclysm. 
         * Don't know if all of them are correct,
         * but the needed ones are working for 1.12.1
         */

		#region ObjType enum

		public enum ObjType : uint
		{
			OT_NONE = 0,
			OT_ITEM = 1,
			OT_CONTAINER = 2,
			OT_UNIT = 3,
			OT_PLAYER = 4,
			OT_GAMEOBJ = 5,
			OT_DYNOBJ = 6,
			OT_CORPSE = 7,
			OT_FORCEDWORD = 0xFFFFFFFF
		}

		#endregion

		public static String GetPlayerFaction(uint playerFaction)
		{
			String[] playerFactionTable = new String[2000];
			playerFactionTable[1] = "Alliance";
			playerFactionTable[2] = "Horde";
			playerFactionTable[3] = "Alliance";
			playerFactionTable[4] = "Alliance";
			playerFactionTable[5] = "Horde";
			playerFactionTable[6] = "Horde";
			playerFactionTable[8] = "Alliance";
			playerFactionTable[9] = "Alliance";
			playerFactionTable[115] = "Alliance";
			playerFactionTable[116] = "Horde";

			for (int i = 117; i < 2000; i++)
			{
				playerFactionTable[i] = "keineAhnung";
			}
			return playerFactionTable[playerFaction];
		}





		#region GameObjectType enum

		public enum GameObjectType : uint
		{
			Door = 0,
			Button = 1,
			QuestGiver = 2,
			Chest = 3,
			Binder = 4,
			Generic = 5,
			Trap = 6,
			Chair = 7,
			SpellFocus = 8,
			Text = 9,
			Goober = 0xa,
			Transport = 0xb,
			AreaDamage = 0xc,
			Camera = 0xd,
			WorldObj = 0xe,
			MapObjTransport = 0xf,
			DuelArbiter = 0x10,
			FishingNode = 0x11,
			Ritual = 0x12,
			Mailbox = 0x13,
			AuctionHouse = 0x14,
			SpellCaster = 0x16,
			MeetingStone = 0x17,
			Unkown18 = 0x18,
			FishingPool = 0x19,
			FORCEDWORD = 0xFFFFFFFF,
		}
		#endregion
	}
}
