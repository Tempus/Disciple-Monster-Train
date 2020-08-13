﻿using DiscipleClan.CardEffects;
using HarmonyLib;
using I2.Loc;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Tayx.Graphy.Utils.NumString;
using static CharacterState;
using static TargetHelper;

namespace DiscipleClan
{
    // This patch is used to import localization data when it gets the localization sources
    [HarmonyPatch(typeof(LocalizationManager), "UpdateSources")]
    class LocalizationInjection
    {
        static void Postfix()
        {
            CustomLocalizationManager.ImportCSV("chrono/Disciple.csv", ';');
        }
    }

    // This attempted to make Wards into spells, but failed.
    //[HarmonyPatch(typeof(CardState), "IsSpawnerCard")]
    //class WardShouldBeSpell
    //{
    //    static bool Postfix(bool isMon, CardState __instance)
    //    {
    //        SubtypeData wardSub;
    //        CustomCharacterManager.CustomSubtypeData.TryGetValue("ChronoSubtype_Ward", out wardSub);
    //        if (__instance.GetSpawnCharacterData() != null)
    //            if (__instance.GetSpawnCharacterData().GetSubtypes()[0].Equals(wardSub))
    //                return true;
    //        return isMon;
    //    }
    //}

    // This is workaround for Pyre immunity
    [HarmonyPatch(typeof(CharacterState), "IsImmune")]
    class PyreNotImmuneA
    {
        // Pyre is no longer immune to gaining statuses like Ambush and Armor
        static bool Prefix(CharacterState __instance, ref bool __result, string statusEffectId)
        {
            if (__instance.IsPyreHeart() && !__instance.PreviewMode)
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Pyre is no longer immune to: " + statusEffectId);
                __result = false;
                return false;
            }
            return true;
        }
    }

    // This is workaround for Pyre immunity. This was added in the Wild Mutators patch, because one hardcoded immunity check was not enough!
    [HarmonyPatch(typeof(CharacterState), "AddStatusEffect", new Type[] { typeof(string), typeof(int), typeof(AddStatusEffectParams) })]
    class PyreNotImmuneB
    {
        // Pyre is no longer immune to gaining statuses like Ambush and Armor
        static void Prefix(CharacterState __instance, string statusId, int numStacks, ref AddStatusEffectParams addStatusEffectParams)
        {
            if (__instance.IsPyreHeart() && !__instance.PreviewMode)
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Pyre is overriding immunity for: " + statusId);
                addStatusEffectParams.overrideImmunity = true;
            }
        }
    }

    // This makes unit upgrades in battle affect the size of the unit, because for some reason in-battle upgrades are attack/def only
    [HarmonyPatch(typeof(CharacterState), "ApplyCardUpgrade")]
    class SizeUpgradePatch
    {
        static void Prefix(CharacterState __instance, CardUpgradeState cardUpgradeState, RoomManager ___roomManager)
        {
            Traverse.Create(__instance).Property("PrimaryStateInformation").Property("Size").SetValue(__instance.GetSize() + cardUpgradeState.GetAdditionalSize());
        }
    }

    // This fixes Enchantments for players, which are hardcoded to only check heroes
    //[HarmonyPatch(typeof(RoomManager), "UpdateEnchantments")]
    //class EnchantUpdateFix
    //{
    //    static void Prefix(RoomManager __instance)
    //    {
    //        if (!__instance.AllowEnchantmentUpdates)
    //        {
    //            return;
    //        }
    //        var toProcessCharacters = new List<CharacterState>();
    //        ProviderManager.CombatManager.GetMonsterManager().AddCharactersInTowerToList(toProcessCharacters);
    //        foreach (CharacterState toProcessCharacter in toProcessCharacters)
    //        {
    //            toProcessCharacter.TryTriggerEnchantment();
    //        }
    //    }
    //}

    // This Makes the squoosher look cute
    [HarmonyPatch(typeof(CharacterUI), "UpdateCharacterSize")]
    class SquoosherSize
    {
        static void Postfix(CharacterUI __instance, CharacterState characterState)
        {
            if (characterState.GetSourceCharacterData().GetID() == "Squoosher")
            {
                float scale = characterState.GetMaxHP().ToFloat() / 30.0f;
                __instance.SetScale(scale);
            }
        }
    }

    // This fixes TargetIgnoresBosses for all target modes, because the base game just forgets to pass it?
    [HarmonyPatch(typeof(TargetHelper), "CollectTargets", new Type[] { typeof(CollectTargetsData), typeof(List<CharacterState>) }, new ArgumentType[] { HarmonyLib.ArgumentType.Normal, HarmonyLib.ArgumentType.Ref })]
    class BossTargetIgnoreFix
    {
        public static bool targetIgnoreBosses = true;
        static void Prefix(CollectTargetsData data, ref List<CharacterState> targets)
        {
            if (data.targetIgnoreBosses)
            {
                targetIgnoreBosses = true;
                return;
            }
            targetIgnoreBosses = false;
        }
    }

    [HarmonyPatch(typeof(TargetHelper), "CheckTargetsOverride")]
    class BossTargetIgnoreFixB
    {
        static void Postfix(CardEffectState effectState, List<CharacterState> targets, SpawnPoint dropLocation, SubtypeData targetSubtype)
        {
            if (BossTargetIgnoreFix.targetIgnoreBosses && effectState.GetTargetMode() == TargetMode.DropTargetCharacter && dropLocation != null)
            {
                CharacterState characterState = dropLocation.GetCharacterState();
                if (characterState.IsMiniboss() || characterState.IsOuterTrainBoss())
                {
                    targets.Clear();
                    // lastTargetedCharacters.Clear();
                }
            }
        }
    }

    [HarmonyPatch(typeof(CardEffectState), "GetModifiedParam")]
    class MagicPowerIsntShit
    {
        static void Postfix(CardEffectState __instance, ref int __result, int startingParam)
        {
            CardStateModifiers cardStateModifiers = null;
            CardStateModifiers cardStateModifiers2 = null;
            if (__instance.GetParentCardState() != null)
            {
                cardStateModifiers = __instance.GetParentCardState().GetCardStateModifiers();
                cardStateModifiers2 = __instance.GetParentCardState().GetTemporaryCardStateModifiers();
            }
            if (__instance.GetCardEffect() is CardEffectEmberwave)
            {
                __result = CardStateModifiers.GetUpgradedStatValue(__result, CardStateModifiers.StatType.Damage, cardStateModifiers);
                __result = CardStateModifiers.GetUpgradedStatValue(__result, CardStateModifiers.StatType.Damage, cardStateModifiers2);
            }
        }
    }

    [HarmonyPatch(typeof(CardManager), "DrawChosenUnitCardInHand")]
    class DebugPriorityDraw
    {
        static void Prefix(CardManager __instance)
        {
            for (int i = 0; i < __instance.GetDrawPile().Count; i++)
            {
                CardState cardState = __instance.GetDrawPile()[i];
                CharacterData spawnCharacterData = cardState.GetSpawnCharacterData();
                if (!(spawnCharacterData != null))
                {
                    continue;
                }
                foreach (SubtypeData subtype in spawnCharacterData.GetSubtypes())
                {
                    if (subtype.Key == "SubtypesData_Chosen")
                    {
                        API.Log(BepInEx.Logging.LogLevel.All, "Chosen Unit: " + spawnCharacterData.name);
                        return;
                    }
                    API.Log(BepInEx.Logging.LogLevel.All, "Unchosen Unit: " + spawnCharacterData.name);
                }
            }
        }
    }

    //[HarmonyPatch(typeof(DeckScreen), "FilterCardStatesByRelicEffect")]
    //class Idunnowhatwrong
    //{
    //    static void Prefix(DeckScreen __instance, List<CardState> cardStatesToFilter, RelicEffectData ___relicEffectData)
    //    {
    //        API.Log(BepInEx.Logging.LogLevel.All, "What up 1");
    //        if (___relicEffectData.GetParamCardType() != CardType.Invalid)
    //        {
    //            API.Log(BepInEx.Logging.LogLevel.All, "What up 1.1");
    //            cardStatesToFilter.RemoveAll((CardState c) => c.GetCardType() != ___relicEffectData.GetParamCardType());
    //        }
    //        API.Log(BepInEx.Logging.LogLevel.All, "What up 2");
    //        if (!___relicEffectData.GetParamCharacterSubtype().IsNone)
    //        {
    //            API.Log(BepInEx.Logging.LogLevel.All, "What up 2.1");
    //            for (int num = cardStatesToFilter.Count - 1; num >= 0; num--)
    //            {
    //                API.Log(BepInEx.Logging.LogLevel.All, "What up 2.1." + num);
    //                foreach (CardEffectState effectState in cardStatesToFilter[num].GetEffectStates())
    //                {
    //                    API.Log(BepInEx.Logging.LogLevel.All, "What up 2.1." + num + " some loop");
    //                    if (effectState.GetCardEffect() is CardEffectSpawnMonster && !effectState.GetParamCharacterData().GetSubtypes().Contains(___relicEffectData.GetParamCharacterSubtype()))
    //                    {
    //                        API.Log(BepInEx.Logging.LogLevel.All, "What up 2.2");
    //                        cardStatesToFilter.RemoveAt(num);
    //                    }
    //                }
    //            }
    //        }
    //        API.Log(BepInEx.Logging.LogLevel.All, "What up 3");
    //        if (___relicEffectData.GetUseIntRange())
    //        {
    //            API.Log(BepInEx.Logging.LogLevel.All, "What up 3.1");
    //            cardStatesToFilter.RemoveAll((CardState c) => c.GetCostWithoutAnyModifications() < ___relicEffectData.GetParamMinInt() || c.GetCostWithoutAnyModifications() > ___relicEffectData.GetParamMaxInt());
    //        }
    //    } 
    //}
}
