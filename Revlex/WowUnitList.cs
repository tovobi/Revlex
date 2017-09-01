using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;
using System.Drawing;
using System.ComponentModel;

namespace Revlex
{
	public class WowUnitList
	{
		[DllImport("winmm.dll")]
		public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

		[DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);


		public List<WowListObject> RadarObjList = new List<WowListObject>();
		public ArrayList WowObjList = new ArrayList();
		public TextBox TxtSearchUnit;
		private DataGridView DataGridWowObjList;
		public int MaxRangeEnemyDetection;
		private int WaveVolume = 0;
		private int VolumeExpo = 30;
		private bool EnemyDetected = false;
		private bool EnemyTargetsMe = false;
		private bool VeinDetected = false;
		private bool SearchUnitDetected = false;
		private WowHelpers WowHelperObj;
		private WowObject CurrentObject = new WowObject();
		private WowListObject CurrentDataGridObject;
		public uint TotalWowObjects;
		//private DataGridViewTextBoxColumn ObjDistance;

		/// <summary>
		/// Constructor of WowUnitList
		/// </summary>
		/// <param name="_TxtSearchUnit">search field for to find units via name</param>
		/// <param name="_MaxRangeEnemyDetection"></param>
		/// <param name="_DataGridView1">DataGridView object of the list</param>
		/// <param name="_WowHelperObj">the WowHelper object</param>
		public WowUnitList(TextBox _TxtSearchUnit, int _MaxRangeEnemyDetection, DataGridView _dataGridWowObjList, WowHelpers _WowHelperObj)
		{
			TxtSearchUnit = _TxtSearchUnit;
			MaxRangeEnemyDetection = _MaxRangeEnemyDetection;
			DataGridWowObjList = _dataGridWowObjList;
			WowHelperObj = _WowHelperObj;



			// By the default set the volume to 0
			uint CurrVol = 0;
			// At this point, CurrVol gets assigned the volume
			waveOutGetVolume(IntPtr.Zero, out CurrVol);
			// Calculate the volume
			ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
			// Get the volume on a scale of 1 to 10 (to fit the trackbar)
			WaveVolume = CalcVol / (ushort.MaxValue / (MaxRangeEnemyDetection / VolumeExpo));
		}
		public void RefreshList(List<WowListObject> _radarObjList)
		{
			String[] allSearchUnitNames = TxtSearchUnit.Text.Split(',');
			int nearestEnemy = MaxRangeEnemyDetection;
			RadarObjList = _radarObjList;
			//DataGridView1.Rows.Clear();
			int i = 0;
			EnemyDetected = false;
			EnemyTargetsMe = false;
			VeinDetected = false;
			SearchUnitDetected = false;
			SoundPlayer simpleSound;


			//foreach (WowObject z in RadarObjList)
			//{
			//	if (z.Type == (short)Constants.ObjType.OT_PLAYER)
			//	{
			//		Log.Print("Na::: " + z.Name);
			//	}
			//}


			DataGridWowObjList.DataSource = RadarObjList.OrderBy(o => o.Distance).Take(20).ToList();
			DataGridWowObjList.Refresh();

			/*
			foreach (WowObject item in RadarObjList)
			{
				if (item.Name != "" && item.Name != null && item.Distance < MaxRangeEnemyDetection)
				{
					i++;
					DataGridView1.Rows.Add(i, item.Type, item.GameObjectType, item.Level, item.Name, item.FactionTemplate, item.FactionOffset, item.Distance);




					// check for search unit
					if (!SearchUnitDetected)
					{
						foreach (String tempStr in allSearchUnitNames)
						{
							if ((item.Name.ToLower() == tempStr || item.Name == tempStr || item.Name.ToUpper() == tempStr) && ((item.Type == 0 && item.CurrentHealth > 0) || (item.Type > 0)))
							{
								SearchUnitDetected = true;
								DataGridView1.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, Color.FromArgb(255, 255, 200));
							}
						}
					}

					// beep and check for Ore
					if (!VeinDetected && (item.Name.Contains("Vein") || item.Name.Contains("Deposit")))
					{
						DataGridView1.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, Color.FromArgb(200, 200, 200));
						VeinDetected = true;
					}

					// check for another Player
					if (item.Guid != WowHelperObj.LocalPlayer.Guid && item.Type == 4)
					{
						// check for Friend
						if (Constants.GetPlayerFaction((uint)WowHelperObj.LocalPlayer.FactionTemplate) == Constants.GetPlayerFaction((uint)item.FactionTemplate))
						{
							int enemyShiftColor = 200 + (10 * ((int)WowHelperObj.LocalPlayer.Level - (int)item.Level));
							enemyShiftColor = Math.Max(Math.Min(200, enemyShiftColor), 50);
							//Log.Print("#: " + enemyShiftColor);
							DataGridView1.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, Color.FromArgb(200, 255, 200));
						}

						// check for Enemy
						if (Constants.GetPlayerFaction((uint)item.FactionTemplate) != null && Constants.GetPlayerFaction((uint)WowHelperObj.LocalPlayer.FactionTemplate) != Constants.GetPlayerFaction((uint)item.FactionTemplate))
						{
							int enemyShiftColor = 200 + (10 * ((int)WowHelperObj.LocalPlayer.Level - (int)item.Level));
							enemyShiftColor = Math.Max(Math.Min(200, enemyShiftColor), 50);
							DataGridView1.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, Color.FromArgb(255, enemyShiftColor, enemyShiftColor));
							EnemyDetected = true;
							if (item.Distance < nearestEnemy)
							{
								nearestEnemy = (int)item.Distance;
							}
							if (item.Target == WowHelperObj.LocalPlayer.Guid)
							{
								EnemyTargetsMe = true;
							}
						}




					}
					// end check for another Player


				}
			}
			DataGridView1.Sort(ObjDistance, ListSortDirection.Ascending);
			*/
			/*
			// Beep on search Unit
			if (SearchUnitDetected)
			{
				// Calculate the volume that's being set. BTW: this is a trackbar!
				int NewVolume = ushort.MaxValue;
				// Set the same volume for both the left and the right channels
				uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
				// Set the volume
				waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);

				try
				{
					simpleSound = new SoundPlayer("c:/Windows/Media/detectunit.wav");
					simpleSound.PlaySync();
				}
				catch (Exception e)
				{
					Log.Print("Could not find sound file: " + e);
				}
			}

			// Beep on Vein
			if (VeinDetected)
			{
				// Calculate the volume that's being set. BTW: this is a trackbar!
				int NewVolume = ushort.MaxValue;
				// Set the same volume for both the left and the right channels
				uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
				// Set the volume
				waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);

				try
				{
					simpleSound = new SoundPlayer("c:/Windows/Media/testtvb.wav");
					simpleSound.PlaySync();
				}
				catch (Exception e)
				{
					Log.Print("Could not find sound file: " + e);
				}
			}



			// Beep on Enemy
			if (EnemyDetected)
			{
				// Calculate the volume that's being set. BTW: this is a trackbar!

				int tmp1 = MaxRangeEnemyDetection / VolumeExpo;
				int tmp2 = nearestEnemy / VolumeExpo;
				tmp2 = tmp2 + 1; // damit es keine Division durch 0 gibt
				int tmp3 = tmp1 / tmp2 / 3;
				int NewVolume = ((ushort.MaxValue / (tmp1)) * (tmp3));
				// Set the same volume for both the left and the right channels
				uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
				// Set the volume
				waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);

				try
				{
					if (EnemyTargetsMe)
					{
						simpleSound = new SoundPlayer("c:/Windows/Media/ringout.wav");
					}
					else
					{
						simpleSound = new SoundPlayer("c:/Windows/Media/chord.wav");
					}
					simpleSound.PlaySync();
				}
				catch (Exception e)
				{
					Log.Print("Could not find sound file: " + e);
				}
			}
			*/

		}


		public List<WowListObject> ScanObj()
		{
			//ArrayList list = new ArrayList();
			List<WowObject> list = new List<WowObject>();
			List<WowListObject> dataGridList = new List<WowListObject>();
			
			// set our counter variable to 0 so we can begin counting the objects
			TotalWowObjects = 0;
			CurrentObject = new WowObject();
			CurrentDataGridObject = new WowListObject();
			// set our current object as the first in the object manager
			CurrentObject.ObjBaseAddress = WowHelperObj.FirstObject;
			//Log.Print("CurrentObject.ObjBaseAddress: " + CurrentObject.ObjBaseAddress.ToString("X"));



			while ((CurrentObject.ObjBaseAddress & 1) == 0)
			{
				TotalWowObjects++;
				// type independent informations
				CurrentObject.Guid = WowHelperObj.wowMem.ReadUInt64((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Guid));
				//Log.Print("List: " + CurrentObject.Guid);
				CurrentObject.Type = (short)(WowHelperObj.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Type)));
				switch (CurrentObject.Type)
				{
					case (short)Constants.ObjType.OT_UNIT: // an npc
						CurrentObject.UnitFieldsAddress = WowHelperObj.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.Health = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
						CurrentObject.MaxHealth = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
						CurrentObject.SummonedBy = WowHelperObj.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.SummonedBy));
						CurrentObject.FactionTemplate = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
						CurrentObject.FactionOffset = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
						CurrentObject.TargetGuid = WowHelperObj.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));
						CurrentObject.Level = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));


						CurrentObject.XPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
						CurrentObject.YPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
						CurrentObject.ZPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));

						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						CurrentObject.Distance = Math.Round((WowHelperObj.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);

						CurrentObject.Rotation = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.RotationOffset));
						CurrentObject.Name = WowHelperObj.MobNameFromBaseAddr(CurrentObject.ObjBaseAddress);



						break;
					case (short)Constants.ObjType.OT_PLAYER: // a player
						CurrentObject.UnitFieldsAddress = WowHelperObj.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.Health = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Health));
						CurrentObject.MaxHealth = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.MaxHealth));
						CurrentObject.FactionTemplate = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionTemplate));
						CurrentObject.FactionOffset = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.FactionOffset));
						CurrentObject.TargetGuid = WowHelperObj.wowMem.ReadUInt64((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Target));
						CurrentObject.Level = WowHelperObj.wowMem.ReadUInt((CurrentObject.UnitFieldsAddress + (uint)Descriptors.WoWUnitFields.Level));

						CurrentObject.XPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.X));
						CurrentObject.YPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Y));
						CurrentObject.ZPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.Z));
						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						CurrentObject.Distance = Math.Round((WowHelperObj.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);

						CurrentObject.Rotation = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.RotationOffset));
						CurrentObject.Name = WowHelperObj.PlayerNameFromGuid(CurrentObject.Guid);
						break;

					case (short)Constants.ObjType.OT_GAMEOBJ:
						CurrentObject.XPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectX));
						CurrentObject.YPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectY));
						CurrentObject.ZPos = WowHelperObj.wowMem.ReadFloat((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.GameObjectZ));
						CurrentObject.vector3d = new WowVector3d(CurrentObject.XPos, CurrentObject.YPos, CurrentObject.ZPos);
						CurrentObject.Distance = Math.Round((WowHelperObj.LocalPlayer.vector3d.Distance(CurrentObject.vector3d)), 2);

						CurrentObject.UnitFieldsAddress = WowHelperObj.wowMem.ReadUInt((CurrentObject.ObjBaseAddress + (uint)Pointers.WowObject.DataPTR));
						CurrentObject.Name = WowHelperObj.ItemNameFromBaseAddr(CurrentObject.ObjBaseAddress);
						CurrentObject.GameObjectType = WowHelperObj.ItemTypeFromBaseAddr(CurrentObject.ObjBaseAddress);

						break;

				}


				Constants bla = new Constants();
				// --- Start > This Section was included to create a list only for datagrid 
				CurrentDataGridObject.Name = CurrentObject.Name;
				CurrentDataGridObject.Level = CurrentObject.Level;
				CurrentDataGridObject.Distance = CurrentObject.Distance;
				if (CurrentObject.Health > 0)
					CurrentDataGridObject.HealthPercent = (uint)(Math.Floor((double)(CurrentObject.Health / CurrentObject.MaxHealth*100)));
				CurrentDataGridObject.Type = CurrentObject.Type;
				CurrentDataGridObject.GameObjectType = CurrentObject.GameObjectType;
				CurrentDataGridObject.FactionTemplate = CurrentObject.FactionTemplate;
				CurrentDataGridObject.FactionOffset = CurrentObject.FactionOffset;
				// --- End > This Section was included to create a list only for datagrid 



				// set the current object as the next object in the object manager
				WowObject tmpObject = CurrentObject;
				WowListObject tmpDataGridObject = CurrentDataGridObject;
				CurrentObject = new WowObject();
				CurrentDataGridObject = new WowListObject();
				list.Add(tmpObject);
				if (tmpDataGridObject.Type == 3 || tmpDataGridObject.Type == 4 || tmpDataGridObject.Type == 5)
				{
					dataGridList.Add(tmpDataGridObject);
				}

				CurrentObject.ObjBaseAddress = WowHelperObj.wowMem.ReadUInt((tmpObject.ObjBaseAddress + (uint)Pointers.ObjectManager.NextObjectOffset));



			}

			return dataGridList;
		}

		

	}
}
