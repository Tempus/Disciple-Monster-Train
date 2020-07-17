using DiscipleClan.Cards.Chronolock;
using DiscipleClan.Cards.Pyrepact;
using HarmonyLib;
using I2.Loc;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using static CharacterState;

namespace DiscipleClan.Cards
{
    [HarmonyPatch(typeof(LocalizationManager), "UpdateSources")]
    class RegisterLocalizationStrings
    {
        // Creates and registers card data for each card class
        static void Postfix()
        {
            CustomLocalizationManager.ImportCSV("Disciple/chrono/Disciple.csv");
        }
    }

    //[HarmonyPatch(typeof(CardState), "Setup")]
    //class DebugCrashCards
    //{
    //    // Creates and registers card data for each card class
    //    static void Prefix(CardState __instance, CardData setCardData)
    //    {
    //        API.Log(BepInEx.Logging.LogLevel.All, "Processing Card: " + setCardData.GetID() + " - " + setCardData.GetNameKey());
    //    }
    //}

    //[HarmonyPatch(typeof(CardState), "SetupBodyStatusEffectsText")]
    //class DebugCrashCards
    //{
    //    // Creates and registers card data for each card class
    //    static void Prefix(CardState __instance)
    //    {
    //        API.Log(BepInEx.Logging.LogLevel.All, "Processing Card Status from: " + __instance.GetID() + " - " + __instance.GetAssetName());
    //    }
    //}

    [HarmonyPatch(typeof(CharacterState), "IsImmune")]
    class PyreNotImmuneA
    {
        // Pyre is no longer immune to gaining statuses like Ambush and Armor
        static bool Prefix(CharacterState __instance, ref bool __result, string statusEffectId)
        {
            if (__instance.IsPyreHeart())
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Pyre is no longer immune.");
                __result = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CharacterState), "AddStatusEffect")]
    class PyreNotImmuneB
    {
        // Pyre is no longer immune to gaining statuses like Ambush and Armor
        static void Prefix(CharacterState __instance, string statusId, int numStacks, ref AddStatusEffectParams addStatusEffectParams)
        {
            if (__instance.IsPyreHeart())
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Pyre is no longer immune.");
                addStatusEffectParams.overrideImmunity = true;
            }
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyreSpike.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Dilation.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyreSpike.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Rewind.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Dilation.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyreSpike.IDName));

        }
    }
}

