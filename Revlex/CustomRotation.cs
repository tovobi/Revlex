using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace Revlex
{
	public class CustomRotation : PlayerRotation
	{
		WowObject lastTarget;
		int LastWeaponStyleWas2H = 0;
		bool LastFailedAutotargetAndCastAttempt;
		bool DidWeaponSwitch;
		bool DidSuccesfullAutotargetCast;
		bool ReInitWeaponSwitch = false;
		bool ReInitCast = false;
		long LastTimeRapidChanged = 0;
		double MeHealthPercent;
		uint RageBefore = 0;
		float CdOffset = 0.1f; // we had 1.6 before, cuz we thought a general GC would count into CD of a skill
		WowObject Me;
		WowObject Target;
		List<WowObject> Targets3Within15;
		List<WowObject> Targets3Within8;
		List<WowObject> Targets5Within20;
		List<WowObject> TargetsCastingWithin5;
		List<WowObject> Targets10Within40;
		List<WowObject> Targets12Within5;
		uint RapidSequence = 0;
        
		public CustomRotation() : base()
		{
		}

		public bool TargetHealthFactor(float unitHealth, float factor = 1.3f)
		{
			return ((float)((float)WowHelperObj.LocalPlayer.Target.Health / (float)WowHelperObj.LocalPlayer.Target.MaxHealth * 100f) > unitHealth && (WowHelperObj.LocalPlayer.Target.Level >= WowHelperObj.LocalPlayer.Level - 6 || WowHelperObj.LocalPlayer.Target.Health > WowHelperObj.LocalPlayer.MaxHealth * factor));
		}

		public bool TargetHealthFactor(float unitHealth, uint minTargetCount, uint currentTargetCount, float factor = 1.3f)
		{
			return ((float)((float)WowHelperObj.LocalPlayer.Target.Health / (float)WowHelperObj.LocalPlayer.Target.MaxHealth * 100f) > unitHealth && currentTargetCount >= minTargetCount && (WowHelperObj.LocalPlayer.Target.Level >= WowHelperObj.LocalPlayer.Level - 6 || WowHelperObj.LocalPlayer.Target.Health > WowHelperObj.LocalPlayer.MaxHealth * factor));
		}

		public void GeneralActionsDmg()
		{						
			Me = WowHelperObj.LocalPlayer;
			MeHealthPercent = Math.Floor((double)Me.Health / (double)Me.MaxHealth * 100);
			Target = WowHelperObj.LocalPlayer.Target;
			Targets3Within15 = WowHelperObj.GetNearEnemies(15, 3);
			Targets3Within8 = WowHelperObj.GetNearEnemies(8, 3);
			Targets5Within20 = WowHelperObj.GetNearEnemies(20, 5);
			//Log.Print("Combat: " + Me.IsInCombat.ToString() + "   TarDynFLags: " + Target.DynamicFlags.ToString("X") +"   rage: " + Me.Rage.ToString() + "   tar: " + WowHelperObj.HasTarget(Me) + "   dodged: " + Target.Dodged + "  facing: " + Target.PlayerIsFacingTo + "   dist: " + Target.Distance);
			//Log.Print("tar fleeing: " + Target.IsFleeing + "   Faction: " + Target.FactionTemplate.ToString() + "   Keys: " + KeysBlocked() + "   near15/8/5: " + WowHelperObj.GetNearEnemies(15, 10).Count() + "/" + WowHelperObj.GetNearEnemies(8, 10).Count() + "/" + WowHelperObj.GetNearEnemies(5, 10).Count() + "   Spell Me/Tar: " + Math.Max(Me.ChannelSpell, Me.CastSpell).ToString() + " / " + Math.Max(Target.ChannelSpell, Target.CastSpell).ToString());

		}
		public void GeneralActionsInterrupt()
		{			
			Me = WowHelperObj.LocalPlayer;
			MeHealthPercent = Math.Floor((double)Me.Health / (double)Me.MaxHealth * 100);
			Target = WowHelperObj.LocalPlayer.Target;
			TargetsCastingWithin5 = WowHelperObj.GetCastingEnemies(5, 5);
			//Log.Print("Combat: " + Me.IsInCombat.ToString() + "   TarDynFLags: " + Target.DynamicFlags.ToString("X") +"   rage: " + Me.Rage.ToString() + "   tar: " + WowHelperObj.HasTarget(Me) + "   dodged: " + Target.Dodged + "  facing: " + Target.PlayerIsFacingTo + "   dist: " + Target.Distance);
			//Log.Print("tar fleeing: " + Target.IsFleeing + "   Faction: " + Target.FactionTemplate.ToString() + "   Keys: " + KeysBlocked() + "   near15/8/5: " + WowHelperObj.GetNearEnemies(15, 10).Count() + "/" + WowHelperObj.GetNearEnemies(8, 10).Count() + "/" + WowHelperObj.GetNearEnemies(5, 10).Count() + "   Spell Me/Tar: " + Math.Max(Me.ChannelSpell, Me.CastSpell).ToString() + " / " + Math.Max(Target.ChannelSpell, Target.CastSpell).ToString());

		}
		public void GeneralActionsTank()
		{			
			Me = WowHelperObj.LocalPlayer;
			MeHealthPercent = Math.Floor((double)Me.Health / (double)Me.MaxHealth * 100);
			Target = WowHelperObj.LocalPlayer.Target;
			Targets3Within15 = WowHelperObj.GetNearEnemies(15, 3);
			Targets3Within8 = WowHelperObj.GetNearEnemies(8, 3);
			Targets5Within20 = WowHelperObj.GetNearEnemies(20, 5);
			//Targets10Within40 = WowHelperObj.GetNoAggroEnemy(20, 5);
			Targets12Within5 = WowHelperObj.GetNearEnemies(5, 12);
			//Log.Print("Combat: " + Me.IsInCombat.ToString() + "   TarDynFLags: " + Target.DynamicFlags.ToString("X") +"   rage: " + Me.Rage.ToString() + "   tar: " + WowHelperObj.HasTarget(Me) + "   dodged: " + Target.Dodged + "  facing: " + Target.PlayerIsFacingTo + "   dist: " + Target.Distance);
			//Log.Print("tar fleeing: " + Target.IsFleeing + "   Faction: " + Target.FactionTemplate.ToString() + "   Keys: " + KeysBlocked() + "   near15/8/5: " + WowHelperObj.GetNearEnemies(15, 10).Count() + "/" + WowHelperObj.GetNearEnemies(8, 10).Count() + "/" + WowHelperObj.GetNearEnemies(5, 10).Count() + "   Spell Me/Tar: " + Math.Max(Me.ChannelSpell, Me.CastSpell).ToString() + " / " + Math.Max(Target.ChannelSpell, Target.CastSpell).ToString());

		}



		public override void Combat1()
		{
			float tempOvrpwrCd = WowHelperObj.GetSpellCooldown("Overpower");
			GeneralActionsDmg();
			// start attack
			StartMeleeAttack();

			//Log.Print("##################################### Target: " + Target.ObjBaseAddress.ToString("X") + "   " + Target.Name + "   class: " + Target.Class);

			// Battle Shout
			if (!WowHelperObj.UnitHasBuff(Me, WowHelperObj.GetSpellIdFromSpellName("Battle Shout")) && Me.Rage >= 10)
			{
				CastSpell("Battle Shout");
				Log.Print("Cast: Battle Shout");
			}
			// Battle Shout wenn nicht im Combat und letzter Battleshout ist länger als 90s her.
			//Log.Print("LASTCAST: " + (WowHelperObj.SpellsInSpellBook.Where(C => C.Name == "Battle Shout").FirstOrDefault().LastCast < WowHelpers.GetTime() - 20000).ToString());
			if (Me.Rage >= 10 && !Me.IsInCombat && WowHelperObj.SpellsInSpellBook.Where(C => C.Name == "Battle Shout").FirstOrDefault().LastCast < WowHelpers.GetTime() - 90000)
			{
				CastSpell("Battle Shout");
				Log.Print("Cast: Battle Shout 2");
			}
			if (WowHelperObj.HasTarget(Me))
			{
				//Execute
				if (Me.Rage >= 10 && (float)((float)Target.Health / (float)Target.MaxHealth * 100f) < 20f)
				{
					CastSpell("Execute");
					Log.Print("Cast: Execute " + (float)((float)Target.Health / (float)Target.MaxHealth * 100f));
				}
				//Hamstring if NPC is fleeing
				if (Me.Rage >= 10 && Target.IsFleeing)
				{
					CastSpell("Hamstring");
					Log.Print("Cast: Hamstring");
				}
				// Blood rage when no rage for overpower 
				if (WowHelperObj.GetSpellCooldown("Bloodrage") <= 0 && Target.Dodged >= 1 && tempOvrpwrCd < CdOffset && Me.Rage < 5 && TargetHealthFactor(80))
				{
					CastSpell("Bloodrage", false, true);
					Log.Print("Cast: Blood Rage (overpower)");
				}
				//Overpower
				if (Target.Dodged >= 1 && tempOvrpwrCd < CdOffset && Me.Rage >= 5)
				{
					CastSpell("Overpower");
					Log.Print("Cast: Overpower");
				}
				// Blood rage when no battleshout is active
				if (WowHelperObj.GetSpellCooldown("Bloodrage") <= 0 && !WowHelperObj.UnitHasBuff(Me, "Battle Shout") && Me.Rage <= 5 && TargetHealthFactor(80))
				{
					CastSpell("Bloodrage", false, true);
					Log.Print("Cast: Bloodrage (battleshout)");
				}
				//Bloodrage when last cast of battleshout is max 60sec ago
				if (WowHelperObj.GetSpellCooldown("Bloodrage") < 0.3 && WowHelperObj.UnitHasBuff(Me, "Battle Shout") && Me.Rage <= 10 && WowHelperObj.SpellsInSpellBook.Where(C => C.Name == "Battle Shout").FirstOrDefault().LastCast > WowHelpers.GetTime() - 60000 && TargetHealthFactor(80))
				{
					CastSpell("Bloodrage");
					Log.Print("Cast: Bloodrage (more Damage)");
				}
				//Bloodrage with more then 1 Target 
				if (WowHelperObj.GetSpellCooldown("Bloodrage") < 0.3 && Me.Rage <= 15 && TargetHealthFactor(80))
				{
					CastSpell("Bloodrage");
					Log.Print("Cast: Bloodrage Multitarget");
				}

				//Rend only when target health is more then the half of my maxhealth
				if (!WowHelperObj.UnitHasDebuff(Target, "Rend") && Me.Rage >= 10 && TargetHealthFactor(60, 1.5f))
				{
					CastSpell("Rend");
					Log.Print("!!!!!!!!Cast: Rend");
				}
				//Rend with more then 1 Target 
				if (!WowHelperObj.UnitHasDebuff(Target, "Rend") && Me.Rage >= 10 && TargetHealthFactor(25f, 2, (uint)Targets3Within15.Count(), 1.5f))
				{
					CastSpell("Rend");
					Log.Print("Cast: Rend Multitarget");
				}
				//Demo Shout
				if (!WowHelperObj.UnitHasDebuff(Target, "Demoralizing Shout") && Me.Rage >= 10 && TargetHealthFactor(40f, 2, (uint)Targets3Within15.Count(), 1.5f))
				{
					CastSpell("Demoralizing Shout");
					Log.Print("Cast: Demo");
				}
				//Cleave
				if (Me.Rage >= 20 && (uint)Targets3Within8.Count() >= 2 && !WowHelperObj.HostileNpcInCc())
				{
					CastSpell("Cleave");
					Log.Print("Cast: Cleave");
				}
				else if (Me.Rage >= 20 && (uint)Targets3Within8.Count() >= 2 && WowHelperObj.HostileNpcInCc())
				{
					Log.Print("No Cleave -> CC'ed mob in range");
				}
				//MS
				if (WowHelperObj.GetSpellCooldown("Mortal Strike") < 0.3 && Me.Rage >= 30)
				{
					CastSpell("Mortal Strike");
					Log.Print("Cast: MS (" + WowHelperObj.GetSpellCooldown(12294).ToString() + ")");
				}
				// HS for lowlvl when no MS is available
				if (Me.Rage >= 15 && !WowHelperObj.SpellExist((int)WowHelperObj.GetSpellIdFromSpellName("Mortal Strike")))
				{
					CastSpell("Heroic Strike");
					//CastSpellById(11567);
					Log.Print("Cast: HS (lowlvl)");
				}
				// HS when to much rage / only when MS is available
				if ((Me.Rage >= 20 && WowHelperObj.GetSpellCooldown("Mortal Strike") > 5) || (Me.Rage >= 40 && WowHelperObj.GetSpellCooldown("Mortal Strike") > 3))
				{
					CastSpell("Heroic Strike");
					Log.Print("Cast: HS (Ragedump)");
				}
				//Blood Fury only when target health is more then the half of my maxhealth
				if (WowHelperObj.GetSpellCooldown("Blood Fury") < 0.3 && TargetHealthFactor(80))
				{
					CastSpell("Blood Fury");
					Log.Print("Cast: Blood Fury");
				}
				//Blood Fury with more then 1 Target 
				if (WowHelperObj.GetSpellCooldown("Blood Fury") < 0.3 && TargetHealthFactor(50f, 2, (uint)Targets3Within15.Count(), 1.3f))
				{
					CastSpell("Blood Fury");
					Log.Print("Cast: Blood Fury Multitarget");
				}
			}
		}


		public override void Combat2()
		{
			GeneralActionsInterrupt();
			//(uint)WowHelperObj.GetNearEnemies(15, 200).Count();
			// start attack
			StartMeleeAttack();
			//Log.Print("Casting Enemies: " + TargetsCastingWithin5.Count());


			//Reset Auto target and cast vars after 5s if smt went wrong
			if (LastTimeRapidChanged < WowHelpers.GetTime() - 5000)
			{
				LastTimeRapidChanged = WowHelpers.GetTime();
				RapidSequence = 0;
				LastWeaponStyleWas2H = 0;
				LastFailedAutotargetAndCastAttempt = false;
				DidWeaponSwitch = false;
				DidSuccesfullAutotargetCast = false;
				ReInitWeaponSwitch = false;
				ReInitCast = false;
				// if Weapon in Mainhand is missing
				if (WowHelperObj.GetMainhandItemId() == 0)
				{
					CastMacroById(23, true, true); //equip 1h
					CastMacroById(24, true, true); //shield
				}

			}



			// Shield Bash with autoswitch weapons and auto switch targets when in Battle or Def Stance
			if (TargetsCastingWithin5.Count() > 0 && (WowHelperObj.UnitHasBuff(Me,2457) || WowHelperObj.UnitHasBuff(Me,71)))
			{

				float tempCd = Math.Max(WowHelperObj.GetSpellCooldown("Pummel"), WowHelperObj.GetSpellCooldown("Shield Bash"));
				Log.Print("SB------- Rapid " + RapidSequence + "   lastWeapon: " + LastWeaponStyleWas2H + "   [" + tempCd + "s]");
				if (!LastFailedAutotargetAndCastAttempt)
				{
					lastTarget = Target;
				}
				// if shortly the weapon was switched, or in this sequence the weaponstyle wasnt checked
				if (tempCd < CdOffset && !DidWeaponSwitch && LastWeaponStyleWas2H == 0)
				{
					if (WowHelperObj.GetOffhandItemId() == 0)
					{
						LastWeaponStyleWas2H = 2;
						Log.Print("Last Weapon was 2h");
					}
					if (WowHelperObj.GetOffhandItemId() > 0)
					{
						LastWeaponStyleWas2H = 1;
						Log.Print("Last Weapon was 1h+Off");
					}
				}
				// Rapid Sequence 100: From 2h -> 1h+Shield -> Shield Bash -> 2h
				// ---
				if (RapidSequence == 0 && tempCd < CdOffset && Me.Rage >= 10 && WowHelperObj.GetOffhandItemId() == 0 && LastWeaponStyleWas2H == 2)
				{
					RapidSequence = 100;
					CastMacroById(25, true, true); //unequip 2h
					CastMacroById(24, true, true); //shield
					DidWeaponSwitch = true;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence == 0 && tempCd < CdOffset && Me.Rage >= 10 && WowHelperObj.GetOffhandItemId() > 0 && LastWeaponStyleWas2H == 1)
				{
					RapidSequence = 100;
					DidWeaponSwitch = true;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}

				if (RapidSequence == 100 && ((tempCd < CdOffset && Me.Rage >= 10) || ReInitCast) && GotRelevantTargetFromList(TargetsCastingWithin5) && WowHelperObj.GetOffhandItemId() > 0)
				{
					RapidSequence = 105;
					CastSpell("Shield Bash", true, true);
					//CastMacroById(23, true, true); //equip 1h // not neccessary
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence == 100)
				{
					LastFailedAutotargetAndCastAttempt = true;
				}
				if (RapidSequence == 105 && tempCd < CdOffset)
				{
					Log.Print("Re-init Cast ["+ tempCd +"s]");
					RapidSequence = 100;
					ReInitCast = true;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if(RapidSequence == 105)
				{
					ReInitCast = false;
					RapidSequence = 107;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				if (RapidSequence == 107 && tempCd > CdOffset && SwitchTarget(lastTarget))
				{
					RapidSequence = 110;
					LastFailedAutotargetAndCastAttempt = false;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				if (RapidSequence >= 110 && RapidSequence <= 120 && ((WowHelperObj.GetOffhandItemId() > 0 || ReInitWeaponSwitch) && LastWeaponStyleWas2H == 2))
				{
					RapidSequence = 120;
					CastMacroById(22, true, true); //back to 2h
					Log.Print("Back to 2h");
					//LastWeaponStyleWas2H = 0; // back to standard setting
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence >= 110 && RapidSequence <= 120 && (WowHelperObj.GetOffhandItemId() > 0 && LastWeaponStyleWas2H == 1))
				{
					RapidSequence = 120;
					CastMacroById(23, true, true); //equip 1h
					CastMacroById(24, true, true); //shield
					Log.Print("Back to 1h + off");
					LastTimeRapidChanged = WowHelpers.GetTime();

					//LastWeaponStyleWas2H = 0; // back to standard setting
				}
				// checks if Weapon was changed back to lastweapon
				if (RapidSequence == 120 && WowHelperObj.GetOffhandItemId() > 0 && LastWeaponStyleWas2H == 2)
				{
					RapidSequence = 110;
					ReInitWeaponSwitch = true;
					Log.Print("Re-init Weaponswitch to 2h  ["+ WowHelperObj.GetOffhandItemId()+"]");
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence == 120)
				{
					LastWeaponStyleWas2H = 0;
					RapidSequence = 130;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				if (RapidSequence == 130)
				{
					ReInitWeaponSwitch = false;
					DidWeaponSwitch = false;
					RapidSequence = 0;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
			}

			// pummel with autoswitch weapons and auto switch targets when inBerserker stance
			if (TargetsCastingWithin5.Count() > 0 && WowHelperObj.UnitHasBuff(Me, 2458))
			{

				float tempCd = Math.Max(WowHelperObj.GetSpellCooldown("Pummel"), WowHelperObj.GetSpellCooldown("Shield Bash"));
				Log.Print("P------- Rapid " + RapidSequence + "   [" + tempCd + "s]");
				if (!LastFailedAutotargetAndCastAttempt)
				{
					lastTarget = Target;
				}
				if (RapidSequence == 0 && ((tempCd < CdOffset && Me.Rage >= 10) || ReInitCast) && GotRelevantTargetFromList(TargetsCastingWithin5))
				{
					RapidSequence = 205;
					CastSpell("Pummel", true, true);
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence == 0)
				{
					LastFailedAutotargetAndCastAttempt = true;
				}
				if (RapidSequence == 205 && tempCd < CdOffset)
				{
					Log.Print("Re-init Cast [" + tempCd + "s]");
					RapidSequence = 0;
					ReInitCast = true;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (RapidSequence == 205)
				{
					ReInitCast = false;
					RapidSequence = 207;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				if (RapidSequence == 207 && tempCd > CdOffset && SwitchTarget(lastTarget))
				{
					RapidSequence = 230;
					LastFailedAutotargetAndCastAttempt = false;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}

				if (RapidSequence == 230)
				{
					RapidSequence = 0;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
			}


		}

		public override void Combat3()
		{

			GeneralActionsTank();
			StartMeleeAttack();
			//Log.Print("- " + Targets12Within5.Count());

			float tempRevengeCd = WowHelperObj.GetSpellCooldown("Revenge");
			float tempShieldBlockCd = WowHelperObj.GetSpellCooldown("Shield Block");
			float tempOvrpwrCd = WowHelperObj.GetSpellCooldown("Overpower");
			float tempThuClaCd = WowHelperObj.GetSpellCooldown("Thunder Clap");
			float tempTauntCd = WowHelperObj.GetSpellCooldown("Taunt");
			float tempMoBlowCd = WowHelperObj.GetSpellCooldown("Mocking Blow");
			bool tempHasZerkSt = WowHelperObj.UnitHasBuff(Me, 2458);
			bool tempHasDefSt = WowHelperObj.UnitHasBuff(Me, 71);
			bool tempHasBattleSt = WowHelperObj.UnitHasBuff(Me, 2457);
			//bool overpowerReadyTank = Target.Dodged >= 1  && tempOvrpwrCd < CdOffset && Me.Health >= 66 &&((Me.Rage >= 5 && Me.Rage <= 15 && !tempHasBattleSt) || (Me.Rage >= 5 && tempHasBattleSt));
			bool overpowerReadyTank = Target.Dodged >= 1 && tempOvrpwrCd < CdOffset && Me.Rage >= 5 && tempHasBattleSt;
			bool thunderClapReadyTank = tempThuClaCd < CdOffset && Me.Health >= 66 && ((Me.Rage >= 20 && Me.Rage <= 30 && !tempHasBattleSt) || (Me.Rage >= 20 && tempHasBattleSt)) && ((double)Targets12Within5.Count(c => c.DebuffList.Exists(o => o.Name == "Thunder Clap")) / (double)Targets12Within5.Count * 100 <= 50);
			bool moBlowReady = tempMoBlowCd < CdOffset && tempTauntCd > CdOffset && tempTauntCd < 8 && WowHelperObj.AggroOnWeak().Guid != 0 && Me.Rage >= 10;

			RageBefore = 0;

			WowObject tempPreferedTar = WowHelperObj.GetBestTankTarget(5,12,"Sunder Armor");
			string[] tempDebuffString = new string[] { "Rend", "Sunder Armor" };
			//WowObject tempPreferedTar2 = WowHelperObj.GetBestTankTarget2(tempDebuffString, 5, 12);


			if (WowHelperObj.AggroOnWeak().Guid == 0 && tempShieldBlockCd < CdOffset && Me.Rage >= 10 && tempHasDefSt && MeHealthPercent < 38 && WowHelperObj.GetOffhandItemId() > 0)
			{
				CastSpell("Shield Block", true, true);
				Log.Print("Cast: Shield Block");
			}


			//Taunt
			if (WowHelperObj.AggroOnWeak().Guid != 0 && !tempHasDefSt && tempTauntCd < CdOffset && SwitchTarget(WowHelperObj.AggroOnWeak()))
			{
				CastSpell("Defensive Stance");
				Log.Print("Cast: Defensive Stance (for Taunt)");
			}
			if (WowHelperObj.AggroOnWeak().Guid != 0 && tempHasDefSt && tempTauntCd < CdOffset && SwitchTarget(WowHelperObj.AggroOnWeak()))
			{
				CastSpell("Taunt");
				Log.Print("Cast: Taunt");
			}

			//Mocking Blow
			if ((RapidSequence < 700 || RapidSequence > 799) && moBlowReady && !tempHasBattleSt && SwitchTarget(WowHelperObj.AggroOnWeak()))
			{
				RapidSequence = 700;
				CastSpell("Battle Stance");
				Log.Print("Cast: Battle Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if ((RapidSequence < 700 || RapidSequence > 799) && moBlowReady && tempHasBattleSt && SwitchTarget(WowHelperObj.AggroOnWeak()))
			{
				RapidSequence = 700;
				Log.Print("already in Battle Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			if (RapidSequence == 700 && !tempHasBattleSt && moBlowReady)
			{
				RapidSequence = 0;
				Log.Print("Re-init Cast Battle St");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 700 && moBlowReady && tempHasBattleSt)
			{
				RageBefore = Me.Rage;
				RapidSequence = 705;
				CastSpell("Mocking Blow");
				Log.Print("Cast: Mocking Blow");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 700 && !moBlowReady && tempHasBattleSt)
			{
				RapidSequence = 710;
				CastSpell("Defensive Stance");
				Log.Print("Cast: workaround -> Defensive Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			Log.Print("  mid moCD: " + moBlowReady + "   me.rage:" + Me.Rage + "   ragebefore: " + RageBefore);
			if (RapidSequence == 705 && !moBlowReady)
			{
				RapidSequence = 710;
				CastSpell("Defensive Stance");
				Log.Print("Cast: Defensive Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 705 && moBlowReady)
			{
				RapidSequence = 700;
				Log.Print("Re-init Mocking Blow");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			if (RapidSequence == 710 && !tempHasDefSt)
			{
				RapidSequence = 705;
				Log.Print("Re-init Cast Def St");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 710 && tempHasDefSt)
			{
				RapidSequence = 0;
				LastTimeRapidChanged = WowHelpers.GetTime();
			}



			//Thunder Clap with Stance Dance
			// ---
			if ((RapidSequence < 600 || RapidSequence > 699) && thunderClapReadyTank && !tempHasBattleSt)
			{
				RapidSequence = 600;
				CastSpell("Battle Stance");
				Log.Print("Cast: Battle Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if ((RapidSequence < 600 || RapidSequence > 699) && thunderClapReadyTank && tempHasBattleSt)
			{
				RapidSequence = 600;
				Log.Print("already in Battle Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			//else
			//{
			//	Log.Print("Dont cast Thunder Clap:  Tar in % with TC:" + ((double)Targets12Within5.Count(c => c.DebuffList.Exists(o => o.Name == "Thunder Clap")) / (double)Targets12Within5.Count * 100) + "   thuclaCD: " + tempThuClaCd + "    rage: " + Me.Rage);
			//}
			if (RapidSequence == 600 && !tempHasBattleSt && thunderClapReadyTank && Me.Rage >= 20)
			{
				RapidSequence = 0;
				Log.Print("Re-init Cast Battle St");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 600 && thunderClapReadyTank && tempHasBattleSt && Me.Rage >= 20)
			{
				RageBefore = Me.Rage;
				RapidSequence = 605;
				CastSpell("Thunder Clap");
				Log.Print("Cast: Thunder Clap");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 600 && !thunderClapReadyTank && tempHasBattleSt)
			{
				RapidSequence = 610;
				CastSpell("Defensive Stance");
				Log.Print("Cast: workaround -> Defensive Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			//Log.Print("  mid tcCD: " + tempThuClaCd + "   me.rage:" + Me.Rage + "   ragebefore: " + RageBefore);
			if (RapidSequence == 605 && !thunderClapReadyTank)
			{
				RapidSequence = 610;
				CastSpell("Defensive Stance");
				Log.Print("Cast: Defensive Stance");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 605 && thunderClapReadyTank)
			{
				RapidSequence = 600;
				Log.Print("Re-init Thunder Clap");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			if (RapidSequence == 610 && !tempHasDefSt)
			{
				RapidSequence = 605;
				Log.Print("Re-init Cast Def St");
				LastTimeRapidChanged = WowHelpers.GetTime();
			}
			else if (RapidSequence == 610 && tempHasDefSt)
			{
				RapidSequence = 0;
				LastTimeRapidChanged = WowHelpers.GetTime();
			}



			// Overpower only if in Battlestance
			//Log.Print("--------Start Rapid: " + RapidSequence + "    overpowerReady: "+ overpowerReadyTank + "  ovpCD: "+ tempOvrpwrCd);
			#region Overpower without stancedance
			// Rapid Sequence 300: Overpower with stancedance
			// ---
			if (overpowerReadyTank)
			{
				CastSpell("Overpower");
				Log.Print("Cast: Overpower");
			}

			#endregion

			#region Rapid Sequence 300: Overpower with stancedance
			//// Rapid Sequence 300: Overpower with stancedance
			//// ---
			//if ((RapidSequence < 300 || RapidSequence > 399) && overpowerReadyTank && !tempHasBattleSt)
			//{
			//	RapidSequence = 300;
			//	CastSpell("Battle Stance");
			//	Log.Print("Cast: Battle Stance");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else if ((RapidSequence < 300 || RapidSequence > 399) && overpowerReadyTank && tempHasBattleSt)
			//{
			//	RapidSequence = 300;
			//	Log.Print("already in Battle Stance");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else
			//{
			//	Log.Print("Dont cast Overpower: dodged: " + Target.Dodged + "   ovpCD: " + tempOvrpwrCd + "    rage: " + Me.Rage);
			//}
			//if (RapidSequence == 300 && !tempHasBattleSt && overpowerReadyTank && Me.Rage >= 5)
			//{
			//	RapidSequence = 0;
			//	Log.Print("Re-init Cast Battle St");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else if (RapidSequence == 300 && overpowerReadyTank && tempHasBattleSt && Me.Rage >= 5)
			//{
			//	RageBefore = Me.Rage;
			//	RapidSequence = 305;
			//	CastSpell("Overpower");
			//	Log.Print("Cast: Overpower");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else if (RapidSequence == 300 && !overpowerReadyTank && tempHasBattleSt)
			//{
			//	RapidSequence = 310;
			//	CastSpell("Defensive Stance");
			//	Log.Print("Cast: workaround -> Defensive Stance");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//Log.Print("  mid ovpCD: " + tempOvrpwrCd + "   me.rage:" + Me.Rage + "   ragebefore: " + RageBefore);
			//if (RapidSequence == 305 && tempHasBattleSt && (tempOvrpwrCd > CdOffset || Me.Rage == RageBefore - 5))
			//{
			//	RapidSequence = 310;
			//	CastSpell("Defensive Stance");
			//	Log.Print("Cast: Defensive Stance");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else if (RapidSequence == 305 && tempHasBattleSt && (tempOvrpwrCd < CdOffset || Me.Rage == RageBefore))
			//{
			//	RapidSequence = 300;
			//	Log.Print("Re-init Cast Overpower");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//if (RapidSequence == 310 && !tempHasDefSt)
			//{
			//	RapidSequence = 305;
			//	Log.Print("Re-init Cast Def St");
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			//else if (RapidSequence == 310 && tempHasDefSt)
			//{
			//	RapidSequence = 0;
			//	LastTimeRapidChanged = WowHelpers.GetTime();
			//}
			#endregion


			//Log.Print("--------Mid1 Rapid: " + RapidSequence);

			//Log.Print("tempPreferedTar " + tempPreferedTar.Guid);

			//Autotarget for autoattack, if no ovrpwer is available
			if (tempPreferedTar != null && RapidSequence == 0 && !overpowerReadyTank && SwitchTarget(tempPreferedTar))
			{
				StartMeleeAttack();
			}

			// Demo Shout
			//Log.Print("Targets with DS: " + (double)Targets12Within5.Count(c => c.DebuffList.Exists(o => o.Name == "Demoralizing Shout")) / (double)Targets12Within5.Count * 100);
			if (!WowHelperObj.UnitHasDebuff(Target, "Demoralizing Shout") && !overpowerReadyTank && Me.Rage >= 10 && TargetHealthFactor(40f, 2, (uint)Targets3Within15.Count(), 1.5f) && ((double)Targets12Within5.Count(c => c.DebuffList.Exists(o => o.Name == "Demoralizing Shout")) / (double)Targets12Within5.Count * 100 <= 50))
			{
				CastSpell("Demoralizing Shout");
				Log.Print("Cast: Demo");
			}



			//// Autotarget for sunder/Revenge/Rend
			//if (
			//	(tempPreferedTar2 != null) &&
			//	(
			//		((Me.Rage >= 5 && tempRevengeCd < CdOffset || Me.Rage >= 10) && !overpowerReadyTank) 
			//		/* ||(RapidSequence >= 500 && RapidSequence < 600)*/
			//	))
			//{
			//	if (!tempHasDefSt)
			//	{
			//		CastSpell("Defensive Stance");
			//		Log.Print("Defensive Stance -> sunder/rend/Revenge ");
			//		LastTimeRapidChanged = WowHelpers.GetTime();
			//	}
			//	if (Me.Rage >= 5 && tempRevengeCd < CdOffset)
			//	{
			//		if (tempRevengeCd < CdOffset)
			//		{
			//			CastSpell("Revenge", true, true);
			//			Log.Print("Cast: Revenge (sunder/rend/Revenge)");
			//		}
			//	}
			//	if (SwitchTarget(tempPreferedTar2) && !WowHelperObj.UnitHasDebuff(tempPreferedTar2, "Rend") && tempPreferedTar2.tempNextSpell == "Rend" && Me.Rage >= 10)
			//	{
			//		CastSpell("Rend");
			//		Log.Print("Cast: Rend (sunder/rend/Revenge)");
			//	}
			//	else if (SwitchTarget(tempPreferedTar2) && !WowHelperObj.UnitHasDebuff(tempPreferedTar2,"Sunder Armor") && tempPreferedTar2.tempNextSpell == "Sunder Armor" && Me.Rage >= 15)
			//	{
			//		CastSpell("Sunder Armor");
			//		Log.Print("Cast: Sunder Armor (sunder/rend/Revenge)");
			//	}
			//}


			// Autotarget for sunder/Revenge
			if (
			(tempPreferedTar.Guid != 0) &&
			(tempHasDefSt) &&
			(
				((Me.Rage >= 5 && tempRevengeCd < CdOffset || Me.Rage >= 15) && !overpowerReadyTank) || 
				(RapidSequence >= 400 && RapidSequence < 500)
			))
			{
				if (!tempHasDefSt)
				{
					CastSpell("Defensive Stance");
					Log.Print("Defensive Stance -> sunder/Revenge ");
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				if (((Me.Rage >= 5 && tempRevengeCd < CdOffset) || Me.Rage >= 15) && SwitchTarget(tempPreferedTar))
				{
					if (tempRevengeCd < CdOffset)
					{
						CastSpell("Revenge", true, true);
						Log.Print("Cast: Revenge");
					}
					if (Me.Rage >= 15)
					{
						CastSpell("Sunder Armor");
						Log.Print("Cast: Sunder Armor");
					}
					RapidSequence = 400;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
				else if (((Me.Rage >= 5 && tempRevengeCd < CdOffset) || Me.Rage >= 15) && !SwitchTarget(tempPreferedTar))
				{
					Log.Print("Re-init Cast Sunder/Revenge & search target");
					RapidSequence = 0;
					LastTimeRapidChanged = WowHelpers.GetTime();
				}
			}




			if (!WowHelperObj.UnitHasBuff(Me, "Battle Shout") && !overpowerReadyTank && Me.Rage >= 10)
			{
				CastSpell("Battle Shout");
				Log.Print("Cast: Battle Shout");
			}




			//Log.Print("--------End Rapid: "+RapidSequence);
			//Log.Br();
			/*
			 overpower
			 stanceswitch
			 target switch
			 beachte Rapidstance, das muss bei den anderen casts bei 0 sein
			 */
		}
		public override void Combat4()
		{


		}
	}
}
/* 
 Kein Target-switch und sunder/evenge bei CC -> auf Debuff prüfen
  */