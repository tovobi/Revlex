using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;
using System.Windows.Forms;
using System.ComponentModel;

namespace Revlex
{
	public class ScanCvar
	{
		public static IniFile config;
		private static int lastCvarOffset;
		private static int lastPid;
		private static RichTextBox richDebug;
		private static Form1 parentForm;
		private static BlackMagic wowMem;
		private static List<CvarData>[] cvarList = {
			new List<CvarData>(),	// Spells
			new List<CvarData>(),	// Player Buffs
			new List<CvarData>(),	// Player Debuffs
			new List<CvarData>(),	// Target Buffs
			new List<CvarData>()		// Target Debuffs
		};
		/*
		public new Action[] DisplayCvarLists = new Action[] {
			()=> CvarPlayerBuffList(),
			()=> CvarPlayerDebuffList(),
			()=> CvarTargetBuffList(),
			()=> CvarTargetDebuffList()
		};
		*/
		public ScanCvar(BlackMagic _wowMem, IniFile _config, int _lastCvarOffset, int _lastPid, RichTextBox _richDebug, Form1 _parentForm)
		{
			wowMem = _wowMem;
			config = _config;
			lastCvarOffset = _lastCvarOffset;
			lastPid = _lastPid;
			richDebug = _richDebug;
			parentForm = _parentForm;
		}

		public static void ShowCvarList(System.Windows.Forms.DataGridView dataGrid, List<CvarData> data, int index)
		{
			int i = 0;
			dataGrid.Rows.Clear();
			foreach (CvarData item in data)
			{
				if (item.Name != "")
				{
					i++;
					dataGrid.Rows.Add(item.Name, item.Cd, item.Id);
				}
			}
			
			dataGrid.Sort(dataGrid.Columns[1], ListSortDirection.Descending);
		}
		//public static void ShowBuffList(DataGridView dataGrid, List<Auras> buffs)
		//{
		//	dataGrid.Rows.Clear();
		//	dataGrid.DataSource = buffs;
		//}

		public static void GenerateCompleteSpellList(DataGridView dataGrid, List<Spells> spells)
		{
			dataGrid.DataSource = spells.OrderByDescending(o => o.Cd).ToList();
		}


		public static List<Spells> UpdateSpellList(DataGridView dataGrid, List<Spells> spells, List<SpellsOnCooldown> spellsOnCd, WowHelpers WowHelperObj)
		{
			//loops through all spells with CD > 0 and sets them to 0
			while (spells.Find(o => o.Cd > 0) != null)
			{
				spells[spells.FindIndex(o => o.Cd > 0)].Cd = 0;
			}

			int count = 0;
			// loops through all spells on spellsOnCd and compare them with the regular spell list "spells" and set the cooldowns
			foreach (SpellsOnCooldown item in spellsOnCd)
			{
				try
				{
					spells[spells.FindIndex(o => o.Id == item.Id)].Cd = item.Cd;
				}
				catch (Exception e)
				{
					Log.Print("Error :-( " + e.Message + "\t(unknown spells in cd list like drinking or eating", 4);
				}
				count++;
			}
			List <Spells> newSpells = spells.OrderByDescending(o => o.Cd).ToList();
			dataGrid.DataSource = newSpells;
			return newSpells;
		}

		public static List<CvarData>[] GetCvarList(BlackMagic _wowMem, IniFile _config, int _lastCvarOffset, int _lastPid, RichTextBox _richDebug, Form1 _parentForm)
		{
			wowMem = _wowMem;
			config = _config;
			lastCvarOffset = _lastCvarOffset;
			lastPid = _lastPid;
			richDebug = _richDebug;
			parentForm = _parentForm;

			cvarList = MemRevlex.GetCvarData(System.Diagnostics.Process.GetProcessById(wowMem.ProcessId), parentForm.CvarOffset);

			return cvarList;
			// create datagridview for spells

		}
		

		
		public static void Scanner(BlackMagic _wowMem, IniFile _config, int _lastCvarOffset, int _lastPid, RichTextBox _richDebug, Form1 _parentForm)
		{
			wowMem = _wowMem;
			config = _config;
			lastCvarOffset = _lastCvarOffset;
			lastPid = _lastPid;
			richDebug = _richDebug;
			parentForm = _parentForm;
			// pattern: "RVXPAT!1"
			//spellCvarOffset = MemRevlex.MemoryScanner(System.Diagnostics.Process.GetProcessById(wowMem.ProcessId), new byte[] { 0x52, 0x56, 0x58, 0x50, 0x41, 0x54, 0x21, 0x31 }, 0x02F00000, 0x24FFFFFF, config, (uint)lastCvarOffset, (uint)lastPid, richDebug, parentForm, 0x200);
			MemRevlex.MemoryScanner(System.Diagnostics.Process.GetProcessById(wowMem.ProcessId), new byte[] { 0x52, 0x56, 0x58, 0x50, 0x41, 0x54, 0x21, 0x31 }, 0x02F00000, 0x24FFFFFF, config, (uint)lastCvarOffset, (uint)lastPid, richDebug, parentForm, 0x200);
			//SigScan scan = new SigScan(System.Diagnostics.Process.GetProcessById(wowMem.ProcessId), new IntPtr(0x03256803), 100);
			//IntPtr ptr = scan.FindPattern(new byte[] { 0x52, 0x56, 0x58, 0x50, 0x41, 0x54, 0x21, 0x31 }, "xxxxxxxx", 0);
			//Log.Print("SigScan: " + ptr.ToString("X"));
			//skip: if the pattern is found, the scanrange is adding with 0x200, because we wont find another pattern in the next 0x200 bytes, due the length of the spell-cvar
			//return (spellCvarOffset != 0) ? true : false; // if (spellCvarOffset != 0) { return true } else return false; //we dont need to return a var, because its the scan is eventbased and would always return 0 as offset

		}


	}
}
