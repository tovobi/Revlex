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
	class Descriptors
	{
		/// <summary>
		///    1.12.1.5875
		/// </summary>
		/// im Thread http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/328263-wow-1-12-1-5875-info-dump-thread-4.html
		/// sind die WoWunitfileds mit "Objectfields.Object_End" angegeben d.h. die müssen mit +18 addiert werden
		/// bei playerfields sind es 0x2D8 .. also playerfieldzeugs + 2d8
		internal enum WoWUnitFields
		{
			Charm = 0x18,
			Summon = 0x20,
			CharmedBy = 0x28,
			SummonedBy = 0x30,
			CreatedBy = 0x38,
			Target = 0x40,
			ChannelObject = 0x50,
			Health = 0x58,
			//Power = 0x5c,
			MaxHealth = 0x70,
			//MaxPower = 0x74,
			Level = 0x88,
			FactionTemplate = 0x8c,
			FactionOffset = (0xC + 0x2d8),
			UnitFlags = 0x88,
			CombatReach = 0x1d8,
			DynamicFlags = 0x23C,
			ChannelSpell = 0x240,
			CreatedBySpell = (0x230 - 0x18),
			UnitNpcFlags = (0x234 - 0x18),
			Dodged = 0x1F4,
			InCombat = 0x23c,											// selber entdeckt, stimmt das?
			UNIT_FIELD_AURA = (0x0A4 + 0x18),                                 // 0x00A4
			UNIT_FIELD_AURAFLAGS = (0x0164 + 0x18),                             // 0x0164
			UNIT_FIELD_AURALEVELS = (0x017c +0x18),                           // 0x017C
																		// Faction & isfriendly: http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/308386-if-unit-hostile-post1968502.html#post1968502
																		// vielleicht doch LUA-Funktionen aufrufen?
																		//https://shynd.wordpress.com/2008/06/15/getunitreaction/#more-20

			// ripped from Zzuk
			//DynamicFlags = 0x23C,
			Flags = 0xB8,

			CreatedByGuid = 0x38,
			GameObjectCreatedByGuid = 0x18,

			movementFlags = 0x9E8,

			XHealth = 0x58,
			XMaxHealth = 0x70,
			FactionId = 0x8C,
			Mana = 0x5C,
			MaxMana = 0x74,
			Rage = 0x60,
			Energy = 0x68,
			TargetGuid = 0x40,
			CorpseOwnedBy = 0x18,

			ItemId = 0xC,
			ItemDurability = 0xB8,
			ItemMaxDurability = 0xBC,
			ItemStackCount = 0x38,


			MountDisplayId = 0x214,

			ContainerTotalSlots = 0x6c8,
			CorpseX = 0x24,
			CorpseY = 0x28,
			CorpseZ = 0x2c,

			NextLevelXp = 0xB34,
			CurrentXp = 0xB30
		}
	}
}

/* nützliches zeug:


EDIT: After taking a break to watch some trailer park boys I got back to it and thanks to CE and a hell of a lot of scanning I found:
Player Base Address + Offset 0xC48 = GUID of Auto-Attack Target

So this only shows a guid at that address if you are actively attempting to Attack (spell id 6603) aka Auto-Attack in retail WoW a mob.
Pretty useful and close-enough to what I was looking for to get the job done. 
http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/328263-wow-1-12-1-5875-info-dump-thread-17.html#post3748717


MouseOver SpellID 0xB4B3C4 -> 0x39C 
*/

//http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/328263-wow-1-12-1-5875-info-dump-thread-22.html#post3450098
/*
struct ObjectFields
{
	uint64 OBJECT_FIELD_GUID;                                   // 0x0000
	uint32 OBJECT_FIELD_TYPE;                                   // 0x0008
	uint32 OBJECT_FIELD_ENTRY;                                  // 0x000C
	float  OBJECT_FIELD_SCALE_X;                                // 0x0010
	uint32 OBJECT_FIELD_PADDING;                                // 0x0014
};

struct ItemFields
{
	uint64 ITEM_FIELD_OWNER;                                    // 0x0000
	uint64 ITEM_FIELD_CONTAINED;                                // 0x0008
	uint64 ITEM_FIELD_CREATOR;                                  // 0x0010
	uint64 ITEM_FIELD_GIFTCREATOR;                              // 0x0018
	uint32 ITEM_FIELD_STACK_COUNT;                              // 0x0020
	uint32 ITEM_FIELD_DURATION;                                 // 0x0024
	uint32 ITEM_FIELD_SPELL_CHARGES[5];                         // 0x0028
	uint32 ITEM_FIELD_FLAGS;                                    // 0x003C
	uint32 ITEM_FIELD_ENCHANTMENT[21];                          // 0x0040
	uint32 ITEM_FIELD_PROPERTY_SEED;                            // 0x0094
	uint32 ITEM_FIELD_RANDOM_PROPERTIES_ID;                     // 0x0098
	uint32 ITEM_FIELD_ITEM_TEXT_ID;                             // 0x009C
	uint32 ITEM_FIELD_DURABILITY;                               // 0x00A0
	uint32 ITEM_FIELD_MAXDURABILITY;                            // 0x00A4
};

struct ContainerFields
{
	uint32 CONTAINER_FIELD_NUM_SLOTS;                           // 0x0000
	uint32 CONTAINER_ALIGN_PAD;                                 // 0x0004
	uint32 CONTAINER_FIELD_SLOT_1[72];                          // 0x0008
};

struct UnitFields
{
	uint64 UNIT_FIELD_CHARM;                                    // 0x0000
	uint64 UNIT_FIELD_SUMMON;                                   // 0x0008
	uint64 UNIT_FIELD_CHARMEDBY;                                // 0x0010
	uint64 UNIT_FIELD_SUMMONEDBY;                               // 0x0018
	uint64 UNIT_FIELD_CREATEDBY;                                // 0x0020
	uint64 UNIT_FIELD_TARGET;                                   // 0x0028
	uint64 UNIT_FIELD_PERSUADED;                                // 0x0030
	uint64 UNIT_FIELD_CHANNEL_OBJECT;                           // 0x0038
	uint32 UNIT_FIELD_HEALTH;                                   // 0x0040
	uint32 UNIT_FIELD_POWER1;                                   // 0x0044
	uint32 UNIT_FIELD_POWER2;                                   // 0x0048
	uint32 UNIT_FIELD_POWER3;                                   // 0x004C
	uint32 UNIT_FIELD_POWER4;                                   // 0x0050
	uint32 UNIT_FIELD_POWER5;                                   // 0x0054
	uint32 UNIT_FIELD_MAXHEALTH;                                // 0x0058
	uint32 UNIT_FIELD_MAXPOWER1;                                // 0x005C
	uint32 UNIT_FIELD_MAXPOWER2;                                // 0x0060
	uint32 UNIT_FIELD_MAXPOWER3;                                // 0x0064
	uint32 UNIT_FIELD_MAXPOWER4;                                // 0x0068
	uint32 UNIT_FIELD_MAXPOWER5;                                // 0x006C
	uint32 UNIT_FIELD_LEVEL;                                    // 0x0070
	uint32 UNIT_FIELD_FACTIONTEMPLATE;                          // 0x0074
	uint32 UNIT_FIELD_BYTES_0;                                  // 0x0078
	uint32 UNIT_VIRTUAL_ITEM_SLOT_DISPLAY[3];                   // 0x007C
	uint32 UNIT_VIRTUAL_ITEM_INFO[6];                           // 0x0088
	uint32 UNIT_FIELD_FLAGS;                                    // 0x00A0
	uint32 UNIT_FIELD_AURA[48];                                 // 0x00A4
	uint32 UNIT_FIELD_AURAFLAGS[6];                             // 0x0164
	uint32 UNIT_FIELD_AURALEVELS[12];                           // 0x017C
	uint32 UNIT_FIELD_AURAAPPLICATIONS[12];                     // 0x01AC
	uint32 UNIT_FIELD_AURASTATE;                                // 0x01DC
	uint64 UNIT_FIELD_BASEATTACKTIME;                           // 0x01E0
	uint32 UNIT_FIELD_RANGEDATTACKTIME;                         // 0x01E8
	uint32 UNIT_FIELD_BOUNDINGRADIUS;                           // 0x01EC
	uint32 UNIT_FIELD_COMBATREACH;                              // 0x01F0
	uint32 UNIT_FIELD_DISPLAYID;                                // 0x01F4
	uint32 UNIT_FIELD_NATIVEDISPLAYID;                          // 0x01F8
	uint32 UNIT_FIELD_MOUNTDISPLAYID;                           // 0x01FC
	uint32 UNIT_FIELD_MINDAMAGE;                                // 0x0200
	uint32 UNIT_FIELD_MAXDAMAGE;                                // 0x0204
	uint32 UNIT_FIELD_MINOFFHANDDAMAGE;                         // 0x0208
	uint32 UNIT_FIELD_MAXOFFHANDDAMAGE;                         // 0x020C
	uint32 UNIT_FIELD_BYTES_1;                                  // 0x0210
	uint32 UNIT_FIELD_PETNUMBER;                                // 0x0214
	uint32 UNIT_FIELD_PET_NAME_TIMESTAMP;                       // 0x0218
	uint32 UNIT_FIELD_PETEXPERIENCE;                            // 0x021C
	uint32 UNIT_FIELD_PETNEXTLEVELEXP;                          // 0x0220
	uint32 UNIT_DYNAMIC_FLAGS;                                  // 0x0224
	uint32 UNIT_CHANNEL_SPELL;                                  // 0x0228
	uint32 UNIT_MOD_CAST_SPEED;                                 // 0x022C
	uint32 UNIT_CREATED_BY_SPELL;                               // 0x0230
	uint32 UNIT_NPC_FLAGS;                                      // 0x0234
	uint32 UNIT_NPC_EMOTESTATE;                                 // 0x0238
	uint32 UNIT_TRAINING_POINTS;                                // 0x023C
	uint32 UNIT_FIELD_STAT0;                                    // 0x0240
	uint32 UNIT_FIELD_STAT1;                                    // 0x0244
	uint32 UNIT_FIELD_STAT2;                                    // 0x0248
	uint32 UNIT_FIELD_STAT3;                                    // 0x024C
	uint32 UNIT_FIELD_STAT4;                                    // 0x0250
	uint32 UNIT_FIELD_RESISTANCES[7];                           // 0x0254
	uint32 UNIT_FIELD_BASE_MANA;                                // 0x0270
	uint32 UNIT_FIELD_BASE_HEALTH;                              // 0x0274
	uint32 UNIT_FIELD_BYTES_2;                                  // 0x0278
	uint32 UNIT_FIELD_ATTACK_POWER;                             // 0x027C
	uint32 UNIT_FIELD_ATTACK_POWER_MODS;                        // 0x0280
	uint32 UNIT_FIELD_ATTACK_POWER_MULTIPLIER;                  // 0x0284
	uint32 UNIT_FIELD_RANGED_ATTACK_POWER;                      // 0x0288
	uint32 UNIT_FIELD_RANGED_ATTACK_POWER_MODS;                 // 0x028C
	uint32 UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER;           // 0x0290
	uint32 UNIT_FIELD_MINRANGEDDAMAGE;                          // 0x0294
	uint32 UNIT_FIELD_MAXRANGEDDAMAGE;                          // 0x0298
	uint32 UNIT_FIELD_POWER_COST_MODIFIER[7];                   // 0x029C
	uint32 UNIT_FIELD_POWER_COST_MULTIPLIER[7];                 // 0x02B8
	uint32 UNIT_FIELD_PADDING;                                  // 0x02D4
};

struct PlayerFields
{
	uint64 PLAYER_DUEL_ARBITER;                                 // 0x0000
	uint32 PLAYER_FLAGS;                                        // 0x0008
	uint32 PLAYER_GUILDID;                                      // 0x000C
	uint32 PLAYER_GUILDRANK;                                    // 0x0010
	uint32 PLAYER_BYTES;                                        // 0x0014
	uint32 PLAYER_BYTES_2;                                      // 0x0018
	uint32 PLAYER_BYTES_3;                                      // 0x001C
	uint32 PLAYER_DUEL_TEAM;                                    // 0x0020
	uint32 PLAYER_GUILD_TIMESTAMP;                              // 0x0024
	uint32 PLAYER_QUEST_LOG_1_1;                                // 0x0028
	uint64 PLAYER_QUEST_LOG_1_2;                                // 0x002C
	uint32 PLAYER_QUEST_LOG_2_1;                                // 0x0034
	uint64 PLAYER_QUEST_LOG_2_2;                                // 0x0038
	uint32 PLAYER_QUEST_LOG_3_1;                                // 0x0040
	uint64 PLAYER_QUEST_LOG_3_2;                                // 0x0044
	uint32 PLAYER_QUEST_LOG_4_1;                                // 0x004C
	uint64 PLAYER_QUEST_LOG_4_2;                                // 0x0050
	uint32 PLAYER_QUEST_LOG_5_1;                                // 0x0058
	uint64 PLAYER_QUEST_LOG_5_2;                                // 0x005C
	uint32 PLAYER_QUEST_LOG_6_1;                                // 0x0064
	uint64 PLAYER_QUEST_LOG_6_2;                                // 0x0068
	uint32 PLAYER_QUEST_LOG_7_1;                                // 0x0070
	uint64 PLAYER_QUEST_LOG_7_2;                                // 0x0074
	uint32 PLAYER_QUEST_LOG_8_1;                                // 0x007C
	uint64 PLAYER_QUEST_LOG_8_2;                                // 0x0080
	uint32 PLAYER_QUEST_LOG_9_1;                                // 0x0088
	uint64 PLAYER_QUEST_LOG_9_2;                                // 0x008C
	uint32 PLAYER_QUEST_LOG_10_1;                               // 0x0094
	uint64 PLAYER_QUEST_LOG_10_2;                               // 0x0098
	uint32 PLAYER_QUEST_LOG_11_1;                               // 0x00A0
	uint64 PLAYER_QUEST_LOG_11_2;                               // 0x00A4
	uint32 PLAYER_QUEST_LOG_12_1;                               // 0x00AC
	uint64 PLAYER_QUEST_LOG_12_2;                               // 0x00B0
	uint32 PLAYER_QUEST_LOG_13_1;                               // 0x00B8
	uint64 PLAYER_QUEST_LOG_13_2;                               // 0x00BC
	uint32 PLAYER_QUEST_LOG_14_1;                               // 0x00C4
	uint64 PLAYER_QUEST_LOG_14_2;                               // 0x00C8
	uint32 PLAYER_QUEST_LOG_15_1;                               // 0x00D0
	uint64 PLAYER_QUEST_LOG_15_2;                               // 0x00D4
	uint32 PLAYER_QUEST_LOG_16_1;                               // 0x00DC
	uint64 PLAYER_QUEST_LOG_16_2;                               // 0x00E0
	uint32 PLAYER_QUEST_LOG_17_1;                               // 0x00E8
	uint64 PLAYER_QUEST_LOG_17_2;                               // 0x00EC
	uint32 PLAYER_QUEST_LOG_18_1;                               // 0x00F4
	uint64 PLAYER_QUEST_LOG_18_2;                               // 0x00F8
	uint32 PLAYER_QUEST_LOG_19_1;                               // 0x0100
	uint64 PLAYER_QUEST_LOG_19_2;                               // 0x0104
	uint32 PLAYER_QUEST_LOG_20_1;                               // 0x010C
	uint64 PLAYER_QUEST_LOG_20_2;                               // 0x0110
	uint64 PLAYER_VISIBLE_ITEM_1_CREATOR;                       // 0x0118
	uint32 PLAYER_VISIBLE_ITEM_1_0[8];                          // 0x0120
	uint32 PLAYER_VISIBLE_ITEM_1_PROPERTIES;                    // 0x0140
	uint32 PLAYER_VISIBLE_ITEM_1_PAD;                           // 0x0144
	uint64 PLAYER_VISIBLE_ITEM_2_CREATOR;                       // 0x0148
	uint32 PLAYER_VISIBLE_ITEM_2_0[8];                          // 0x0150
	uint32 PLAYER_VISIBLE_ITEM_2_PROPERTIES;                    // 0x0170
	uint32 PLAYER_VISIBLE_ITEM_2_PAD;                           // 0x0174
	uint64 PLAYER_VISIBLE_ITEM_3_CREATOR;                       // 0x0178
	uint32 PLAYER_VISIBLE_ITEM_3_0[8];                          // 0x0180
	uint32 PLAYER_VISIBLE_ITEM_3_PROPERTIES;                    // 0x01A0
	uint32 PLAYER_VISIBLE_ITEM_3_PAD;                           // 0x01A4
	uint64 PLAYER_VISIBLE_ITEM_4_CREATOR;                       // 0x01A8
	uint32 PLAYER_VISIBLE_ITEM_4_0[8];                          // 0x01B0
	uint32 PLAYER_VISIBLE_ITEM_4_PROPERTIES;                    // 0x01D0
	uint32 PLAYER_VISIBLE_ITEM_4_PAD;                           // 0x01D4
	uint64 PLAYER_VISIBLE_ITEM_5_CREATOR;                       // 0x01D8
	uint32 PLAYER_VISIBLE_ITEM_5_0[8];                          // 0x01E0
	uint32 PLAYER_VISIBLE_ITEM_5_PROPERTIES;                    // 0x0200
	uint32 PLAYER_VISIBLE_ITEM_5_PAD;                           // 0x0204
	uint64 PLAYER_VISIBLE_ITEM_6_CREATOR;                       // 0x0208
	uint32 PLAYER_VISIBLE_ITEM_6_0[8];                          // 0x0210
	uint32 PLAYER_VISIBLE_ITEM_6_PROPERTIES;                    // 0x0230
	uint32 PLAYER_VISIBLE_ITEM_6_PAD;                           // 0x0234
	uint64 PLAYER_VISIBLE_ITEM_7_CREATOR;                       // 0x0238
	uint32 PLAYER_VISIBLE_ITEM_7_0[8];                          // 0x0240
	uint32 PLAYER_VISIBLE_ITEM_7_PROPERTIES;                    // 0x0260
	uint32 PLAYER_VISIBLE_ITEM_7_PAD;                           // 0x0264
	uint64 PLAYER_VISIBLE_ITEM_8_CREATOR;                       // 0x0268
	uint32 PLAYER_VISIBLE_ITEM_8_0[8];                          // 0x0270
	uint32 PLAYER_VISIBLE_ITEM_8_PROPERTIES;                    // 0x0290
	uint32 PLAYER_VISIBLE_ITEM_8_PAD;                           // 0x0294
	uint64 PLAYER_VISIBLE_ITEM_9_CREATOR;                       // 0x0298
	uint32 PLAYER_VISIBLE_ITEM_9_0[8];                          // 0x02A0
	uint32 PLAYER_VISIBLE_ITEM_9_PROPERTIES;                    // 0x02C0
	uint32 PLAYER_VISIBLE_ITEM_9_PAD;                           // 0x02C4
	uint64 PLAYER_VISIBLE_ITEM_10_CREATOR;                      // 0x02C8
	uint32 PLAYER_VISIBLE_ITEM_10_0[8];                         // 0x02D0
	uint32 PLAYER_VISIBLE_ITEM_10_PROPERTIES;                   // 0x02F0
	uint32 PLAYER_VISIBLE_ITEM_10_PAD;                          // 0x02F4
	uint64 PLAYER_VISIBLE_ITEM_11_CREATOR;                      // 0x02F8
	uint32 PLAYER_VISIBLE_ITEM_11_0[8];                         // 0x0300
	uint32 PLAYER_VISIBLE_ITEM_11_PROPERTIES;                   // 0x0320
	uint32 PLAYER_VISIBLE_ITEM_11_PAD;                          // 0x0324
	uint64 PLAYER_VISIBLE_ITEM_12_CREATOR;                      // 0x0328
	uint32 PLAYER_VISIBLE_ITEM_12_0[8];                         // 0x0330
	uint32 PLAYER_VISIBLE_ITEM_12_PROPERTIES;                   // 0x0350
	uint32 PLAYER_VISIBLE_ITEM_12_PAD;                          // 0x0354
	uint64 PLAYER_VISIBLE_ITEM_13_CREATOR;                      // 0x0358
	uint32 PLAYER_VISIBLE_ITEM_13_0[8];                         // 0x0360
	uint32 PLAYER_VISIBLE_ITEM_13_PROPERTIES;                   // 0x0380
	uint32 PLAYER_VISIBLE_ITEM_13_PAD;                          // 0x0384
	uint64 PLAYER_VISIBLE_ITEM_14_CREATOR;                      // 0x0388
	uint32 PLAYER_VISIBLE_ITEM_14_0[8];                         // 0x0390
	uint32 PLAYER_VISIBLE_ITEM_14_PROPERTIES;                   // 0x03B0
	uint32 PLAYER_VISIBLE_ITEM_14_PAD;                          // 0x03B4
	uint64 PLAYER_VISIBLE_ITEM_15_CREATOR;                      // 0x03B8
	uint32 PLAYER_VISIBLE_ITEM_15_0[8];                         // 0x03C0
	uint32 PLAYER_VISIBLE_ITEM_15_PROPERTIES;                   // 0x03E0
	uint32 PLAYER_VISIBLE_ITEM_15_PAD;                          // 0x03E4
	uint64 PLAYER_VISIBLE_ITEM_16_CREATOR;                      // 0x03E8
	uint32 PLAYER_VISIBLE_ITEM_16_0[8];                         // 0x03F0
	uint32 PLAYER_VISIBLE_ITEM_16_PROPERTIES;                   // 0x0410
	uint32 PLAYER_VISIBLE_ITEM_16_PAD;                          // 0x0414
	uint64 PLAYER_VISIBLE_ITEM_17_CREATOR;                      // 0x0418
	uint32 PLAYER_VISIBLE_ITEM_17_0[8];                         // 0x0420
	uint32 PLAYER_VISIBLE_ITEM_17_PROPERTIES;                   // 0x0440
	uint32 PLAYER_VISIBLE_ITEM_17_PAD;                          // 0x0444
	uint64 PLAYER_VISIBLE_ITEM_18_CREATOR;                      // 0x0448
	uint32 PLAYER_VISIBLE_ITEM_18_0[8];                         // 0x0450
	uint32 PLAYER_VISIBLE_ITEM_18_PROPERTIES;                   // 0x0470
	uint32 PLAYER_VISIBLE_ITEM_18_PAD;                          // 0x0474
	uint64 PLAYER_VISIBLE_ITEM_19_CREATOR;                      // 0x0478
	uint32 PLAYER_VISIBLE_ITEM_19_0[8];                         // 0x0480
	uint32 PLAYER_VISIBLE_ITEM_19_PROPERTIES;                   // 0x04A0
	uint32 PLAYER_VISIBLE_ITEM_19_PAD;                          // 0x04A4
	uint32 PLAYER_FIELD_INV_SLOT_HEAD[46];                      // 0x04A8
	uint32 PLAYER_FIELD_PACK_SLOT_1[32];                        // 0x0560
	uint32 PLAYER_FIELD_BANK_SLOT_1[48];                        // 0x05E0
	uint32 PLAYER_FIELD_BANKBAG_SLOT_1[12];                     // 0x06A0
	uint32 PLAYER_FIELD_VENDORBUYBACK_SLOT_1[24];               // 0x06D0
	uint32 PLAYER_FIELD_KEYRING_SLOT_1[64];                     // 0x0730
	uint64 PLAYER_FARSIGHT;                                     // 0x0830
	uint64 PLAYER__FIELD_COMBO_TARGET;                          // 0x0838
	uint32 PLAYER_XP;                                           // 0x0840
	uint32 PLAYER_NEXT_LEVEL_XP;                                // 0x0844
	uint32 PLAYER_SKILL_INFO_1_1[384];                          // 0x0848
	uint32 PLAYER_CHARACTER_POINTS1;                            // 0x0E48
	uint32 PLAYER_CHARACTER_POINTS2;                            // 0x0E4C
	uint32 PLAYER_TRACK_CREATURES;                              // 0x0E50
	uint32 PLAYER_TRACK_RESOURCES;                              // 0x0E54
	uint32 PLAYER_BLOCK_PERCENTAGE;                             // 0x0E58
	uint32 PLAYER_DODGE_PERCENTAGE;                             // 0x0E5C
	uint32 PLAYER_PARRY_PERCENTAGE;                             // 0x0E60
	uint32 PLAYER_CRIT_PERCENTAGE;                              // 0x0E64
	uint32 PLAYER_RANGED_CRIT_PERCENTAGE;                       // 0x0E68
	uint32 PLAYER_EXPLORED_ZONES_1[64];                         // 0x0E6C
	uint32 PLAYER_REST_STATE_EXPERIENCE;                        // 0x0F6C
	uint32 PLAYER_FIELD_COINAGE;                                // 0x0F70
	uint32 PLAYER_FIELD_POSSTAT0;                               // 0x0F74
	uint32 PLAYER_FIELD_POSSTAT1;                               // 0x0F78
	uint32 PLAYER_FIELD_POSSTAT2;                               // 0x0F7C
	uint32 PLAYER_FIELD_POSSTAT3;                               // 0x0F80
	uint32 PLAYER_FIELD_POSSTAT4;                               // 0x0F84
	uint32 PLAYER_FIELD_NEGSTAT0;                               // 0x0F88
	uint32 PLAYER_FIELD_NEGSTAT1;                               // 0x0F8C
	uint32 PLAYER_FIELD_NEGSTAT2;                               // 0x0F90
	uint32 PLAYER_FIELD_NEGSTAT3;                               // 0x0F94
	uint32 PLAYER_FIELD_NEGSTAT4;                               // 0x0F98
	uint32 PLAYER_FIELD_RESISTANCEBUFFMODSPOSITIVE[7];          // 0x0F9C
	uint32 PLAYER_FIELD_RESISTANCEBUFFMODSNEGATIVE[7];          // 0x0FB8
	uint32 PLAYER_FIELD_MOD_DAMAGE_DONE_POS[7];                 // 0x0FD4
	uint32 PLAYER_FIELD_MOD_DAMAGE_DONE_NEG[7];                 // 0x0FF0
	uint32 PLAYER_FIELD_MOD_DAMAGE_DONE_PCT[7];                 // 0x100C
	uint32 PLAYER_FIELD_BYTES;                                  // 0x1028
	uint32 PLAYER_AMMO_ID;                                      // 0x102C
	uint32 PLAYER_SELF_RES_SPELL;                               // 0x1030
	uint32 PLAYER_FIELD_PVP_MEDALS;                             // 0x1034
	uint32 PLAYER_FIELD_BUYBACK_PRICE_1[12];                    // 0x1038
	uint32 PLAYER_FIELD_BUYBACK_TIMESTAMP_1[12];                // 0x1068
	uint32 PLAYER_FIELD_SESSION_KILLS;                          // 0x1098
	uint32 PLAYER_FIELD_YESTERDAY_KILLS;                        // 0x109C
	uint32 PLAYER_FIELD_LAST_WEEK_KILLS;                        // 0x10A0
	uint32 PLAYER_FIELD_THIS_WEEK_KILLS;                        // 0x10A4
	uint32 PLAYER_FIELD_THIS_WEEK_CONTRIBUTION;                 // 0x10A8
	uint32 PLAYER_FIELD_LIFETIME_HONORBALE_KILLS;               // 0x10AC
	uint32 PLAYER_FIELD_LIFETIME_DISHONORBALE_KILLS;            // 0x10B0
	uint32 PLAYER_FIELD_YESTERDAY_CONTRIBUTION;                 // 0x10B4
	uint32 PLAYER_FIELD_LAST_WEEK_CONTRIBUTION;                 // 0x10B8
	uint32 PLAYER_FIELD_LAST_WEEK_RANK;                         // 0x10BC
	uint32 PLAYER_FIELD_BYTES2;                                 // 0x10C0
	uint32 PLAYER_FIELD_WATCHED_FACTION_INDEX;                  // 0x10C4
	uint32 PLAYER_FIELD_COMBAT_RATING_1[20];                    // 0x10C8
};

struct GameObjectFields
{
	uint64 OBJECT_FIELD_CREATED_BY;                             // 0x0000
	uint32 GAMEOBJECT_DISPLAYID;                                // 0x0008
	uint32 GAMEOBJECT_FLAGS;                                    // 0x000C
	uint32 GAMEOBJECT_ROTATION[4];                              // 0x0010
	uint32 GAMEOBJECT_STATE;                                    // 0x0020
	uint32 GAMEOBJECT_POS_X;                                    // 0x0024
	uint32 GAMEOBJECT_POS_Y;                                    // 0x0028
	uint32 GAMEOBJECT_POS_Z;                                    // 0x002C
	uint32 GAMEOBJECT_FACING;                                   // 0x0030
	uint32 GAMEOBJECT_DYN_FLAGS;                                // 0x0034
	uint32 GAMEOBJECT_FACTION;                                  // 0x0038
	uint32 GAMEOBJECT_TYPE_ID;                                  // 0x003C
	uint32 GAMEOBJECT_LEVEL;                                    // 0x0040
	uint32 GAMEOBJECT_ARTKIT;                                   // 0x0044
	uint32 GAMEOBJECT_ANIMPROGRESS;                             // 0x0048
	uint32 GAMEOBJECT_PADDING;                                  // 0x004C
};

struct DynamicObjectFields
{
	uint64 DYNAMICOBJECT_CASTER;                                // 0x0000
	uint32 DYNAMICOBJECT_BYTES;                                 // 0x0008
	uint32 DYNAMICOBJECT_SPELLID;                               // 0x000C
	uint32 DYNAMICOBJECT_RADIUS;                                // 0x0010
	uint32 DYNAMICOBJECT_POS_X;                                 // 0x0014
	uint32 DYNAMICOBJECT_POS_Y;                                 // 0x0018
	uint32 DYNAMICOBJECT_POS_Z;                                 // 0x001C
	uint32 DYNAMICOBJECT_FACING;                                // 0x0020
	uint32 DYNAMICOBJECT_PAD;                                   // 0x0024
};

struct CorpseFields
{
	uint64 CORPSE_FIELD_OWNER;                                  // 0x0000
	uint32 CORPSE_FIELD_FACING;                                 // 0x0008
	uint32 CORPSE_FIELD_POS_X;                                  // 0x000C
	uint32 CORPSE_FIELD_POS_Y;                                  // 0x0010
	uint32 CORPSE_FIELD_POS_Z;                                  // 0x0014
	uint32 CORPSE_FIELD_DISPLAY_ID;                             // 0x0018
	uint32 CORPSE_FIELD_ITEM[19];                               // 0x001C
	uint32 CORPSE_FIELD_BYTES_1;                                // 0x0068
	uint32 CORPSE_FIELD_BYTES_2;                                // 0x006C
	uint32 CORPSE_FIELD_GUILD;                                  // 0x0070
	uint32 CORPSE_FIELD_FLAGS;                                  // 0x0074
	uint32 CORPSE_FIELD_DYNAMIC_FLAGS;                          // 0x0078
	uint32 CORPSE_FIELD_PAD;                                    // 0x007C
};
*/
