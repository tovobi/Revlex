using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class WowObjectCache
	{
		public WowHelpers Helper;
		//public WowObjectCache(WowHelpers _helper)
		//{
		//	Helper = _helper;
		//}





		public static List<WowObject> ScanObj(WowHelpers Helper)
		{
			//ArrayList list = new ArrayList();
			List<WowObject> list = new List<WowObject>();
			//Helper.GetLocalPlayer();

			// set our counter variable to 0 so we can begin counting the objects
			int TotalWowObjects = 0;
			WowObject CurrentObject = new WowObject();
			// set our current object as the first in the object manager
			CurrentObject.ObjBaseAddress = Helper.FirstObject;
			//Log.Print("CurrentObject.ObjBaseAddress: " + CurrentObject.ObjBaseAddress.ToString("X"));



			while ((CurrentObject.ObjBaseAddress & 1) == 0)
			{
                try
                {
                    TotalWowObjects++;
                    // type independent informations
                    CurrentObject.Guid = Helper.wowMem.ReadUInt64((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Guid));
                    //Log.Print("List: " + CurrentObject.Guid);
                    CurrentObject.Type = (short)(Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Type)));
                    switch (CurrentObject.Type)
                    {
                        case (short)Constants.ObjType.OT_UNIT: // an npc
                            CurrentObject.CastSpell = Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.CastSpell));
                            CurrentObject.UnitFieldsAddress = Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
                            CurrentObject.ChannelSpell = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.ChannelSpell));
                            CurrentObject.Health = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
                            CurrentObject.MaxHealth = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
                            CurrentObject.SummonedBy = Helper.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.SummonedBy));
                            CurrentObject.FactionTemplate = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
                            CurrentObject.FactionOffset = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
                            CurrentObject.Dodged = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Dodged));
                            CurrentObject.Level = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));
                            CurrentObject.XPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
                            CurrentObject.YPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
                            CurrentObject.ZPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));
                            CurrentObject.Rotation = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Facing));
                            CurrentObject.PlayerIsFacingTo = Helper.GetFacingToUnit(CurrentObject);
                            CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
                            CurrentObject.Distance = Math.Round((Helper.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
                            CurrentObject.Rotation = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.RotationOffset));
                            CurrentObject.UnitFlags = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Flags));
                            CurrentObject.DynamicFlags = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.DynamicFlags));
                            CurrentObject.IsHostile = Helper.HostileFaction[CurrentObject.FactionTemplate];
                            CurrentObject.Name = Helper.MobNameFromBaseAddr(CurrentObject.ObjBaseAddress);
                            Helper.DecodeUnitFlags(CurrentObject);
                            CurrentObject.Target = new WowObject();
                            //if (readTarget == true)
                            //{
                            //	CurrentObject.TargetGuid = Helper.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));
                            //	if (CurrentObject.TargetGuid != 0)
                            //	{
                            //		CurrentObject.Target = GetUnitFromGuid(CurrentObject.TargetGuid, false, false);
                            //	}
                            //}
                            break;
                        case (short)Constants.ObjType.OT_PLAYER: // a player
                            CurrentObject.CastSpell = Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.CastSpell));
                            CurrentObject.GuidOfAutoAttackTarget = Helper.wowMem.ReadUInt64((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GuidOfAutoAttackTarget));
                            CurrentObject.UnitFieldsAddress = Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
                            CurrentObject.ChannelSpell = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.ChannelSpell));
                            CurrentObject.Health = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
                            CurrentObject.MaxHealth = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
                            CurrentObject.Mana = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Mana));
                            CurrentObject.MaxMana = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxMana));
                            CurrentObject.Rage = (uint)Math.Floor((double)Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Rage)) / 10);
                            CurrentObject.Energy = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Energy));
                            CurrentObject.FactionTemplate = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
                            CurrentObject.FactionOffset = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
                            CurrentObject.Level = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));
                            CurrentObject.Dodged = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Dodged));
                            CurrentObject.XPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
                            CurrentObject.YPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
                            CurrentObject.ZPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));
                            CurrentObject.Rotation = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Facing));
                            CurrentObject.PlayerIsFacingTo = Helper.GetFacingToUnit(CurrentObject);
                            CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
                            CurrentObject.Distance = Math.Round((Helper.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
                            CurrentObject.Rotation = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.RotationOffset));
                            CurrentObject.UnitFlags = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Flags));
                            CurrentObject.DynamicFlags = Helper.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.DynamicFlags));
                            CurrentObject.IsHostile = Helper.IsHostile(CurrentObject);
                            CurrentObject.Name = Helper.PlayerNameFromGuid(CurrentObject.Guid);

                            Helper.DecodeUnitFlags(CurrentObject);
                            CurrentObject.Target = new WowObject();
                            //if (readTarget == true)
                            //{
                            //	CurrentObject.TargetGuid = Helper.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));
                            //	if (CurrentObject.TargetGuid != 0)
                            //	{
                            //		CurrentObject.Target = GetUnitFromGuid(CurrentObject.TargetGuid, false, false);
                            //	}
                            //}
                            break;

                        case (short)Constants.ObjType.OT_GAMEOBJ:
                            CurrentObject.XPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectX));
                            CurrentObject.YPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectY));
                            CurrentObject.ZPos = Helper.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectZ));
                            CurrentObject.PlayerIsFacingTo = Helper.GetFacingToUnit(CurrentObject);
                            CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
                            CurrentObject.Distance = Math.Round((Helper.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
                            CurrentObject.UnitFieldsAddress = Helper.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
                            CurrentObject.Name = Helper.ItemNameFromBaseAddr(CurrentObject.ObjBaseAddress);
                            CurrentObject.GameObjectType = Helper.ItemTypeFromBaseAddr(CurrentObject.ObjBaseAddress);
                            break;

                    }
                }
                catch (Exception e)
                {
                    Log.Print("WowObjectCache.cs -> ScanObj\r\n" + e.Message, 4);
                }
				if (CurrentObject.Type == (short)Constants.ObjType.OT_GAMEOBJ || CurrentObject.Type == (short)Constants.ObjType.OT_PLAYER || CurrentObject.Type == (short)Constants.ObjType.OT_UNIT)
				{
					if (CurrentObject.Guid == Helper.LocalPlayer.Guid)
					{
						Log.Print(CurrentObject.Name + " \t" + CurrentObject.Guid + " \t" + CurrentObject.Distance + " \t<---");
					}
					else
					{
						Log.Print(CurrentObject.Name + " \t" + CurrentObject.Guid + " \t" + CurrentObject.Distance);
					}
				}



				// set the current object as the next object in the object manager
				WowObject tmpObject = CurrentObject;
				CurrentObject = new WowObject();
				list.Add(tmpObject);

                try
                {
                    CurrentObject.ObjBaseAddress = Helper.wowMem.ReadUInt((tmpObject.ObjBaseAddress + (uint)Pointers.ObjectManager.NextObjectOffset));
                }
                catch (Exception e)
                {
                    Log.Print("WowObjectCache.cs -> nextobject\r\n" + e.Message, 4);
                    return list; //if error occured on NextObject, listening will break up
                }



			}

			return list;
		}




	}
}
