using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Magic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Revlex
{
	public class WowHelpers
	{

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern uint QueryPerformanceCounter(out long lpPerformanceCount);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern uint QueryPerformanceFrequency(out long lpFrequency);


		/// <summary>
		/// Cunstructor of WowHelpers
		/// </summary>
		/// <param name="_localPlayer">WowObject of local player</param>
		/// <param name="_currentTarget">WowObject of current target</param>
		public WowHelpers(WowObject _localPlayer, WowObject _currentTarget, RichTextBox _richDebug)
		{
			LocalPlayer = _localPlayer;
			CurrentTarget = _currentTarget;
			RichDebug = _richDebug;
			//for (int i = 1; i <= 12; i++)
			//{
			//	ConfigButtonsToIngameButtons.Add("ACTIONBUTTON"+i.ToString(), "ActionButton");
			//	ConfigButtonsToIngameButtons.Add("MULTIACTIONBAR1BUTTON", "BottomLeftActionButton");
			//	ConfigButtonsToIngameButtons.Add("MULTIACTIONBAR2BUTTON", "BottomRightActionButton");
			//	ConfigButtonsToIngameButtons.Add("MULTIACTIONBAR3BUTTON", "RightActionButton");
			//}
		}
        public List<WowObject> LastCachedUnitList = new List<WowObject>();
        public List<WowObject> CachedUnitlist = new List<WowObject>();
		public RichTextBox RichDebug = new System.Windows.Forms.RichTextBox();
		public bool[] HostileFaction = new bool[2500];
		public string AccountName;
		public string WowFolder;
		public string bindingFile;
		public int wowProc { get; private set; }
		public WowObject LocalPlayer;
		public ulong LastLocalPlayerGuid = 0;
		public bool ConnectionError = false;
		public long LastPlayerGuidUpdate = 0;
		public WowObject CurrentTarget;
		public uint FirstObject = 0;
		public uint StaticClientConnection = 0;
		public uint readTarget = 0;
		uint ClientConnection = 0;
		uint FirstObjectOffset = (uint)Pointers.ObjectManager.FirstObjectOffset;
		public BlackMagic wowMem { get; set; }  = new BlackMagic();
		public BindingList<ActionButtonAndSpell> ActionButtonAndSpellList = new BindingList<ActionButtonAndSpell>();
		public List<Spells> SpellsInSpellBook = new List<Spells>();

		public bool Init(bool firstInit = true)
		{
			if (!GetWowProcess())
			{
				if (!firstInit) return false;
				MessageBox.Show(
					"WoW process found: \t[ERROR]", 
					"Error"
					);
				Log.Print("No wow process found.");
				return false;
			}
			if (!GetStaticClientConnection())
			{
				if (!firstInit) return false;
				MessageBox.Show(
					"WoW process found: \t[done]" +
					"\r\nstatic client connection: \t[ERROR]", 
					"Error");
				Log.Print("Could not load static client connection.",4);

				return false;
			}
			//LocalPlayer = GetLocalPlayer()
			return true;
		}
		public static long GetTime()
		{
			var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
			return (long)timeSpan.TotalMilliseconds;
		}
		public void clearLog()
		{
			while (RichDebug.Lines.Length > 70)
			{
				RichDebug.Select(0, RichDebug.GetFirstCharIndexFromLine(1)); // select the first line
				RichDebug.SelectedText = "";
			}
		}
		public bool CheckConnection()
		{
			if (LastPlayerGuidUpdate < WowHelpers.GetTime() - 5000)
			{
				LocalPlayer.Guid = GetLocalPlayerGuid();
				//Log.Print("Refresh LocalPlayerGuid " + LastPlayerGuidUpdate + " : " + WowHelpers.GetTime());
				LastPlayerGuidUpdate = WowHelpers.GetTime();
				// if is not the first guid and the Guid was changed or the current guid is 0
				if (LastLocalPlayerGuid != 0 && (LastLocalPlayerGuid != LocalPlayer.Guid || LocalPlayer.Guid == 0))
				{
					LastLocalPlayerGuid = LocalPlayer.Guid;
					ConnectionError = true;
					Log.Print("Connect error (GUID)");
					return false;
				}
				LastLocalPlayerGuid = LocalPlayer.Guid;
				ConnectionError = false;
			}
			return true;
		}
		public uint GetTextCursor()
		{
			try
			{
				return wowMem.ReadUInt((uint)Pointers.StaticAddresses.TextCaretActive);
			}
			catch (IndexOutOfRangeException exc)
			{
				MessageBox.Show("Couldnt read memory for textcaret: " + exc.ToString());
				Log.Print("Couldnt read memory for textcaret: " + exc.ToString(), 4);
			}
			return 0;
		}

		public bool GetWowProcess()
		{
			try
			{
				if (wowMem != null)
				{
					return wowMem.OpenProcessAndThread(SProcess.GetProcessFromWindowTitle("World of Warcraft"));
				}
				return false;
			}
			catch (IndexOutOfRangeException exc)
			{
				return false;
			}
		}

		private bool GetStaticClientConnection()
		{
			StaticClientConnection = (uint)wowMem.MainModule.BaseAddress + (uint)Pointers.ObjectManager.CurMgrPointer;
			return (StaticClientConnection != 0) ? true : false;
		}




		

		public bool GetBasicWowData(bool firstInit = true)
		{
			try
			{
				ClientConnection = wowMem.ReadUInt((StaticClientConnection));
				FirstObject = wowMem.ReadUInt((ClientConnection + FirstObjectOffset));
				LocalPlayer.Guid = wowMem.ReadUInt64((uint)wowMem.MainModule.BaseAddress + (uint)Pointers.StaticAddresses.LocalPlayerGUID);
				LastPlayerGuidUpdate = GetTime();
				//ScanObj();
			}
			catch (Exception e)
			{
				MessageBox.Show(
					"WoW process found: \t[done]" +
					"\r\nstatic client connection: \t[done]" +
					"\r\nget basic wow data: \t\t[ERROR]",
					"Error");
				Log.Print("Could not load client connection: " + ClientConnection.ToString("X") + " / " + FirstObject.ToString("X") + " / "  + LocalPlayer.Guid.ToString("X") + "\r\n" + e.Message,4);
				return false;
			}
			ScanObj();

			// if the local guid is zero it means that something failed.
			return (LocalPlayer.Guid != 0) ? true : false;
		}

		public ulong GetLocalPlayerGuid()
		{
			try
			{
				LocalPlayer.Guid = wowMem.ReadUInt64((uint)wowMem.MainModule.BaseAddress + (uint)Pointers.StaticAddresses.LocalPlayerGUID);
				LastPlayerGuidUpdate = GetTime();
				return LocalPlayer.Guid;
			}
			catch (Exception e)
			{
				Log.Print("No connection to world.\r\n" + e.Message, 4);
				return 0;
			}
		}

		public ulong GetCurrentTargetGuid()
		{
			return wowMem.ReadUInt64((uint)wowMem.MainModule.BaseAddress + (uint)Pointers.StaticAddresses.CurrentTargetGUID);
		}

		private string ReadASCIIString(uint addr)
		{
			return wowMem.ReadASCIIString(addr, 50);
		}

		public string ItemNameFromBaseAddr(uint BaseAddr)
		{
			uint ObjectBase = BaseAddr;
			return ReadASCIIString((wowMem.ReadUInt((wowMem.ReadUInt((ObjectBase + (uint)Pointers.UnitName.ObjectName1)) + (uint)Pointers.UnitName.ObjectName2))));
		}

		public uint ItemTypeFromBaseAddr(uint BaseAddr)
		{
			uint ObjectBase = BaseAddr;
			return wowMem.ReadUInt((ObjectBase + (uint)Pointers.UnitName.ItemType));
		}

		public string MobNameFromBaseAddr(uint BaseAddr)
		{
			uint ObjectBase = BaseAddr;
			return ReadASCIIString((wowMem.ReadUInt((wowMem.ReadUInt((ObjectBase + (uint)Pointers.UnitName.UnitName1)) + (uint)Pointers.UnitName.UnitName2))));
		}

		/*
		private string PlayerNameFromGuid2(ulong guid)
		{

			var nameBasePtr = (uint)wowMem.MainModule.BaseAddress + (uint)Pointers.UnitName.PlayerNameCachePointer; // Player name database
			while (true)
			{
				var nextGuid = wowMem.ReadUInt64(((uint)nameBasePtr + (uint)Pointers.UnitName.PlayerNameGUIDOffset));
				if (nextGuid == 0)
				{
					return "";
				}
				if (nextGuid != guid)
				{
					nameBasePtr = nameBasePtr.ReadAs<IntPtr>();
				}
				else
				{
					break;
				}
			}
			return nameBasePtr.Add(0x14).ReadString(30);

		}
		*/
		public string PlayerNameFromGuid(ulong guid)
		{
			ulong nameStorePtr = (uint)wowMem.MainModule.BaseAddress + (uint)Pointers.UnitName.PlayerNameCachePointer; // Player name database
			ulong base_, testGUID;

			base_ = wowMem.ReadUInt((uint)nameStorePtr);
			testGUID = wowMem.ReadUInt64(((uint)base_ + (uint)Pointers.UnitName.PlayerNameGUIDOffset));
			//if (testGUID == 0) // 1.8.2017 tvb
			//{
			//	return "";
			//}
			while (testGUID != guid)
			{
				//read next
				base_ = wowMem.ReadUInt(((uint)base_));
				testGUID = wowMem.ReadUInt64(((uint)base_ + (uint)Pointers.UnitName.PlayerNameGUIDOffset));
			}

			// Hopefully found the guid in the name list...
			// I don't know how to check for not found
			return ReadASCIIString((uint)base_ + (uint)Pointers.UnitName.PlayerNameStringOffset);
		}


		public string MobNameFromGuid(ulong Guid)
		{
			uint objectBase = GetObjectBaseByGuid(Guid);
			return ReadASCIIString((wowMem.ReadUInt((wowMem.ReadUInt((objectBase + (uint)Pointers.UnitName.UnitName1)) + (uint)Pointers.UnitName.UnitName2))));
		}
		public string MobNameFromGuid(ulong Guid, uint objectBase)
		{
			return ReadASCIIString((wowMem.ReadUInt((wowMem.ReadUInt((objectBase + (uint)Pointers.UnitName.UnitName1)) + (uint)Pointers.UnitName.UnitName2))));
		}
		public uint GetObjectBaseByGuid(ulong Guid)
		{
			WowObject TempObject = new WowObject();
			// set the current object to the first object in the object manager
			TempObject.ObjBaseAddress = FirstObject;

			// while the base address of the current object is not 0, find the guid
			// and compare it to the one passed in. if it matches, return that base
			// address, otherwise continue looking
			while (TempObject.ObjBaseAddress != 0)
			{
				try
				{
					TempObject.Guid = wowMem.ReadUInt64((TempObject.ObjBaseAddress + (uint)Pointers.WowObject.Guid));
					if (TempObject.Guid == Guid)
						return TempObject.ObjBaseAddress;
					TempObject.ObjBaseAddress = wowMem.ReadUInt((TempObject.ObjBaseAddress + (uint)Pointers.ObjectManager.NextObjectOffset));
				}
				catch (Exception)
				{
					return 0;
				}

			}
			// if we reached here it means we couldn't find the Guid we were looking for, return 0
			return 0;
		}
		public bool HostileNpcInCc(double radius = 10)
		{
			return CachedUnitlist.Exists(o => o.Distance <= radius && o.Type == (short)Constants.ObjType.OT_UNIT && o.HasBreakableCc && o.IsHostile);
		}

		// For Sunder/Rend-Combo
		public WowObject GetBestTankTarget2(string[] aura, double radius = 15, int maxNumberToScan = 15)
		{
			WowObject tempPreferedTar;
			//read stacks of sunder armor and add it to the concerned unit
			//Targets12Within5.ForEach(x => x.tempBuffStacks = ((WowHelperObj.GetUnitBuffs(x).FirstOrDefault(c => c.Name == "Sunder Armor")).Stacks ?? 0));
			Auras tempEmptyAura = new Auras("", 1, _stacks: 0);
			//Scans all Targets for "Sunder Armor"-Debuff and assign the stacks to the respective WowObject, if no Auras-Object, it returns stacks a new empty Object of Auras via Null-Coalesce Operator
			List<WowObject> tempList = CachedUnitlist.Where(o => o.Distance <= radius && o.Health > 0 && o.Type == (short)Constants.ObjType.OT_UNIT && !o.HasBreakableCc).Take(maxNumberToScan).ToList();
			foreach (WowObject x in tempList)
			{
				x.tempNextSpell = "";
				x.tempTargetPrio = 0;
				foreach (string z in aura)
				{
                    if (!x.DebuffList.Exists(o => o.Name == z))
					{
						x.tempNextSpell = z;
						x.tempTargetPrio++;
					}
				}
			}
			//foreach (WowObject x in tempList)
			//{
			//	Log.Print("-- --" + x.Name + " " + x.Guid + " " + x.tempTargetPrio + " " + x.tempNextSpell);
			//}
			//Log.Print("--2--GetBestTankTarget2():   pref: " + tempList.OrderByDescending(c => c.tempTargetPrio).FirstOrDefault().Guid.ToString() + " |    tar: " + LocalPlayer.Target.Guid + " |   next Hostile: " + CachedUnitlist.FirstOrDefault(c => c.IsHostile).Guid.ToString());
			tempPreferedTar = tempList.OrderByDescending(c => c.tempTargetPrio).FirstOrDefault(x => x.tempTargetPrio > 0);
			return tempPreferedTar;
		}
		public WowObject GetBestTankTarget(double radius = 15, int maxNumberToScan = 15, string aura = "")
		{
			WowObject tempPreferedTar;
			//read stacks of sunder armor and add it to the concerned unit
			//Targets12Within5.ForEach(x => x.tempBuffStacks = ((WowHelperObj.GetUnitBuffs(x).FirstOrDefault(c => c.Name == "Sunder Armor")).Stacks ?? 0));
			Auras tempEmptyAura = new Auras("", 1, _stacks: 0);
			//Scans all Targets for "Sunder Armor"-Debuff and assign the stacks to the respective WowObject, if no Auras-Object, it returns stacks a new empty Object of Auras via Null-Coalesce Operator
			List<WowObject> tempList = CachedUnitlist.Where(o => o.Distance <= radius && o.Health > 0 && o.Type == (short)Constants.ObjType.OT_UNIT && !o.HasBreakableCc).Take(maxNumberToScan).ToList();
			tempList.ForEach(x => x.tempBuffStacks = (x.DebuffList.FirstOrDefault(o => o.Name == aura) ?? tempEmptyAura).Stacks);
            //tempList.ForEach(x => x.tempBuffStacks = (GetUnitDebuffs(x).FirstOrDefault(c => c.Name == aura) ?? tempEmptyAura).Stacks);
            // now return the object with the least stack of sunder armor, if no such object is returned, the ??-Operator choose the current target		
            //List<WowObject> tempList2 = tempList;
            //tempList2.OrderBy(c => c.tempBuffStacks);
            //foreach (WowObject x in tempList2)
            //{
            //	Log.Print(x.Guid.ToString() + " : " + x.tempBuffStacks);
            //	List<Auras> tempEmptyAura2 = GetUnitDebuffs(x);
            //	foreach (Auras z in tempEmptyAura2)
            //	{
            //		Log.Print(" Aura: " + z.Name);
            //	}
            //}
            //Log.Print("GetBestTankTarget(): AggroOnWeak: " + AggroOnWeak(40).Guid + "   AggroOnParty: " + AggroOnParty(40).Guid + "   pref: " + tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault().Guid.ToString() + " |    tar: " + LocalPlayer.Target.Guid + " |   next Hostile: " + CachedUnitlist.FirstOrDefault(c => c.IsHostile).Guid.ToString());
            //Log.Print("AW: " + AggroOnWeak(40).PlayerIsFacingTo + "   AP: " + AggroOnParty(40).PlayerIsFacingTo + "   tar: " + LocalPlayer.Target.PlayerIsFacingTo);
            if (AggroOnWeak(40).Guid != 0)
            {
                Log.Print("AggroOnWeak(40): " + AggroOnWeak(40).Guid);
                return AggroOnWeak(40);
            }
            else if (AggroOnParty(40).Guid != 0)
            {
                Log.Print("AggroOnParty(40): " + AggroOnParty(40).Guid);
                return AggroOnParty(40);
            }
            else if (tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault().Guid != 0)
            {
                WowObject zop = tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault();
                Log.Print("tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault(): " + zop.Guid + " " + zop.Name + " " + zop.Distance);
                return tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault();
            }
            else if (LocalPlayer.Target.Guid != 0)
            {
                Log.Print("LocalPlayer.Target: " + LocalPlayer.Target.Guid);
                return LocalPlayer.Target;
            }

			return CachedUnitlist.FirstOrDefault(c => c.IsHostile);
			//tempPreferedTar = AggroOnWeak(40, 10) ?? AggroOnParty(40, 10) ?? tempList.OrderBy(c => c.tempBuffStacks).FirstOrDefault() ?? LocalPlayer.Target ?? CachedUnitlist.FirstOrDefault(c => c.IsHostile);
			//return tempPreferedTar;
		}

		public WowObject AggroOnParty(double radius = 40, int maxNumberToScan = 10)
		{
			List<WowObject> unitList = CachedUnitlist.Where(c => c.Distance < radius && c.Health > 0 && c.IsInCombat && c.TargetGuid != LocalPlayer.Guid && !c.Target.IsHostile && c.IsHostile && c.Target.Guid != 0 && !c.HasBreakableCc).ToList();
			//List<WowObject> unitList = CachedUnitlist.Where(c => c.Distance < radius && c.IsHostile && !c.HasBreakableCc && c.PlayerIsFacingTo < 1.0).ToList();
			WowObject unit = unitList.OrderBy(c => c.Distance).FirstOrDefault();
			return unit ?? new WowObject();
		}
		public WowObject AggroOnWeak(double radius = 40, int maxNumberToScan = 10)
		{
			List<WowObject> unitList = CachedUnitlist.Where(c => c.Distance < radius && c.Health > 0 && c.IsInCombat && c.TargetGuid != LocalPlayer.Guid && !c.Target.IsHostile && c.IsHostile && c.Target.Guid != 0 && !c.HasBreakableCc && (c.Target.Class == "Priest" || c.Target.Class == "Druid" || c.Target.Class == "Shaman" || c.Target.Class == "Paladin" || c.Target.HealthPercent < 30)).ToList();
			//List<WowObject> unitList = CachedUnitlist.Where(c => c.Distance < radius && c.IsHostile && !c.HasBreakableCc && c.PlayerIsFacingTo < 0.5).ToList();
			WowObject unit = unitList.OrderBy(c => c.Distance).FirstOrDefault();
			return unit ?? new WowObject();
		}


		public List<WowObject> GetCastingEnemies(double radius = 15, int maxNumberToScan = 5)
		{
			//Log.Print("GetCastingEnemies");
   //         List<WowObject> tempTarList0 = CachedUnitlist.Where(c => c.IsHostile && c.Health > 0).ToList();
   //         List<WowObject> tempTarList1 = CachedUnitlist.Where(c => c.Distance <= radius && c.Health > 0).ToList();
   //         List<WowObject> tempTarList2 = tempTarList1.Where(c => (c.CastSpell > 0 || c.ChannelSpell > 0) && c.IsHostile).ToList();
   //         List<WowObject> tempTarList3 = tempTarList2.Where(c => c.PlayerIsFacingTo < 1.5).ToList();
   //         foreach (WowObject x in tempTarList0)
   //         {
   //             Log.Print("X: " + x.Guid + " " + x.Name);
   //         }
   //         Log.Br();
   //         foreach (WowObject x in tempTarList1)
   //         {
   //             Log.Print("A: " + x.Guid + " " + x.Name);
   //         }
   //         Log.Br();
   //         foreach (WowObject x in tempTarList2)
   //         {
   //             Log.Print("B: " + x.Guid + " " + x.Name);
   //         }
   //         Log.Br();
   //         foreach (WowObject x in tempTarList3)
   //         {
   //             Log.Print("C: " + x.Guid + " " + x.Name);
   //         }
   //         Log.Br();

            return CachedUnitlist.Where(c => (c.CastSpell > 0 || c.ChannelSpell > 0) &&  c.IsHostile && c.Distance <= radius && c.Health > 0 && c.PlayerIsFacingTo < 1.5).Take(maxNumberToScan).ToList();
		}



		public List<WowObject> GetNearEnemies(double radius = 15, int maxNumberToScan = 5)
		{
			//Log.Print("GetNearEnemies("+radius+", "+maxNumberToScan+"): ",0,0);
			List<WowObject> tempList = CachedUnitlist.Where(c => c.IsHostile && c.Health > 10 && c.Distance <= radius).Take(maxNumberToScan).ToList();
			//foreach (WowObject x in tempList)
			//{
			//	Log.Print(x.Name+", ",0,0);
			//}
			Log.Br();
			return tempList;
		}


		private ulong GetObjectGuidByBase(uint Base)
		{
			return wowMem.ReadUInt64((Base + (uint)(uint)Pointers.WowObject.Guid));
		}
		public bool IsHostile(WowObject unit)
		{
			return Constants.GetPlayerFaction((uint)unit.FactionTemplate) != null && Constants.GetPlayerFaction((uint)unit.FactionTemplate) != Constants.GetPlayerFaction((uint)LocalPlayer.FactionTemplate);
		}
		public bool HasHostileTarget(WowObject unit)
		{
			return Constants.GetPlayerFaction((uint)unit.FactionTemplate) != null && Constants.GetPlayerFaction((uint)unit.FactionTemplate) != Constants.GetPlayerFaction((uint)unit.Target.FactionTemplate);

		}
		public bool HasFriendlyTarget(WowObject unit)
		{
			return Constants.GetPlayerFaction((uint)unit.FactionTemplate) != null && Constants.GetPlayerFaction((uint)unit.FactionTemplate) == Constants.GetPlayerFaction((uint)unit.Target.FactionTemplate);
		}
		public bool HasTarget(WowObject unit)
		{
			return unit.TargetGuid != 0;
		}

		public double nfmod(double a, double b)
		{
			return a - b * Math.Floor(a / b);
		}
		public double GetAtan2ToUnit(WowObject src, WowObject dest)
		{
			return nfmod(Math.Atan2((src.XPos - dest.XPos), (src.YPos - dest.YPos)), 2 * Math.PI);
		}
		public double GetFacingToUnit(WowObject dest)
		{
			// wtf sometimes it returns a "-" when its under 0.5, anyway doesnt matter because the crititacl point is > 1.5
			//return Math.PI - Math.Abs(LocalPlayer.Rotation - (nfmod(Math.Atan2((LocalPlayer.XPos - dest.XPos), (LocalPlayer.YPos - dest.YPos)), 2 * Math.PI)));
			return Math.Abs(Math.PI - Math.Abs(LocalPlayer.Rotation - (nfmod(Math.Atan2((LocalPlayer.XPos - dest.XPos), (LocalPlayer.YPos - dest.YPos)), 2 * Math.PI))));
		}


		public WowObject GetUnitFromGuid(ulong guid)
		{
			//if found, it returns the right unit or an empty WowObject
			return CachedUnitlist.FirstOrDefault(c => c.Guid == guid) ?? new WowObject();
		}


		public WowObject GetCurrentTarget()
		{
			return LocalPlayer.Target;
		}




		public void ScanObj()
		{
            //Log.Print("[]");
            //ArrayList list = new ArrayList();
            LastCachedUnitList = CachedUnitlist;
            CachedUnitlist = new List<WowObject>();
			// set our counter variable to 0 so we can begin counting the objects
			int TotalWowObjects = 0;
			WowObject CurrentObject = new WowObject();
			// set our current object as the first in the object manager
			CurrentObject.ObjBaseAddress = FirstObject;
			while ((CurrentObject.ObjBaseAddress & 1) == 0)
			{
				TotalWowObjects++;
				// type independent informations
				CurrentObject.Guid = wowMem.ReadUInt64((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Guid));
				//Log.Print("List: " + CurrentObject.Guid);
				CurrentObject.Type = (short)(wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Type)));
				switch (CurrentObject.Type)
				{
					case (short)Constants.ObjType.OT_UNIT: // an npc
						CurrentObject.CastSpell = wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.CastSpell));
						CurrentObject.UnitFieldsAddress = wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.ChannelSpell = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.ChannelSpell));
						CurrentObject.Health = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
						CurrentObject.MaxHealth = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
						CurrentObject.HealthPercent = (uint)Math.Floor(((double)CurrentObject.Health / (double)CurrentObject.MaxHealth * 100));
						CurrentObject.SummonedBy = wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.SummonedBy));
						CurrentObject.FactionTemplate = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
						CurrentObject.FactionOffset = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
						//CurrentObject.Dodged = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Dodged));
						CurrentObject.Level = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));
						CurrentObject.XPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
						CurrentObject.YPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
						CurrentObject.ZPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));
						Pointers.PlayerClass.TryGetValue(wowMem.ReadByte((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.PlayerClass)), out CurrentObject.Class);
						CurrentObject.Rotation = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Facing));
						//CurrentObject.PlayerIsFacingTo = GetFacingToUnit(CurrentObject);
						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						//CurrentObject.Distance = Math.Round((LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
						CurrentObject.UnitFlags = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Flags));
						CurrentObject.DynamicFlags = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.DynamicFlags));
						CurrentObject.IsHostile = HostileFaction[CurrentObject.FactionTemplate];
						CurrentObject.Name = MobNameFromBaseAddr(CurrentObject.ObjBaseAddress);
						DecodeUnitFlags(CurrentObject);
						CurrentObject.Target = new WowObject();
						CurrentObject.TargetGuid = wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));

                        CachedUnitlist.Add(CurrentObject);
                        break;

					case (short)Constants.ObjType.OT_PLAYER: // a player
						CurrentObject.CastSpell = wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.CastSpell));
						CurrentObject.GuidOfAutoAttackTarget = wowMem.ReadUInt64((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GuidOfAutoAttackTarget));
						CurrentObject.UnitFieldsAddress = wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.ChannelSpell = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.ChannelSpell));
						CurrentObject.Health = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
						CurrentObject.MaxHealth = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
						CurrentObject.HealthPercent = (uint)Math.Floor(((double)CurrentObject.Health / (double)CurrentObject.MaxHealth * 100));
						CurrentObject.Mana = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Mana));
						CurrentObject.MaxMana = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxMana));
						CurrentObject.Rage = (uint)Math.Floor((double)wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Rage)) / 10);
						CurrentObject.Energy = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Energy));
						CurrentObject.FactionTemplate = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
						CurrentObject.FactionOffset = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
						CurrentObject.Level = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));
						//CurrentObject.Dodged = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Dodged));
						CurrentObject.XPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
						CurrentObject.YPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
						CurrentObject.ZPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));
						Pointers.PlayerClass.TryGetValue(wowMem.ReadByte((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.PlayerClass)), out CurrentObject.Class);
						CurrentObject.Rotation = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Facing));
						//CurrentObject.PlayerIsFacingTo = GetFacingToUnit(CurrentObject);
						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						//CurrentObject.Distance = Math.Round((LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
						CurrentObject.UnitFlags = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Flags));
						CurrentObject.DynamicFlags = wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.DynamicFlags));
						CurrentObject.IsHostile = IsHostile(CurrentObject);
						CurrentObject.Name = PlayerNameFromGuid(CurrentObject.Guid);
						DecodeUnitFlags(CurrentObject);
						CurrentObject.Target = new WowObject();
						CurrentObject.TargetGuid = wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));

                        CachedUnitlist.Add(CurrentObject);
                        break;

					case (short)Constants.ObjType.OT_GAMEOBJ:
						CurrentObject.XPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectX));
						CurrentObject.YPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectY));
						CurrentObject.ZPos = wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectZ));
						//CurrentObject.PlayerIsFacingTo = GetFacingToUnit(CurrentObject);
						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						//CurrentObject.Distance = Math.Round((LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);
						CurrentObject.UnitFieldsAddress = wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.Name = ItemNameFromBaseAddr(CurrentObject.ObjBaseAddress);
						CurrentObject.GameObjectType = ItemTypeFromBaseAddr(CurrentObject.ObjBaseAddress);

                        CachedUnitlist.Add(CurrentObject);
                        break;
				}

				// set the current object as the next object in the object manager
				WowObject tmpObject = CurrentObject;
				CurrentObject = new WowObject();				
				CurrentObject.ObjBaseAddress = wowMem.ReadUInt((tmpObject.ObjBaseAddress + (uint)Pointers.ObjectManager.NextObjectOffset));
			}

			foreach (WowObject listObj in CachedUnitlist)
			{
				if (listObj.Type == (short)Constants.ObjType.OT_PLAYER || listObj.Type == (short)Constants.ObjType.OT_UNIT)
				{

					//if the currentobj-guid is already in cache as a targetguid, then the corresponding obj will get his target(CurrentObject)
					List<WowObject> IsTargetOfList = CachedUnitlist.Where(c => (c.Type == (short)Constants.ObjType.OT_PLAYER || c.Type == (short)Constants.ObjType.OT_UNIT) && c.TargetGuid == listObj.Guid).ToList();
					if (IsTargetOfList.Count > 0)
					{
						IsTargetOfList.ForEach(c => c.Target = listObj);
					}
					if (listObj.Guid == LocalPlayer.Guid)
					{
						LocalPlayer = listObj; // Copy whole listObj to LocalPlayer
					}
				}
				if (listObj.Type == (short)Constants.ObjType.OT_PLAYER || listObj.Type == (short)Constants.ObjType.OT_UNIT || listObj.Type == (short)Constants.ObjType.OT_GAMEOBJ)
				{
					if (listObj.vector3d != null && LocalPlayer.vector3d != null)
					{
						listObj.Distance = Math.Round((LocalPlayer.vector3d.Distance(listObj.vector3d)), 2);
					}
					else
					{
						listObj.Distance = 999;
					}
					listObj.PlayerIsFacingTo = GetFacingToUnit(listObj);

				}
                // scan the auras of unit only i unit is near
                if (listObj.Distance <= 30 && (listObj.Type == (short)Constants.ObjType.OT_UNIT || listObj.Type == (short)Constants.ObjType.OT_PLAYER))
                {
                    listObj.Dodged = wowMem.ReadUInt((listObj.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Dodged));
                    listObj.BuffList = GetUnitBuffs(listObj);
                    // give TimeApplied values from last list to the new list of units
                    if (LastCachedUnitList.Count != 0)
                    { 
                        listObj.BuffList.ForEach(x => x.TimeApplied = LastCachedUnitList.FirstOrDefault(o => o.Guid == listObj.Guid).BuffList.FirstOrDefault(z => z.Id == x.Id).TimeApplied);
                    }
                    listObj.DebuffList = GetUnitDebuffs(listObj);
                    // give TimeApplied values from last list to the new list of units
                    if (LastCachedUnitList.Count != 0)
                    {
                        listObj.DebuffList.ForEach(x => x.TimeApplied = LastCachedUnitList.FirstOrDefault(o => o.Guid == listObj.Guid).DebuffList.FirstOrDefault(z => z.Id == x.Id).TimeApplied);
                    }
					listObj.HasBreakableCc = listObj.DebuffList.Exists(c => c.Name == "Polymorph" || c.Name == "Sap" || c.Name == "Blind");
				}
				else
				{
					listObj.Dodged = 0;
					listObj.BuffList = null;
					listObj.DebuffList = null;
					listObj.HasBreakableCc = false;
				}
			}
			//// add again data to localplayer
			//LocalPlayer = CachedUnitlist.FirstOrDefault(c => c.Guid == LocalPlayer.Guid);
			//foreach (WowObject listObj in CachedUnitlist)
			//{
			//	if (listObj.Type == (short)Constants.ObjType.OT_PLAYER || listObj.Type == (short)Constants.ObjType.OT_UNIT)
			//	{
			//		if (listObj.Type == (short)Constants.ObjType.OT_PLAYER)
			//		{
			//			Log.Print("Human: ", 0, 0);
			//		}
			//		else if (listObj.Type == (short)Constants.ObjType.OT_UNIT)
			//		{
			//			Log.Print("Npc:   ", 0, 0);
			//		}
			//		Log.Print(z(listObj.Name, 20) + " \t" + z(listObj.Guid.ToString(), 24) + " \t" + z(listObj.Target.Name, 20) + " \t" + listObj.Distance, 0, 0);
			//		if (listObj.Guid == LocalPlayer.Guid)
			//		{
			//			LocalPlayer = listObj; // Copy whole listObj to LocalPlayer
			//			Log.Print("\t <-- me   ", 0, 0);
			//		}
			//		Log.Br();
			//	}


			//}
			//Log.Print("[]");
		}


		public string z(string s,int l)
		{
			string addLength = "";
			for (int i = 1; i <= l; i++)
			{
				addLength += " ";
			}
			s += addLength;
			return s.Substring(0, l);
		}


		public void DecodeUnitFlags(WowObject unit)
		{
			unit.IsInCombat = (unit.UnitFlags & Pointers.UnitFlags.UNIT_FLAG_IN_COMBAT) != 0;
			unit.IsFleeing = (unit.UnitFlags & Pointers.UnitFlags.UNIT_FLAG_FLEEING) != 0;
			unit.IsStunned = (unit.UnitFlags & Pointers.UnitFlags.UNIT_FLAG_STUNNED) != 0;
			unit.IsConfused = (unit.UnitFlags & Pointers.UnitFlags.UNIT_FLAG_CONFUSED) != 0;
			unit.CantMove = (unit.UnitFlags & Pointers.UnitFlags.UNIT_FLAG_DISABLE_MOVE) != 0;
		}


		public List<Auras> GetUnitBuffs(WowObject Unit)
		{
			List<Auras> objAuras = new List<Auras>();
			for (int i = 0; i <= 31; i++)
			{
				//Alternative: int id = *(int*)(*(uint*)(addr + 8) + auraPos * 4)   //the inner *(uint*) is a pointer to the descriptors
				uint buffid = wowMem.ReadUInt((uint)(Unit.ObjBaseAddress + 0xB58 + i * 4));   // for i = 0..47
				byte stacks = wowMem.ReadByte((uint)(Unit.ObjBaseAddress + 0x1e2c + i + 0x108));
				if (buffid != 0)
				{
					//Log.Print("Buff #" + i + ":\t " + (Unit.ObjBaseAddress + 0xB58 + (i * 4)).ToString("X") + "\t id: " + buffid.ToString() + " \t" + (Unit.ObjBaseAddress + 0x1e2c + 0x108 + i).ToString("X") + "Stacks: " + stacks);
					objAuras.Add(new Auras(GetSpellName(buffid), buffid,1,(uint)stacks + 1));
					//Log.Print(((uint)(LocalPlayer.ObjBaseAddress + 0xB58 + i * 4)).ToString("X") + ": \t" + buffid.ToString() + " \t" + i.ToString());
				}

			}
			return objAuras;
		}
		public List<Auras> GetUnitDebuffs(WowObject Unit)
		{
			List<Auras> objAuras = new List<Auras>();
			//Log.Print("Debuff Base:\t " + (Unit.ObjBaseAddress + 0xB58).ToString("X"));
			for (int i = 32; i <= 47; i++)
			{
				//Alternative: int id = *(int*)(*(uint*)(addr + 8) + auraPos * 4)   //the inner *(uint*) is a pointer to the descriptors
				uint buffid = wowMem.ReadUInt((uint)(Unit.ObjBaseAddress + 0xB58 + i * 4));   // for i = 0..47
				//byte stacks = wowMem.ReadByte((uint)(Unit.ObjBaseAddress + 0xFA4 + 0xA8 + i));
				byte stacks = wowMem.ReadByte((uint)(Unit.ObjBaseAddress + 0xF84 + i + 0xA8));
				if (buffid != 0)
				{
					//Log.Print("Debuff #"+i+":\t " + (Unit.ObjBaseAddress + 0xB58 + (i*4)).ToString("X")+ "\t id: " + buffid.ToString() + " \t" + (Unit.ObjBaseAddress + 0xF84 + 0xA8 + i).ToString("X") + "Stacks: " + stacks);
					//Log.Print("Debuff #"+i+":\t " + (Unit.ObjBaseAddress + 0xB58 + (i*4)).ToString("X")+ "\t id: " + buffid.ToString());
					objAuras.Add(new Auras(GetSpellName(buffid), buffid, 1, (uint)stacks + 1));
				}
			}
			return objAuras;
		}
		public bool UnitHasBreakableCc(WowObject Unit)
		{
			List<Auras> objAuras = new List<Auras>();
			//Log.Print("Debuff Base:\t " + (Unit.ObjBaseAddress + 0xB58).ToString("X"));
			for (int i = 32; i <= 47; i++)
			{
				//Alternative: int id = *(int*)(*(uint*)(addr + 8) + auraPos * 4)   //the inner *(uint*) is a pointer to the descriptors
				uint buffid = wowMem.ReadUInt((uint)(Unit.ObjBaseAddress + 0xB58 + i * 4));   // for i = 0..47
																							  //byte stacks = wowMem.ReadByte((uint)(Unit.ObjBaseAddress + 0xFA4 + 0xA8 + i));
				byte stacks = wowMem.ReadByte((uint)(Unit.ObjBaseAddress + 0xF84 + i + 0xA8));
				if (buffid != 0)
				{
					//Log.Print("Debuff #"+i+":\t " + (Unit.ObjBaseAddress + 0xB58 + (i*4)).ToString("X")+ "\t id: " + buffid.ToString() + " \t" + (Unit.ObjBaseAddress + 0xF84 + 0xA8 + i).ToString("X") + "Stacks: " + stacks);
					//Log.Print("Debuff #"+i+":\t " + (Unit.ObjBaseAddress + 0xB58 + (i*4)).ToString("X")+ "\t id: " + buffid.ToString());
					if (GetSpellName(buffid) == "Polymorph" || GetSpellName(buffid) == "Sap" || GetSpellName(buffid) == "Blind")
					{
						Unit.HasBreakableCc = true;
						return true;
					}
					else
					{
						Unit.HasBreakableCc = false;
					}
				}
			}
			return false;
		}
		public bool UnitHasBuff(WowObject unit, uint id)
		{
			return unit.BuffList.Exists(c => c.Id == id);
		}
		public bool UnitHasBuff(WowObject unit, string name)
		{
			return unit.BuffList.Exists(c => c.Name == name);
		}
		public bool UnitHasBuff2(WowObject unit, uint id)
		{
			return GetUnitBuffs(unit).Exists(c => c.Id == id);
		}
		public bool UnitHasBuff2(WowObject unit, string name)
		{
			return GetUnitBuffs(unit).Exists(c => c.Name == name);
		}

		public bool UnitHasDebuff(WowObject unit, uint id)
		{
			return unit.DebuffList.Exists(c => c.Id == id);
		}
		public bool UnitHasDebuff(WowObject unit, string name)
		{
			return unit.DebuffList.Exists(c => c.Name == name);
		}
		public bool UnitHasDebuff2(WowObject unit, uint id)
		{
			return GetUnitDebuffs(unit).Exists(c => c.Id == id);
		}
		public bool UnitHasDebuff2(WowObject unit, string name)
		{
			return GetUnitDebuffs(unit).Exists(c => c.Name == name);
		}

		public uint GetMainhandItemId()
		{
			return wowMem.ReadUInt((LocalPlayer.ObjBaseAddress + (uint)Pointers.WowObject.Mainhand));
		}
		public uint GetOffhandItemId()
		{
			return wowMem.ReadUInt((LocalPlayer.ObjBaseAddress + (uint)Pointers.WowObject.Offhand));
		}

        public uint GetStacksOfDebuff(WowObject unit, string aura)
        {
            Auras tempEmptyAura = new Auras("", 1, _stacks: 0);
            //Scans all Targets for "Sunder Armor"-Debuff and assign the stacks to the respective WowObject, if no Auras-Object, it returns stacks a new empty Object of Auras via Null-Coalesce Operator
            return (GetUnitDebuffs(unit).FirstOrDefault(c => c.Name == aura) ?? tempEmptyAura).Stacks;
        }

		public List<Spells> GetPlayerSpells()
		{
			//Log.Print("--GetPlayerSpells()");
			int SpellCounter = 0;
			SpellsInSpellBook.Clear();
			while (true)
			{
				SpellCounter++;
				var currentSpellId = wowMem.ReadUInt((uint)(Pointers.ObjectManager.CurPlayerSpellPtr + (SpellCounter * 4)));
				if (currentSpellId == 0) break;
				var entryPtr = wowMem.ReadUInt(wowMem.ReadUInt(0x00C0D780 + 8) + currentSpellId * 4);

				var entrySpellId = wowMem.ReadUInt(entryPtr);
				var namePtr = wowMem.ReadUInt(entryPtr + 0x1E0);
				var name = wowMem.ReadASCIIString(namePtr, 512); // Will default to ascii
				Spells spell = new Spells(name, 0, entrySpellId);
				SpellsInSpellBook.Add(spell);
			}
			// manually add attack
			Spells spell2 = new Spells("Attack", 0, 6603);
			SpellsInSpellBook.Add(spell2);
			//PrintSpells(SpellsInSpellBook);
			return SpellsInSpellBook;
		}

		public string tmpPrint(string name)
		{
			//Log.Print("°° " + name.ToString());
			return name;
		}

		public uint GetSpellIdFromSpellName(string name)
		{
			Spells result = SpellsInSpellBook.FirstOrDefault(C => C.Name == name);
			return result != null ? result.Id : 0;
		}

		public bool SpellExist(int spellId)
		{
			return SpellsInSpellBook.Exists(C => C.Id == spellId);
		}
		public bool SpellExist(string spellName)
		{
			return SpellsInSpellBook.Exists(C => C.Name == spellName);
		}

		private void PrintSpells(List<Spells> spellList)
		{
			foreach (Spells x in spellList)
			{
				Log.Print("ID: " + x.Id + " \t" + x.Name);
			}
		}

		public List<SpellsOnCooldown> GetPlayerSpellsOnCooldown()
		{
			//Log.Print("--GetPlayerSpellsOnCooldown");
			List <SpellsOnCooldown> SpellListOnCd = new List<SpellsOnCooldown>();
			//Current time in ms
			var currentTime = wowMem.ReadFloat((uint)Pointers.StaticAddresses.Timestamp) * 1000;

			//Get first list object
			var currentListObject = wowMem.ReadUInt((uint)Pointers.eSpellHistory.SpellHistory + (uint)Pointers.eSpellHistory.FirstRec);

			uint spellId = 0;
			float cooldown = 0;
			uint startTime = 0;//get record with latest starttime
			while ((currentListObject != 0) && ((currentListObject & 1) == 0))
			{		
				var currentStartTime = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.StartTime);
				if (currentStartTime > startTime) //get CD for the latest start time record
				{
					startTime = currentStartTime;
					// there exists 2 offsets with different values for CD, we check both.
					var spellCDx20 = wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellCoolDownx20);
					var spellCDx14 = wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellCoolDownx14);
					cooldown = Math.Max((float)Math.Round((startTime + spellCDx20 - currentTime) / 1000, 1), (float)Math.Round((startTime + spellCDx14 - currentTime) / 1000, 1));
					if (cooldown > 0)
					{
						spellId = (uint)wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellID);
						SpellsOnCooldown spellOnCd = new SpellsOnCooldown(spellId, cooldown);
						SpellListOnCd.Add(spellOnCd);
					}
				}
				//Get next list object
				currentListObject = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.NextRec);
			}
			return SpellListOnCd;


		}


		/*
		public void ScanSpells()
		{
			uint spellObjList = wowMem.ReadUInt((uint)0xCECAEC + (uint)0x8);
			while (spellObjList != 0u && (spellObjList & 1u) == 0u)
			{
				var spellId = wowMem.ReadUInt(spellObjList + 0x8);
				//Start time of the spell cooldown in ms        
				var startTime = wowMem.ReadUInt(spellObjList + 0x10);
				//Cooldown of spells with gcd
				var cooldown1 = wowMem.ReadUInt(spellObjList + 0x14);
				//Cooldown of spells without gcd
				var cooldown2 = wowMem.ReadUInt(spellObjList + 0x20);
				var enabled = wowMem.ReadUInt(spellObjList + 0x24);
				var globalLength = wowMem.ReadUInt(spellObjList + 0x2C);
				var length = wowMem.ReadUInt(spellObjList + 0x14) + wowMem.ReadUInt(spellObjList + 0x20);
				var cooldown3 = wowMem.ReadUInt(spellObjList + 0x30);
				var timestamp = wowMem.ReadFloat((uint)Pointers.StaticAddresses.Timestamp);
				//Log.Print(GetSpellName(spellId));
				//Log.Print("spellObj: " + (spellObjList).ToString("x") + "\tSpellID: " + spellId.ToString() );
				Log.Print("spellObj: " + (spellObjList).ToString("x") + "\tSpellID: " + spellId.ToString() + "\t" + (GetSpellName(spellId).ToString()+"----------").Substring(0,7) + "\ttimestamp: " + timestamp.ToString() + "\tcdCalc: " + ((startTime/1000) + (length / 1000) - timestamp).ToString() + "\tstart-cd: " + (startTime).ToString() + "\tcd1: " + cooldown1.ToString() + "\tcd2: " + cooldown2.ToString() + "\tcd3: " + cooldown3.ToString() + "\tglength: " + globalLength + "\tlength: " + length + "\tenabled: " + enabled);
				spellObjList = wowMem.ReadUInt((uint)spellObjList + (uint)0x4);
			}
		}
		*/

		public Dictionary<InGameKeyCombo, string> ReadBindingsCache(string file)
		{
			string readContents;
			string[] s;

			Dictionary<InGameKeyCombo, string> splittedBindingList = new Dictionary<InGameKeyCombo, string>();
			using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
			{
				readContents = streamReader.ReadToEnd().Substring(5);
			}
			s = System.Text.RegularExpressions.Regex.Split(readContents, "bind ");
			foreach (string z in s)
			{
				InGameKeyCombo tmpK = null;
				string[] t;
				string[] configKeyCombo;
				t = z.Split(' ');
				t[1] = t[1].Trim();
				configKeyCombo = t[0].Split('-');
				if (configKeyCombo.Length == 1)
				{
					tmpK = new InGameKeyCombo(configKeyCombo[0], "");
				}
				else if (configKeyCombo.Length == 2)
				{
					tmpK = new InGameKeyCombo(configKeyCombo[1], configKeyCombo[0]);
				}
				if (tmpK != null && t[1].Contains("ACTION") && !t[1].Contains("SELFACTION") && !t[1].Contains("BONUSACTION") && !t[1].Contains("ACTIONPAGE") && !t[1].Contains("TURNORACTION"))
				{
					splittedBindingList.Add(tmpK, t[1].Trim());
					//Log.Print("b: " + tmpK.Key + ":" + tmpK.Modifier + " \t" + t[1].Trim());
				}
			}
			return splittedBindingList;
		}

		public List<ActionButtonBinding> GetActionButtonBindings()
		{
			List<ActionButtonBinding> ActionButtonBindingList = new List<ActionButtonBinding>();
			var tmpEnumArray = Enum.GetValues(typeof(Pointers.ActionButtons));
			for (uint i = 0; i < tmpEnumArray.Length; i++) // count of enum
			{
				uint tmpValue = (uint)tmpEnumArray.GetValue(i);
				ActionButtonBinding temp = new ActionButtonBinding(tmpValue);
				temp.SpellId = (uint)wowMem.ReadInt(tmpValue);
				string[] tmpName = (Enum.GetName(typeof(Pointers.ActionButtons), tmpValue)).Split('_');
				temp.ActionButtonName = tmpName[0];
				int tmpStance = 0;
				if (tmpName != null && int.TryParse(tmpName[1], out tmpStance))
				{
					temp.Stance = (uint)tmpStance;
				}
				else
				{
					temp.Stance = 0;
				}
				ActionButtonBindingList.Add(temp);
				//Log.Print(" ## " + temp.ActionButtonName + ": \t" + temp.SpellId + " \t" + temp.ActionButtonOffset.ToString("X") + " \t" + temp.SpellName + " \t" + temp.Stance);

			}
			return ActionButtonBindingList;
		}

		public BindingList<ActionButtonAndSpell> CreateActionButtonData(Dictionary<InGameKeyCombo,string> igc, List<ActionButtonBinding> ab)
		{
			ActionButtonAndSpellList.Clear();
			for (int i = 0; i < igc.Count; i++)
			{
				foreach (ActionButtonBinding ActionButtonFromOffset in ab)
				{
					//Log.Print("%%%%%%" + ActionButtonFromOffset.ActionButtonName + ":" + igc.ElementAt(i).Value + " \t" + igc.ElementAt(i).Key.Key + "-" + igc.ElementAt(i).Key.Modifier + "\t" + ActionButtonFromOffset.SpellId);

					if (ActionButtonFromOffset.ActionButtonName == igc.ElementAt(i).Value && ActionButtonFromOffset.SpellId != 0)
					{
						//Log.Print("######" + ActionButtonFromOffset.ActionButtonName + ":" + igc.ElementAt(i).Value + " \t" + igc.ElementAt(i).Key.Key + "-" + igc.ElementAt(i).Key.Modifier);

						if (ActionButtonFromOffset.SpellId < 0x40000000)
						{
							ActionButtonFromOffset.SpellName = GetSpellName(ActionButtonFromOffset.SpellId);
						}
						else if (ActionButtonFromOffset.SpellId < 0x80000000)
						{
							ActionButtonFromOffset.SpellName = "Macro: " + (ActionButtonFromOffset.SpellId - 0x40000000);
							//ActionButtonFromOffset.SpellId = 0;
						}
						else if (ActionButtonFromOffset.SpellId >= 0x80000000)
						{
							ActionButtonFromOffset.SpellName = "Item: " + (ActionButtonFromOffset.SpellId - 0x80000000);
							//ActionButtonFromOffset.SpellId = 0;
						}

						//Log.Print("  || " + ActionButtonFromOffset.ActionButtonName + ":" + igc.ElementAt(i).Value + " \t {" + ActionButtonFromOffset.SpellId + "} \t" + ActionButtonFromOffset.ActionButtonOffset.ToString("X") + " \t" + igc.ElementAt(i).Key.Key + "-" + igc.ElementAt(i).Key.Modifier + " \t" + ActionButtonFromOffset.SpellName);
						//-------------------------------------------------------        spell name        |   The real Key ('Q','E' or so)     |  Modifier from cfg            | spell id              | Actionbuttonname from cfg		
						ActionButtonAndSpell abas = new ActionButtonAndSpell(ActionButtonFromOffset.SpellName, igc.ElementAt(i).Key.Key, igc.ElementAt(i).Key.Modifier, ActionButtonFromOffset.Stance, ActionButtonFromOffset.SpellId, igc.ElementAt(i).Value);
						ActionButtonAndSpellList.Add(abas);
					}

				}
			}
			return ActionButtonAndSpellList;
		}
		public float GetSpellCooldown(string spellName)
		{
			return GetSpellCooldown(GetSpellIdFromSpellName(spellName));
		}
		public float GetSpellCooldown(uint spellID)
		{
			//Current time in ms
			var currentTime = wowMem.ReadFloat((uint)Pointers.StaticAddresses.Timestamp)*1000;

			//Get first list object
			var currentListObject = wowMem.ReadUInt((uint)Pointers.eSpellHistory.SpellHistory + (uint)Pointers.eSpellHistory.FirstRec);

			float cooldown = 0;
			uint startTime = 0;//get record with latest starttime
			while ((currentListObject != 0) && ((currentListObject & 1) == 0))
			{

				if (wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellID) == spellID)//filter by ID here
				{
					var currentStartTime = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.StartTime);
					if (currentStartTime > startTime) //get CD for the latest start time record
					{
						startTime = currentStartTime;
						// there exists 2 offsets with different values for CD, we check both.
						var spellCDx20 = wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellCoolDownx20);
						var spellCDx14 = wowMem.ReadInt(currentListObject + (uint)Pointers.eSpellHistory.SpellCoolDownx14);
						cooldown = Math.Max((float)Math.Round((startTime + spellCDx20 - currentTime) / 1000, 1), (float)Math.Round((startTime + spellCDx14 - currentTime) / 1000, 1));
					}
				}
				//Get next list object
				currentListObject = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.NextRec);
			}
			return (cooldown < 0) ? 0 : cooldown;
		}

		public bool GlobalCooldown
		{
			get
			{
				//Current time in ms
				var currentTime = wowMem.ReadFloat((uint)Pointers.StaticAddresses.Timestamp)*1000;

				//Get first list object
				var currentListObject = wowMem.ReadUInt((uint)Pointers.eSpellHistory.SpellHistory + (uint)Pointers.eSpellHistory.FirstRec);

				while ((currentListObject != 0) && ((currentListObject & 1) == 0))
				{
					var bytes = wowMem.ReadBytes(currentListObject, 0x100);
					//Start time of the spell cooldown in ms
					var startTime = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.StartTime);

					//Absolute gcd of the spell in ms
					var globalCooldown = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.GlobalCooldown);

					//Spell on gcd?
					if ((startTime + globalCooldown) > currentTime)
						return true;


					//Get next list object
					currentListObject = wowMem.ReadUInt(currentListObject + (uint)Pointers.eSpellHistory.NextRec);
				}
				return false;
			}
		}


		public uint GetStance()
		{
			return wowMem.ReadUInt((uint)Pointers.StaticAddresses.Stance);
		}

		internal string GetSpellName(uint id)
		{
			try
			{
				uint descriptor = wowMem.ReadUInt(0x00C0D780 + 0x8);
				uint entryPtr = wowMem.ReadUInt(descriptor + id * 4);
				uint namePtr = wowMem.ReadUInt(entryPtr + 0x1E0);
				return wowMem.ReadASCIIString(namePtr, 512);
			}
			catch (Exception e)
			{
				Log.Print(e.ToString());
				return "unknown Spell :(";
			}
		}

	}
}
