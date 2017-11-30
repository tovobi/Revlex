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
	public class WowObject
	{
		// general properties
		public ulong Guid = 0;
		public ulong TargetGuid = 0;
		public WowObject Target;
		public ulong SummonedBy = 0;
		public float XPos = 0;
		public float YPos = 0;
		public float ZPos = 0;
		public string Class = "None";
		public double Distance = 0;
		public uint FactionTemplate = 0;
		public float FactionOffset = 0;
		public WowVector3d vector3d;
		public float Rotation = 0;
		public uint ObjBaseAddress = 0;
		public uint UnitFieldsAddress = 0;
		public short Type = 0;
		public uint Dodged = 0;
		public ulong GuidOfAutoAttackTarget;
		public bool IsInCombat = false;
		public bool IsFleeing = false;
		public bool IsStunned = false;
		public bool CantMove = false;
		public bool IsConfused = false;
		public bool HasBreakableCc = false;
		public List<Auras> BuffList = null;
		public List<Auras> DebuffList = null;		
		public uint ChannelSpell = 0;
		public uint CastSpell = 0;
		public double PlayerIsFacingTo = 0;
		public uint UnitFlags = 0;
		public uint DynamicFlags = 0;
		//public bool TappedByMe = false;
		public String Name = "";
		public bool IsHostile = false; //we havent found a way yet to determine the hostile, thats just for a quick and dirty way for the calculation of # of targets
		public uint tempBuffStacks = 0;
		public string tempNextSpell = "";
		public uint tempTargetPrio = 0;

		// more specialised properties (player or mob)
		public uint Health = 0;
		public uint MaxHealth = 0;
		public uint HealthPercent = 100;
		public uint Mana = 0; // mana, rage and energy will all fall under energy.
		public uint MaxMana = 0;
		public uint ManaPercent = 100;
		public uint Rage = 0; // mana, rage and energy will all fall under energy.
		public uint Energy = 0;
		public uint Level = 0;

		public uint GameObjectType = 0;
		public WowObject next = null;
		public uint Test = 0;

        public WowVector3d GetRadarPosition(uint radarSizeX, uint radarSizeY, double maxRange, double xFromVec3d, double yFromVec3d)
        {
            double x;
            double y;
            double distanceX = (XPos - xFromVec3d) * -1; // the coords in wow seems to be invers, so * -1
            double distanceY = (YPos - yFromVec3d) * -1; // the coords in wow seems to be invers, so * -1
            x = ((radarSizeX / (maxRange * 2)) * distanceX) + (radarSizeX / 2);
            y = ((radarSizeY / (maxRange * 2)) * distanceY) + (radarSizeY / 2);
            return new WowVector3d(x, y);
        }
        public WowVector3d GetRotatingRadarPosition(uint radarWidth, uint radarHeight, float maxRange, WowVector3d localPlayerPos)
        {
            return new WowVector3d(1, 1);
        }
    }
}