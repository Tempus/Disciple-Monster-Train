using DiscipleClan.CardEffects;
using HarmonyLib;
using I2.Loc;
using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Tayx.Graphy.Utils.NumString;
using static CharacterState;
using static TargetHelper;
using static Trainworks.Constants.VanillaStatusEffectIDs;
using System.Collections;

namespace DiscipleClan
{
    // This is workaround for Pyre immunity
    [HarmonyPatch(typeof(CharacterState), "IsImmune")]
    class PyreNotImmuneA
    {
        // Pyre is no longer immune to gaining statuses like Ambush and Armor
        static bool Prefix(CharacterState __instance, ref bool __result, string statusEffectId)
        {
            if (__instance.IsPyreHeart() && !__instance.PreviewMode && (statusEffectId == Quick || statusEffectId == Armor))
            {
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
            if (__instance.IsPyreHeart() && !__instance.PreviewMode && (statusId == Quick || statusId == Armor))
            {
                addStatusEffectParams.overrideImmunity = true;
            }
        }
    }

    // This makes unit upgrades in battle affect the size of the unit, because for some reason in-battle upgrades are attack/def only
    [HarmonyPatch(typeof(CharacterState), "ApplyCardUpgrade")]
    class SizeUpgradePatch
    {
        static IEnumerator Postfix(IEnumerator __result, CharacterState __instance, CardUpgradeState cardUpgradeState)
        {
            Traverse.Create(__instance).Property("PrimaryStateInformation").Property("Size").SetValue(__instance.GetSize() + cardUpgradeState.GetAdditionalSize());
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            if (ProviderManager.CombatManager.IsPlayerActionPhase())
            {
                var room = roomManager.GetRoom(__instance.GetCurrentRoomIndex());
                Traverse.Create(room).Field("roomUI").Field<RoomCapacityUI>("roomCapacityUI").Value.Show((room), false);
            }
            //yield return roomManager.GetRoom(__instance.GetCurrentRoomIndex()).AdjustCapacity(Team.Type.Monsters, 0, false);
            yield return __result;
        }
    }

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
                        return;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(CardTraitScalingReturnConsumedCards), "GetCurrentEffectText")]
    class RewindDisplaysTheCorrectNumber
    {
        static void Postfix(ref string __result, CardTraitScalingReturnConsumedCards __instance, CardStatistics cardStatistics)
        {
            if (cardStatistics != null)
            {
                if (!ProviderManager.TryGetProvider<CardManager>(out CardManager cardManager)) { return; }
                if (!ProviderManager.TryGetProvider<PlayerManager>(out PlayerManager playerManager)) { return; };

                if (cardManager.GetExhaustedPile() == null) { return; }

                __result = "CardTraitScalingReturnConsumedCards_CurrentScaling_CardText".Localize(new LocalizedIntegers(UnityEngine.Mathf.Min(playerManager.GetEnergy(), cardManager.GetExhaustedPile().FindAll(x => !x.GetDebugName().Contains("Rewind")).Count)));
            }
        }
    }

    [HarmonyPatch(typeof(CardManager), "RestoreNextExhaustedCard")]
    class RewindCantRewindItself
    {
        static bool Prefix(ref bool __result, CardManager __instance)
        {

            __instance.GetExhaustedPile().Shuffle(RngId.CardDraw);
            var card = __instance.GetExhaustedPile().Find(x => !x.GetDebugName().Contains("Rewind"));

            // Nothing to restore if the deck only has Rewinds
            if (card == null)
            {
                __result = false;
                return false;
            }
            __result = __instance.RestoreExhaustedOrEatenCard(card);

            __instance.ShuffleDeck(false);
            return false;
        }
    }
}
