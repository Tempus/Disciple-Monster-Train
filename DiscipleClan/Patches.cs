using HarmonyLib;
using I2.Loc;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using static CharacterState;

namespace DiscipleClan
{
    // This patch is used to import localization data when it gets the localization sources
    [HarmonyPatch(typeof(LocalizationManager), "UpdateSources")]
    class LocalizationInjection
    {
        static void Postfix()
        {
            CustomLocalizationManager.ImportCSV("Disciple/chrono/Disciple.csv", ';');
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
            if (__instance.IsPyreHeart())
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
            if (__instance.IsPyreHeart())
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
    [HarmonyPatch(typeof(RoomManager), "UpdateEnchantments")]
    class EnchantUpdateFix
    {
        static void Prefix(RoomManager __instance)
        {
            if (!__instance.AllowEnchantmentUpdates)
            {
                return;
            }
            var toProcessCharacters = new List<CharacterState>();
            ProviderManager.CombatManager.GetMonsterManager().AddCharactersInTowerToList(toProcessCharacters);
            foreach (CharacterState toProcessCharacter in toProcessCharacters)
            {
                toProcessCharacter.TryTriggerEnchantment();
            }
        }
    }

}
