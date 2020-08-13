using DiscipleClan.Artifacts;
using DiscipleClan.Cards.Chronolock;
using DiscipleClan.Cards.Prophecy;
using DiscipleClan.Cards.Pyrepact;
using DiscipleClan.Cards.Retain;
using DiscipleClan.Cards.Shifter;
using DiscipleClan.Cards.Speedtime;
using DiscipleClan.Cards.Unused;
using HarmonyLib;
using I2.Loc;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.Cards
{
    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            // Gotta make my own "CardEffectAddStatusToPyre"
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Firewall.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Firewall.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Firewall.IDName));

            // Adding these because I've mussed up the start effects, and I need to test the Chain Dragon friend (and ensure Wardmaster is targeting enemies)
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PatternShift.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PatternShift.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PatternShift.IDName));

            // Needs to summon Slag, remove capacity, check if can be played on capcity floors
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyromancyWardBeta.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyromancyWardBeta.IDName));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(PyromancyWardBeta.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Cinderborn.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Hootstorian.IDName));

            // Slag should have a chance of being cute statues, or having hats, etc. Like the cute rocks

            //__instance.AddRelic(__instance.GetAllGameData().FindCollectableRelicData(GoldOverTime.ID));
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
}

