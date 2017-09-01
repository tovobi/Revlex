using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class KeybindCombo
	{
		public Action RotationFunc;
		public CustomRotation Rotation;
		public System.Windows.Forms.Keys Realkey;
		public uint Index
		{
			get
			{
				return _Index;
			}
			set
			{
				_Index = value;
				if (value == 1)
						RotationFunc = () => Rotation.Combat1();
				else if (value == 2)
					RotationFunc = () => Rotation.Combat2();
				else if (value == 3)
					RotationFunc = () => Rotation.Combat3();
				else if (value == 4)
					RotationFunc = () => Rotation.Combat4();
				else if (value == 5)
					RotationFunc = () => Rotation.Combat5();
				else if (value == 6)
					RotationFunc = () => Rotation.Combat6();
				else if (value == 7)
					RotationFunc = () => Rotation.Combat7();
				else if (value == 8)
					RotationFunc = () => Rotation.Combat8();
				else if (value == 9)
					RotationFunc = () => Rotation.Combat9();
				else if (value == 10)
					RotationFunc = () => Rotation.Combat10();
				else if (value == 11)
					RotationFunc = () => Rotation.Combat11();
			}
		}
		private uint _Index = 0;
		private List<int> AvailableKeys = new List<int>();
		private Dictionary<string, int> AvailableModifiers = new Dictionary<string, int>();
		public static Dictionary<int, int> ShiftedKeycodes = new Dictionary<int, int>();
		private string _Hotkey = "None";
		private string _Modifer = "None";

		public static int GetKeyCode(string s)
		{
			return (int)(s.ToCharArray()[0]);
		}
		public static int GetModifierValue(string s)
		{
			switch (s)
			{
				case "None": return 0;
				case "Alt": return 1;
				case "Ctrl": return 2;
				case "Control": return 4;
			}
			return 0;
		}

		public string Hotkey
		{
			get
			{
				return _Hotkey;
			}
			set
			{
				if (value == "" || value == "None")
				{
					value = "None";
					_Hotkey = "None";
				}
				else if (AvailableKeys.Contains((int)(value.ToLower().ToCharArray()[0])))
				{
					_Hotkey = value.ToLower();
				}
				else
				{
					_Hotkey = "None";
				}
			}
		}
		public string Modifer
		{
			get
			{
				return _Modifer;
			}
			set
			{
				if (value == "None" || value == "Alt" || value == "Control" || value == "Shift")
				{
					_Modifer = value;
				}
				else
				{
					_Modifer = "None";
				}
			}
		}

		public KeybindCombo(string _hotkey, string _modifier, uint _index, CustomRotation _rotation)
		{
			fillKeyList();
			fillModifiers();
			Hotkey = _hotkey;
			Modifer = _modifier;
			Index = _index;
			Rotation = _rotation;

			if (ShiftedKeycodes.Count == 0)
			{
				//0-9
				ShiftedKeycodes.Add(0x30, 0x3D);
				ShiftedKeycodes.Add(0x31, 0x21);
				ShiftedKeycodes.Add(0x32, 0x22);
				ShiftedKeycodes.Add(0x33, 0xA7);
				ShiftedKeycodes.Add(0x34, 0x24);
				ShiftedKeycodes.Add(0x35, 0x25);
				ShiftedKeycodes.Add(0x36, 0x26);
				ShiftedKeycodes.Add(0x37, 0x2f);
				ShiftedKeycodes.Add(0x38, 0x28);
				ShiftedKeycodes.Add(0x39, 0x29);
				//A-Z
				for (int i = 0x41; i < 0x5b; i++)
				{
					ShiftedKeycodes.Add(i + 0x20, i);
				}
				// special signs
				ShiftedKeycodes.Add(0xE4, 0xC4);    // ä Ä
				ShiftedKeycodes.Add(0xF6, 0xD6);    // ö Ö
				ShiftedKeycodes.Add(0xFC, 0xFC);    // ü Ü
				ShiftedKeycodes.Add(0x2B, 0x2A);    // + *
				ShiftedKeycodes.Add(0x23, 0x27);    // # '
				ShiftedKeycodes.Add(0x2D, 0x5F);    // - _
				ShiftedKeycodes.Add(0x2E, 0x3A);    // . :
				ShiftedKeycodes.Add(0x2C, 0x3B);    // , ;
				ShiftedKeycodes.Add(0x3C, 0x3E);    // < >
				ShiftedKeycodes.Add(0xDF, 0x3F);    // ß ?
				ShiftedKeycodes.Add(0xB4, 0x60);    // ´ `
				ShiftedKeycodes.Add(0x5e, 0xF8);    // ^ °
			}
		}

		private string _CompoundString;
		public string CompoundString
		{
			get
			{
				return Hotkey + "_" + Modifer;
			}
			set
			{
				if (value == null || value == "")
				{
					value = "None_None";
				}
				string temp1 = "";
				string temp2 = "";
				string[] s = value.Split('_');
				if (s[0] == "None" || AvailableKeys.Contains((int)(s[0].ToCharArray()[0])))
				{
					temp1 = s[0];
				}
				if (s[1] == "None" || s[1] == "Alt" || s[1] == "Control" || s[1] == "Shift")
				{
					temp2 = s[1];
				}
				if (temp1 != "" && temp2 != "")
				{
					_CompoundString = value;
					_Hotkey = temp1;
					_Modifer = temp2;
				}
				else
				{
					Log.Print("Couldnt create a compound keybind property (" + this.ToString() + ")");
					_CompoundString = "None_None";
					_Hotkey = "None";
					_Modifer = "None";
				}

			}
		}

		public int GetUTF8Hotkey()
		{
			if (Hotkey == "None") return 0;
			return (int)(Hotkey.ToCharArray().First());
		}
		public int GetIntModifier()
		{
			switch (Modifer)
			{
				case "None": return 0;
				case "Alt": return 1;
				case "Control": return 2;
				case "Shift": return 4;
			}
			return 0;
		}
		private void fillKeyList()
		{
			for (int i = 0x30; i < 0x3a; i++)
			{
				AvailableKeys.Add(i);
			}
			for (int i = 0x61; i < 0x7b; i++)
			{
				AvailableKeys.Add(i);
			}
			AvailableKeys.Add(0xE4);    // ä
			AvailableKeys.Add(0xF6);    // ö
			AvailableKeys.Add(0xFC);    // ü
			AvailableKeys.Add(0x2B);    // +
			AvailableKeys.Add(0x23);    // #
			AvailableKeys.Add(0x2D);    // -
			AvailableKeys.Add(0x2E);    // .
			AvailableKeys.Add(0x2C);    // ,
			AvailableKeys.Add(0x3C);    // <
			AvailableKeys.Add(0xDF);    // ß
			AvailableKeys.Add(0xB4);    // ´
			AvailableKeys.Add(0x5E);    // ^
		}
		private void fillModifiers()
		{
			AvailableModifiers.Add("None", 0);
			AvailableModifiers.Add("Alt", 1);
			AvailableModifiers.Add("Control", 2);
			AvailableModifiers.Add("Shift", 4);
		}

		



	}
}
