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
	public class PlayerRotation
	{
		protected BindingList<ActionButtonAndSpell> Abasl { get; set; }
		protected Timer CheckHookTimer;
		protected globalKeyboardHook Gkh;
		protected WowHelpers WowHelperObj;
		protected long LastSendKey = 0;
		protected long BlockSendKeyTime = 140;
		protected uint Stance = 0;
		public PlayerRotation()
		{
		}

        //private void CheckForNewAuras(double dist = 30)
        //{
        //    //Log.Print("www");
        //    List<WowObject> tempCurrList = WowHelperObj.CachedUnitlist.Where(o => o.Distance <= dist && (o.Type == (short)Constants.ObjType.OT_PLAYER || o.Type == (short)Constants.ObjType.OT_UNIT)).ToList();
        //   // Log.Print("ddd " + tempCurrList.Count);
        //    List<WowObject> tempLastList = WowHelperObj.LastCachedUnitList.Where(o => o.Distance <= dist && (o.Type == (short)Constants.ObjType.OT_PLAYER || o.Type == (short)Constants.ObjType.OT_UNIT)).ToList();
        //    Log.Print("eee " + tempLastList.Count);
        //    foreach (WowObject tempCurr in tempCurrList)
        //    {
        //        foreach (WowObject tempLast in tempLastList)
        //        {
        //            if (tempLast.DebuffList != null)
        //            {
        //                //if (tempCurr.DebuffList.Exists(o => tempLast.DebuffList.FirstOrDefault(x => x.Id == o.Id && x.Id != 0))
        //                //if (tempCurr.DebuffList.Exists(o => o.Id == tempLast.DebuffList.ForEach(x => x.Id != 0)
        //                bool buffStillActive = false;
        //                foreach (Auras a in tempLast.DebuffList)
        //                {
        //                    //tempCurr.DebuffList.(o => o.Id == 0 && o.Id != a.Id && o.Id != 0 && tempCurr.Guid == tempLast.Guid);
        //                    buffStillActive = tempCurr.DebuffList.Exists(o => o.Id == a.Id && a.Id != 0);
        //                    if (buffStillActive)
        //                        break;
        //                    Auras tempEmptyAura = new Auras("", 1, _stacks: 0);
        //                    (tempCurr.DebuffList.FirstOrDefault(o => o.Id != a.Id && o.Id != 0) ?? tempEmptyAura).TimeApplied = WowHelpers.GetTime();
        //                }
        //            }

        //        }
        //    }
        //}

		public void Init(BindingList<ActionButtonAndSpell> _abasl, Timer _checkHookTimer, globalKeyboardHook _gkh, WowHelpers _wowHelperObj)
		{
			Abasl = _abasl;
			CheckHookTimer = _checkHookTimer;
			Gkh = _gkh;
			WowHelperObj = _wowHelperObj;
		}

		protected string KeysBlocked()
		{
			if (LastSendKey < (WowHelpers.GetTime() - BlockSendKeyTime))
			{
				return "free";
			}
			return "blocked";
		}
		public bool GotRelevantTargetFromList(List<WowObject> targetList, bool surpressBlockKeys = true)
		{
			
			if (LastSendKey < WowHelpers.GetTime() - BlockSendKeyTime || surpressBlockKeys)
			{
				//WowHelperObj.LocalPlayer.Target = WowHelperObj.GetCurrentTarget();
				if (targetList.FirstOrDefault().Guid != WowHelperObj.LocalPlayer.Target.Guid)
				{
					SendKeys.Send("{TAB}");
					Log.Print("Sendkeys: [TAB]");
					if (surpressBlockKeys == false) // surpressBlockKeys == true -> no globalCooldown of revlex ui is triggered
					{
						LastSendKey = WowHelpers.GetTime(); //to set the time rhere is important, because if tmpAbas = null (spell not available), it will skip to the next command in rotation
					}
					return false;
				}
				Log.Print("correct Target! [1]");
				return true;
			}
			return false;
		}
		public bool SwitchTarget(WowObject unit, bool surpressBlockKeys = true)
		{
			
			if (LastSendKey < WowHelpers.GetTime() - BlockSendKeyTime || surpressBlockKeys)
			{
				//WowHelperObj.LocalPlayer.Target = WowHelperObj.GetCurrentTarget();
				if (unit.Guid != WowHelperObj.LocalPlayer.Target.Guid)
				{
					SendKeys.Send("{TAB}");
					Log.Print("Sendkeys: [TAB]" + unit.Guid.ToString() + "   tar:" + WowHelperObj.LocalPlayer.Target.Guid.ToString() + "   preferedTarget == tar?:" + (unit.Guid == WowHelperObj.LocalPlayer.Target.Guid) + "   lastSendKey:" + LastSendKey);
					if (surpressBlockKeys == false) // surpressBlockKeys == true -> no globalCooldown of revlex ui is triggered
					{
						LastSendKey = WowHelpers.GetTime(); //to set the time rhere is important, because if tmpAbas = null (spell not available), it will skip to the next command in rotation
					}
					return false;
				}
				Log.Print("correct Target! [2]");
				return true;
			}
			return false;
		}


		public void CheckForHostileFaction()
		{
			//check for hostile faction
			//if (WowHelperObj.LocalPlayer.Target.DynamicFlags == (uint)Pointers.DynamicFlags.TappedByMe)
			//{
			//	if (!WowHelperObj.HostileFaction[WowHelperObj.LocalPlayer.Target.FactionTemplate] && !WowHelperObj.IsHostile(WowHelperObj.LocalPlayer.Target))
			//	{
			//		Log.Print("Marked " + WowHelperObj.LocalPlayer.Target.FactionTemplate + " as hostile");
			//	}
			//	WowHelperObj.HostileFaction[WowHelperObj.LocalPlayer.Target.FactionTemplate] = true;
			//}

            //check for hostile faction
            if (WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget > 0)
            {
                if (!WowHelperObj.HostileFaction[WowHelperObj.LocalPlayer.Target.FactionTemplate] && !WowHelperObj.IsHostile(WowHelperObj.LocalPlayer.Target))
                {
                    Log.Print("Marked " + WowHelperObj.LocalPlayer.Target.FactionTemplate + " as hostile");
                }
                WowHelperObj.HostileFaction[WowHelperObj.LocalPlayer.Target.FactionTemplate] = true;
            }
        }

		public void StartMeleeAttack()
		{
			CheckForHostileFaction();
			//Log.Print(KeysBlocked().ToString() + ": StartMeleeAttack");
			Stance = WowHelperObj.GetStance();
			if (WowHelperObj.CheckConnection() && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0)
			{
				ActionButtonAndSpell pressAttack = Abasl.Where(C => C.SpellId == 6603 && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
				Log.Print("Sendkeys: [" + pressAttack.ModifierDecoded + "] [" + pressAttack.Key + "] [" + pressAttack.Modifier + "] \t" + Stance.ToString() + " \t" + pressAttack.SpellName + " \t AttackGuid: " + WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget.ToString());
				SendKeys.Send(pressAttack.ModifierDecoded + pressAttack.Key.ToLower());
			}
		}

		public bool CastSpell(uint spellId, bool enableAutoAttack = true, bool surpressBlockKeys = false)
		{
			CheckForHostileFaction();
            //CheckForNewAuras();
            //Log.Print(KeysBlocked() + ": " + spellId + " \t");
            Stance = WowHelperObj.GetStance();
			if (LastSendKey < (WowHelpers.GetTime() - BlockSendKeyTime) && WowHelperObj.CheckConnection())
			{
				ActionButtonAndSpell tmpAbas = Abasl.Where(C => C.SpellId == spellId && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
				if (tmpAbas != null && WowHelperObj.SpellExist((int)spellId))
				{
					if (enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0)
					{
						ActionButtonAndSpell pressAttack = Abasl.Where(C => C.SpellId == 6603 && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
						//Log.Print("Sendkeys: [" + pressAttack.ModifierDecoded + "] [" + pressAttack.Key + "] [" + pressAttack.Modifier + "] \t" + Stance.ToString() + " \t" + pressAttack.SpellName + " \t AttackGuid: " + WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget.ToString().Substring(1,3));
						SendKeys.Send(pressAttack.ModifierDecoded + pressAttack.Key.ToLower());
					}
					Log.Print("Sendkeys: " + ((enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0) ? "[+ Attack]" : "")  + " [" + tmpAbas.ModifierDecoded + "] [" + tmpAbas.Key + "] [" + tmpAbas.Modifier + "] \t" + Stance.ToString() + " " + tmpAbas.SpellName + " \t trigger key block: " + surpressBlockKeys.ToString());
					SendKeys.Send(tmpAbas.ModifierDecoded + tmpAbas.Key.ToLower());
					// last time this spell was casted
					WowHelperObj.SpellsInSpellBook.Where(C => C.Id == spellId).FirstOrDefault().LastCast = WowHelpers.GetTime();
					if (surpressBlockKeys == false) // surpressBlockKeys == true -> no globalCooldown of revlex ui is triggered
					{
						LastSendKey = WowHelpers.GetTime(); //to set the time rhere is important, because if tmpAbas = null (spell not available), it will skip to the next command in rotation
					}
					return true;
				}
				else
				{
					Log.Print("Couldnt find spell " + spellId + ". No key binding for it?");
				}
			}
			return false;
		}
		public bool CastSpell(string spellName, bool enableAutoAttack = true, bool surpressBlockKeys = false)
		{
			CheckForHostileFaction();
            //CheckForNewAuras();
            //Log.Print(KeysBlocked() + ": " + spellId + " \t");
            Stance = WowHelperObj.GetStance();
			if (LastSendKey < (WowHelpers.GetTime() - BlockSendKeyTime) && WowHelperObj.CheckConnection())
			{
				
				ActionButtonAndSpell tmpAbas = Abasl.Where(C => C.SpellName == spellName && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
				if (tmpAbas != null && WowHelperObj.SpellExist(spellName))
				{
					if (enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0)
					{
						ActionButtonAndSpell pressAttack = Abasl.Where(C => C.SpellId == 6603 && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
						//Log.Print("Sendkeys: [" + pressAttack.ModifierDecoded + "] [" + pressAttack.Key + "] [" + pressAttack.Modifier + "] \t" + Stance.ToString() + " \t" + pressAttack.SpellName + " \t AttackGuid: " + WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget.ToString().Substring(1, 3));
						SendKeys.Send(pressAttack.ModifierDecoded + pressAttack.Key.ToLower());
					}

					Log.Print("Sendkeys: " + ((enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0) ? "[+ Attack]" : "") + " [" + tmpAbas.ModifierDecoded + "] [" + tmpAbas.Key + "] [" + tmpAbas.Modifier + "] \t" + Stance.ToString() + " " + tmpAbas.SpellName + " \t trigger key block: " + surpressBlockKeys.ToString());
					SendKeys.Send(tmpAbas.ModifierDecoded + tmpAbas.Key.ToLower());
					// last time this spell was casted
					WowHelperObj.SpellsInSpellBook.Where(C => C.Name == spellName).FirstOrDefault().LastCast = WowHelpers.GetTime();
					if (surpressBlockKeys == false) // surpressBlockKeys == true -> no globalCooldown of revlex ui is triggered
					{
						LastSendKey = WowHelpers.GetTime(); //to set the time rhere is important, because if tmpAbas = null (spell not available), it will skip to the next command in rotation
					}
					return true;
				}
				else
				{
					Log.Print("Couldnt find spell " + spellName + ". No key binding for it?");
				}
			}
			return false;
		}
		public bool CastMacroById(uint macroId, bool enableAutoAttack = true, bool surpressBlockKeys = false)
		{
            //CheckForNewAuras();
            //Log.Print(KeysBlocked() + ": " + macroId + " \t");
            Stance = WowHelperObj.GetStance();
			if (LastSendKey < (WowHelpers.GetTime() - BlockSendKeyTime) && WowHelperObj.CheckConnection())
			{
				ActionButtonAndSpell tmpAbas = Abasl.Where(C => C.MacroId == macroId && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
				if (tmpAbas != null)
				{
					if (enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0)
					{
						ActionButtonAndSpell pressAttack = Abasl.Where(C => C.SpellId == 6603 && (C.Stance == Stance || C.Stance == 99)).FirstOrDefault();
						//Log.Print("Sendkeys: [" + pressAttack.ModifierDecoded + "] [" + pressAttack.Key + "] [" + pressAttack.Modifier + "] \t" + Stance.ToString() + " " + pressAttack.SpellName);
						SendKeys.Send(pressAttack.ModifierDecoded + pressAttack.Key.ToLower());
					}
					Log.Print("Sendkeys: " + ((enableAutoAttack && WowHelperObj.LocalPlayer.GuidOfAutoAttackTarget == 0) ? "[+ Attack]" : "") + " [" + tmpAbas.ModifierDecoded + "] [" + tmpAbas.Key + "] [" + tmpAbas.Modifier + "] \t" + Stance.ToString() + " " + tmpAbas.SpellName + " \t trigger key block: " + surpressBlockKeys.ToString());
					SendKeys.Send(tmpAbas.ModifierDecoded + tmpAbas.Key.ToLower());
					if (surpressBlockKeys == false) // surpressBlockKeys == true -> no globalCooldown of revlex ui is triggered
					{
						LastSendKey = WowHelpers.GetTime(); //to set the time rhere is important, because if tmpAbas = null (spell not available), it will skip to the next command in rotation
					}
					return true;
				}
				else
				{
					Log.Print("Couldnt find macro " + macroId + ". No key binding for it?");
				}

			}
			return false;
		}
		public void UseItemById(uint itemId)
		{
			if (WowHelperObj.CheckConnection())
			{
				ActionButtonAndSpell tmpAbas = Abasl.Where(C => C.ItemId == itemId).FirstOrDefault();
				if (tmpAbas != null)
				{
					SendKeys.Send(tmpAbas.ModifierDecoded + tmpAbas.Key.ToLower());
					//Log.Print("Sendkeys: [" + tmpAbas.ModifierDecoded +"] ["+ tmpAbas.Key + "] [" + tmpAbas.Modifier+"]");
				}
			}
		}
		public virtual void Combat1()
		{
			Log.Print("combat 1");
		}
		public virtual void Combat2()
		{
			Log.Print("combat 2");
		}
		public virtual void Combat3()
		{
			Log.Print("combat 3");
		}
		public virtual void Combat4()
		{
			Log.Print("combat 4");
		}
		public virtual void Combat5()
		{
			Log.Print("combat 5");
		}
		public virtual void Combat6()
		{
			Log.Print("combat 6");
		}
		public virtual void Combat7()
		{
			Log.Print("combat 7");
		}
		public virtual void Combat8()
		{
			Log.Print("combat 8");
		}
		public virtual void Combat9()
		{
			Log.Print("combat 9");
		}
		public virtual void Combat10()
		{
			Log.Print("combat 10");
		}
		public virtual void Combat11()
		{
			Log.Print("combat 11");
		}
	}
}
