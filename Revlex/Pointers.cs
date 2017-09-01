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


//	STance: BC6E88

//Stance 1
//bc6aa0 Buttonbarstate 1
//bc69b0 Buttonbarstate 2

//Stance 2
//BC6AD0 Buttonbarstate 1
//BC69B0 Buttonbarstate 2

//Stance 2
//BC6B00 Buttonbarstate 1
//BC69B0 Buttonbarstate 2

	class Pointers
	{
		// GetTime
		// 0xC7B2A0
		// 0xCE9B98

		// static buff duration
		// 00CF0AA4



		#region Nested type: StaticAddresses
		/// <summary>
		///   1.12.1.5875
		/// </summary>
		internal enum StaticAddresses:uint
		{
			CurrentTargetGUID = 0x74e2d8,
			LocalPlayerGUID = 0x741e30,
			Timestamp = 0xC7B2A0,
			TextCaretActive = 0x884ca8,
			IsInGame = 0x00B4B424, // not validated, from :http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/328263-wow-1-12-1-5875-info-dump-thread-post2436167.html#post2436167
			Stance = 0x00BC6E88, // pure address, dont add baseaddress
		}

		#endregion

		#region Nested type: ObjectManager
		/// <summary>
		///   1.12.1.5875
		/// </summary>
		internal enum ObjectManager
		{
			CurMgrPointer = 0x00741414,
			CurMgrOffset = 0xAC,
			NextObjectOffset = 0x3c,
			FirstObjectOffset = 0xAC,
			LocalGUID = 0xC0,
			DescriptorOffset = 0x8,
			CurPlayerSpellPtr = 0x00B700F0,
			SpellHistory = 0xCECAEC
		}

		#endregion


		#region Nested type: UnitName
		/// <summary>
		///    1.12.1.5875
		/// </summary>

		internal enum UnitName : uint
		{
			ObjectName1 = 0x214, //pointing to itemtype of objectdescription
			ItemType = 0x2DC, // *DataPTR (0x288) + 0x54
			ObjectName2 = 0x8,
			UnitName1 = 0xB30,
			UnitName2 = 0x0,

			PlayerNameCachePointer = 0x80E230,
			PlayerNameGUIDOffset = 0xc,
			PlayerNameStringOffset = 0x14
		}

		#endregion

		#region Nested type: WowObject

		/// <summary>
		///   1.12.1.5875
		/// </summary>
		internal enum WowObject
		{
			DataPTR = 0x8,
			Type = 0x14,
			Guid = 0x30,
			Y = 0x9b8,
			X = Y + 0x4,
			Z = Y + 0x8,
            Facing = Z + 0x4, //Scheint zu funktionieren!			
			RotationOffset = X + 0x10,  // This seems to be wrong
			GameObjectY = 0x2C4, // *DataPTR (0x288) + 0x3c
			GameObjectX = GameObjectY + 0x4,
			GameObjectZ = GameObjectY + 0x8,
			Faction = 0x38,
			Speed = 0xA34,
			GuidOfAutoAttackTarget = 0xC48,
			CastSpell = 0xC8C,
			Mainhand = 0x2450,
			Offhand = 0x2480,
			PlayerClass = 0x1e01

		}

		#endregion

		#region DynamicFlags

		/// <summary>
		///   1.12.1.5875
		/// </summary>
		internal enum DynamicFlags:int
		{
			IsMarked = 0x2,
			CanBeLooted = 0xD,
			TappedByMe = 0xC,
			TappedByOther = 0x4,
			Untouched = 0x0,
			AuraBase = 0xBC,
		}

		#endregion

		public enum offsets
		{
			playerName = 0x827D88,
			targetGUID = 0x74E2D4,
		}
		public enum ActionButtons : uint
		{
			ACTIONBUTTON1_0 = 0x00BC6980,
			ACTIONBUTTON2_0 = ACTIONBUTTON1_0 + 0x4,
			ACTIONBUTTON3_0 = ACTIONBUTTON1_0 + 0x8,
			ACTIONBUTTON4_0 = ACTIONBUTTON1_0 + 0xC,
			ACTIONBUTTON5_0 = ACTIONBUTTON1_0 + 0x10,
			ACTIONBUTTON6_0 = ACTIONBUTTON1_0 + 0x14,
			ACTIONBUTTON7_0 = ACTIONBUTTON1_0 + 0x18,
			ACTIONBUTTON8_0 = ACTIONBUTTON1_0 + 0x1C,
			ACTIONBUTTON9_0 = ACTIONBUTTON1_0 + 0x20,
			ACTIONBUTTON10_0 = ACTIONBUTTON1_0 + 0x24,
			ACTIONBUTTON11_0 = ACTIONBUTTON1_0 + 0x28,
			ACTIONBUTTON12_0 = ACTIONBUTTON1_0 + 0x2C,

			ACTIONBUTTON1_1 = 0x00BC6AA0,
			ACTIONBUTTON2_1 = ACTIONBUTTON1_1 + 0x4,
			ACTIONBUTTON3_1 = ACTIONBUTTON1_1 + 0x8,
			ACTIONBUTTON4_1 = ACTIONBUTTON1_1 + 0xC,
			ACTIONBUTTON5_1 = ACTIONBUTTON1_1 + 0x10,
			ACTIONBUTTON6_1 = ACTIONBUTTON1_1 + 0x14,
			ACTIONBUTTON7_1 = ACTIONBUTTON1_1 + 0x18,
			ACTIONBUTTON8_1 = ACTIONBUTTON1_1 + 0x1C,
			ACTIONBUTTON9_1 = ACTIONBUTTON1_1 + 0x20,
			ACTIONBUTTON10_1 = ACTIONBUTTON1_1 + 0x24,
			ACTIONBUTTON11_1 = ACTIONBUTTON1_1 + 0x28,
			ACTIONBUTTON12_1 = ACTIONBUTTON1_1 + 0x2C,

			ACTIONBUTTON1_2 = 0x00BC6AD0,
			ACTIONBUTTON2_2 = ACTIONBUTTON1_2 + 0x4,
			ACTIONBUTTON3_2 = ACTIONBUTTON1_2 + 0x8,
			ACTIONBUTTON4_2 = ACTIONBUTTON1_2 + 0xC,
			ACTIONBUTTON5_2 = ACTIONBUTTON1_2 + 0x10,
			ACTIONBUTTON6_2 = ACTIONBUTTON1_2 + 0x14,
			ACTIONBUTTON7_2 = ACTIONBUTTON1_2 + 0x18,
			ACTIONBUTTON8_2 = ACTIONBUTTON1_2 + 0x1C,
			ACTIONBUTTON9_2 = ACTIONBUTTON1_2 + 0x20,
			ACTIONBUTTON10_2 = ACTIONBUTTON1_2 + 0x24,
			ACTIONBUTTON11_2 = ACTIONBUTTON1_2 + 0x28,
			ACTIONBUTTON12_2 = ACTIONBUTTON1_2 + 0x2C,

			ACTIONBUTTON1_3 = 0x00BC6B00,
			ACTIONBUTTON2_3 = ACTIONBUTTON1_3 + 0x4,
			ACTIONBUTTON3_3 = ACTIONBUTTON1_3 + 0x8,
			ACTIONBUTTON4_3 = ACTIONBUTTON1_3 + 0xC,
			ACTIONBUTTON5_3 = ACTIONBUTTON1_3 + 0x10,
			ACTIONBUTTON6_3 = ACTIONBUTTON1_3 + 0x14,
			ACTIONBUTTON7_3 = ACTIONBUTTON1_3 + 0x18,
			ACTIONBUTTON8_3 = ACTIONBUTTON1_3 + 0x1C,
			ACTIONBUTTON9_3 = ACTIONBUTTON1_3 + 0x20,
			ACTIONBUTTON10_3 = ACTIONBUTTON1_3 + 0x24,
			ACTIONBUTTON11_3 = ACTIONBUTTON1_3 + 0x28,
			ACTIONBUTTON12_3 = ACTIONBUTTON1_3 + 0x2C,

			MULTIACTIONBAR3BUTTON1_99 = 0x00BC69E0,
			MULTIACTIONBAR3BUTTON2_99 = MULTIACTIONBAR3BUTTON1_99 + 0x4,
			MULTIACTIONBAR3BUTTON3_99 = MULTIACTIONBAR3BUTTON1_99 + 0x8,
			MULTIACTIONBAR3BUTTON4_99 = MULTIACTIONBAR3BUTTON1_99 + 0xC,
			MULTIACTIONBAR3BUTTON5_99 = MULTIACTIONBAR3BUTTON1_99 + 0x10,
			MULTIACTIONBAR3BUTTON6_99 = MULTIACTIONBAR3BUTTON1_99 + 0x14,
			MULTIACTIONBAR3BUTTON7_99 = MULTIACTIONBAR3BUTTON1_99 + 0x18,
			MULTIACTIONBAR3BUTTON8_99 = MULTIACTIONBAR3BUTTON1_99 + 0x1C,
			MULTIACTIONBAR3BUTTON9_99 = MULTIACTIONBAR3BUTTON1_99 + 0x20,
			MULTIACTIONBAR3BUTTON10_99 = MULTIACTIONBAR3BUTTON1_99 + 0x24,
			MULTIACTIONBAR3BUTTON11_99 = MULTIACTIONBAR3BUTTON1_99 + 0x28,
			MULTIACTIONBAR3BUTTON12_99 = MULTIACTIONBAR3BUTTON1_99 + 0x2C,

			MULTIACTIONBAR4BUTTON1_99 = 0x00BC6A10,
			MULTIACTIONBAR4BUTTON2_99 = MULTIACTIONBAR4BUTTON1_99 + 0x4,
			MULTIACTIONBAR4BUTTON3_99 = MULTIACTIONBAR4BUTTON1_99 + 0x8,
			MULTIACTIONBAR4BUTTON4_99 = MULTIACTIONBAR4BUTTON1_99 + 0xC,
			MULTIACTIONBAR4BUTTON5_99 = MULTIACTIONBAR4BUTTON1_99 + 0x10,
			MULTIACTIONBAR4BUTTON6_99 = MULTIACTIONBAR4BUTTON1_99 + 0x14,
			MULTIACTIONBAR4BUTTON7_99 = MULTIACTIONBAR4BUTTON1_99 + 0x18,
			MULTIACTIONBAR4BUTTON8_99 = MULTIACTIONBAR4BUTTON1_99 + 0x1C,
			MULTIACTIONBAR4BUTTON9_99 = MULTIACTIONBAR4BUTTON1_99 + 0x20,
			MULTIACTIONBAR4BUTTON10_99 = MULTIACTIONBAR4BUTTON1_99 + 0x24,
			MULTIACTIONBAR4BUTTON11_99 = MULTIACTIONBAR4BUTTON1_99 + 0x28,
			MULTIACTIONBAR4BUTTON12_99 = MULTIACTIONBAR4BUTTON1_99 + 0x2C,

			MULTIACTIONBAR2BUTTON1_99 = 0x00BC6A40,
			MULTIACTIONBAR2BUTTON2_99 = MULTIACTIONBAR2BUTTON1_99 + 0x4,
			MULTIACTIONBAR2BUTTON3_99 = MULTIACTIONBAR2BUTTON1_99 + 0x8,
			MULTIACTIONBAR2BUTTON4_99 = MULTIACTIONBAR2BUTTON1_99 + 0xC,
			MULTIACTIONBAR2BUTTON5_99 = MULTIACTIONBAR2BUTTON1_99 + 0x10,
			MULTIACTIONBAR2BUTTON6_99 = MULTIACTIONBAR2BUTTON1_99 + 0x14,
			MULTIACTIONBAR2BUTTON7_99 = MULTIACTIONBAR2BUTTON1_99 + 0x18,
			MULTIACTIONBAR2BUTTON8_99 = MULTIACTIONBAR2BUTTON1_99 + 0x1C,
			MULTIACTIONBAR2BUTTON9_99 = MULTIACTIONBAR2BUTTON1_99 + 0x20,
			MULTIACTIONBAR2BUTTON10_99 = MULTIACTIONBAR2BUTTON1_99 + 0x24,
			MULTIACTIONBAR2BUTTON11_99 = MULTIACTIONBAR2BUTTON1_99 + 0x28,
			MULTIACTIONBAR2BUTTON12_99 = MULTIACTIONBAR2BUTTON1_99 + 0x2C,

			MULTIACTIONBAR1BUTTON1_99 = 0x00BC6A70,
			MULTIACTIONBAR1BUTTON2_99 = MULTIACTIONBAR1BUTTON1_99 + 0x4,
			MULTIACTIONBAR1BUTTON3_99 = MULTIACTIONBAR1BUTTON1_99 + 0x8,
			MULTIACTIONBAR1BUTTON4_99 = MULTIACTIONBAR1BUTTON1_99 + 0xC,
			MULTIACTIONBAR1BUTTON5_99 = MULTIACTIONBAR1BUTTON1_99 + 0x10,
			MULTIACTIONBAR1BUTTON6_99 = MULTIACTIONBAR1BUTTON1_99 + 0x14,
			MULTIACTIONBAR1BUTTON7_99 = MULTIACTIONBAR1BUTTON1_99 + 0x18,
			MULTIACTIONBAR1BUTTON8_99 = MULTIACTIONBAR1BUTTON1_99 + 0x1C,
			MULTIACTIONBAR1BUTTON9_99 = MULTIACTIONBAR1BUTTON1_99 + 0x20,
			MULTIACTIONBAR1BUTTON10_99 = MULTIACTIONBAR1BUTTON1_99 + 0x24,
			MULTIACTIONBAR1BUTTON11_99 = MULTIACTIONBAR1BUTTON1_99 + 0x28,
			MULTIACTIONBAR1BUTTON12_99 = MULTIACTIONBAR1BUTTON1_99 + 0x2C,
		}

		public enum eSpellHistory : uint
		{
			SpellHistory = 0xCECAEC,
			FirstRec = 0x8,
			NextRec = 0x4,
			StartTime = 0x10,
			GlobalCooldown = 0x30,
			SpellID = 0x8,
			SpellCoolDownx20 = 0x20,
			SpellCoolDownx14 = 0x14
		}

		[Flags]
		//public enum eWoWObjectType
		//{
		//	Object = 0x1,
		//	Item = 0x2,
		//	Container = 0x4,
		//	Unit = 0x8,
		//	Player = 0x10,
		//	GameObject = 0x20,
		//	DynamicObject = 0x40,
		//	Corpse = 0x80,
		//	AiGroup = 0x100,
		//	AreaTrigger = 0x200,
		//}
		internal enum WoWObjectTypes : byte
		{
			OT_NONE = 0,
			OT_ITEM = 1,
			OT_CONTAINER = 2,
			OT_UNIT = 3,
			OT_PLAYER = 4,
			OT_GAMEOBJ = 5,
			OT_DYNOBJ = 6,
			OT_CORPSE = 7
		}

		internal static class CreatureType
		{
			internal static int Beast = 1;
			internal static int Dragonkin = 2;
			internal static int Demon = 3;
			internal static int Elemental = 4;
			internal static int Giant = 5;
			internal static int Undead = 6;
			internal static int Humanoid = 7;
			internal static int Critter = 8;
			internal static int Mechanical = 9;
			internal static int NotSpecified = 10;
			internal static int Totem = 11;
		}


		//internal static class DynamicFlags
		//{
		//	internal static uint IsMarked = 0x2;
		//	internal static uint CanBeLooted = 0xD;
		//	internal static uint TappedByMe = 0xC;
		//	internal static uint TappedByOther = 0x4;
		//	internal static uint Untouched = 0x0;
		//	internal static int AuraBase = 0xBC;
		//	internal static int NextAura = 4;
		//	//internal static uint CanBeLooted = 0xD;
		//	//internal static uint TappedByMe = 0xC;
		//	//internal static uint TappedByOther = 0x4;
		//	//internal static uint Untouched = 0x0;

		//	internal static void AdjustToRealm()
		//	{
		//		var isElysium = Options.RealmList.Contains("elysium");
		//		if (!Options.RealmList.Contains("nostalrius") && !isElysium)
		//		{
		//			CanBeLooted = 0x1;
		//			TappedByMe = 0x0;
		//		}
		//		if (Options.RealmList.Contains("vanillagaming"))
		//		{
		//			AuraBase = 0x138;
		//			NextAura = -4;
		//		}
		//	}
		//}

		internal static class UnitFlags
		{
			internal static int UNIT_FLAG_FLEEING = 0x00800000;
			internal static int UNIT_FLAG_CONFUSED = 0x00400000;
			internal static int UNIT_FLAG_IN_COMBAT = 0x00080000;
			internal static int UNIT_FLAG_SKINNABLE = 0x04000000;
			internal static int UNIT_FLAG_STUNNED = 0x00040000;
			internal static int UNIT_FLAG_DISABLE_MOVE = 0x00000004;
		}

		internal enum MovementFlags : uint
		{
			None = 0x0,
			Front = 0x00000001,
			Back = 0x00000002,
			Left = 0x00000010,
			Right = 0x00000020,
			StrafeLeft = 0x00000004,
			StrafeRight = 0x00000008,

			Swimming = 0x00200000,
			jumping = 0x00002000,
			Falling = 0x0000A000,
			Levitate = 0x70000000
		}

		[Flags]
		internal enum ControlBits : uint
		{
			All = Front | Right | Left | StrafeLeft | StrafeRight | Back,
			Nothing = 0x0,
			CtmWalk = 0x00001000,
			Front = 0x00000010,
			Back = 0x00000020,
			Left = 0x00000100,
			Right = 0x00000200,
			StrafeLeft = 0x00000040,
			StrafeRight = 0x00000080
		}

		internal enum ControlBitsMouse : uint
		{
			Rightclick = 0x00000001,
			Leftclick = 0x00000002
		}

		internal enum ChatType
		{
			Say = 0,
			Yell = 5,
			Channel = 14,
			Group = 1,
			Guild = 3,
			Whisper = 6
		}


		internal enum UnitReaction : uint
		{
			Neutral = 3,
			Friendly = 4,

			// Guards of the other faction are for example hostile 2.
			// All other hostile mobs I met are just hostile.
			Hostile = 1,
			Hostile2 = 0
		}
		// Faction & isfriendly: http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/308386-if-unit-hostile-post1968502.html#post1968502
		// vielleicht doch LUA-Funktionen aufrufen?
		//https://shynd.wordpress.com/2008/06/15/getunitreaction/#more-20
		public static Dictionary<uint, string> PlayerClass = new Dictionary<uint, string>
		{
			{ 0,"None" },
			{ 1,"Warrior" },
			{ 2,"Paladin" },
			{ 3,"Hunter" },
			{ 4,"Rogue" },
			{ 5,"Priest" },
			{ 7,"Shaman" },
			{ 8,"Mage" },
			{ 9,"Warlock" },
			{ 11,"Druid" }
		};



		internal enum ItemQuality
		{
			Grey = 0,
			White = 1,
			Green = 2,
			Blue = 3,
			Epic = 4
		}


		internal enum CtmType : uint
		{
			FaceTarget = 0x1,
			Face = 0x2,

			/// <summary>
			///     Will throw a UI error. Have not figured out how to avoid it!
			/// </summary>
			// ReSharper disable InconsistentNaming
			Stop_ThrowsException = 0x3,
			// ReSharper restore InconsistentNaming
			Move = 0x4,
			NpcInteract = 0x5,
			Loot = 0x6,
			ObjInteract = 0x7,
			FaceOther = 0x8,
			Skin = 0x9,
			AttackPosition = 0xA,
			AttackGuid = 0xB,
			ConstantFace = 0xC,
			None = 0xD,
			Attack = 0x10,
			Idle = 0xC
		}



	}
}
