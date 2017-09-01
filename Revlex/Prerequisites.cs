using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Revlex
{

	static class Prerequisites
	{

		private static string Filepath = Application.StartupPath + "\\classroutines";
		private static string[] ConfigData = new string[20];
		private static IniFile Config = new IniFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.ini");
		private static DirectoryInfo pathInfo = new DirectoryInfo(Filepath);


		public static List<string> GetAccountNames()
		{
			List<string> accountNames = new List<string>();


			return accountNames;
		}

		public static List<string> GetAllWowClassFiles()
		{
			List<string> wowClassFiles = new List<string>();
			try
			{
				foreach (var x in pathInfo.GetFiles("*.cs"))
				{
					Log.Print("file: " + x);
					wowClassFiles.Add(x.ToString());
				}
			}
			catch (Exception e)
			{
				Log.Print("Could not find any class file!", 4);
			}
			return wowClassFiles;
		}

		public static bool TryLoad(string setting, out string configValue)
		{
			try
			{
				configValue = Config.Read(setting);
				if (configValue == "")
				{
					Log.Print("No " + setting + " value in config found! Creating it ...");
					return false;
				}					
				return true;
			}
			catch
			{
				Log.Print("Couldnt read config for " + setting + " value!");
				configValue = "";
			}
			return false;
		}


		public static string[] LoadConfig()
		{
			// lastCvarOffset
			if (!TryLoad("lastCvarOffset", out ConfigData[0]))
			{
				Config.Write("lastCvarOffset", "0");
				Log.Print("no lastCvarOffset in config");
				ConfigData[0] = "0";
			}


			// lastPid
			if (!TryLoad("lastPid", out ConfigData[1]))
			{
				Config.Write("lastPid", "0");
				Log.Print("no lastPid in config");
				ConfigData[1] = "0";
			}

			// account name
			if (!TryLoad("account", out ConfigData[2]))
			{
				Log.Print("no account name in config");
				ConfigData[2] = "";
			}
			// wow folder
			if (!TryLoad("wow_folder", out ConfigData[3]))
			{
				Log.Print("no wow folder in config");
				ConfigData[3] = "";
			}
			// window position X
			if (!TryLoad("windowPosX", out ConfigData[4]))
			{
				Log.Print("no windowPosX in config");
				ConfigData[4] = "0";
			}
			// window position Y
			if (!TryLoad("windowPosY", out ConfigData[5]))
			{
				Log.Print("no windowPosY in config");
				ConfigData[5] = "0";
			}

			// Keybindings
			for (int i = 0; i < 11; i++)
			{
				if (!TryLoad("keybind_" + i.ToString(), out ConfigData[6 + i]))
				{
					Config.Write("keybind_" + i.ToString(), "None_None");
					Log.Print("no keybind_" + i.ToString() + " in config");
					ConfigData[6 + i] = "None_None";
				}
			}

			// hostile faction Alliance
			if (!TryLoad("HostileFactionAlliance", out ConfigData[17]))
			{
				Log.Print("no hostile faction for alliance in config");
				ConfigData[17] = "";
			}
			// hostile faction Horde
			if (!TryLoad("HostileFactionHorde", out ConfigData[18]))
			{
				Log.Print("no hostile faction for horde in config");
				ConfigData[18] = "";
			}
			return ConfigData;
		}


		public static void SaveConfigOffsets(int possibleOffset, int procId)
		{
			Config.Write("lastCvarOffset", possibleOffset.ToString());
			Config.Write("lastPid", procId.ToString());
		}
		public static void SaveConfigKeybind(int index, KeybindCombo keybind)
		{
			Log.Print("keybind_" + index.ToString() + "\t" + keybind.CompoundString);
			Config.Write("keybind_" + index.ToString(), keybind.CompoundString);
		}
		public static void SaveConfigAccountName(string acc)
		{
			Config.Write("account", acc);
		}
		public static void SaveConfigWowFolder(string folder)
		{
			Config.Write("wow_folder", folder);
		}
		public static void SaveConfigWindowPositionX(string x)
		{
			Config.Write("windowPosX", x);
		}
		public static void SaveConfigWindowPositionY(string y)
		{
			Config.Write("windowPosY", y);
		}
		public static void SaveHostileFactionAlliance(bool[] flist)
		{
			string factionString = "";
			int u = 0;
			foreach (bool f in flist)
			{
				if (f)
				{
					factionString += u.ToString() + ",";
				}
				u++;
			}
			Config.Write("HostileFactionAlliance", factionString);
		}
		public static void SaveHostileFactionHorde(bool[] flist)
		{
			string factionString = "";
			int u = 0;
			foreach (bool f in flist)
			{
				if (f)
				{
					factionString += u.ToString() + ",";
				}
				u++;
			}
			Config.Write("HostileFactionHorde", factionString);
		}
	}
}
