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
using System.Net.Sockets;
using UnityEngine;

namespace DiscipleClan.Cards
{
    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID(AncientSavant.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(SunspottedOwl.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(SunspottedOwl.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(SunspottedOwl.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(SunspottedOwl.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Cinderborn.IDName));

            //__instance.AddRelic(__instance.GetAllGameData().FindCollectableRelicData(FirstBuffExtraStack.ID));
        }
    }

    [HarmonyPatch(typeof(CompendiumSectionCards), "IsCardUnlockedAndDiscovered")]
    class RevealAllCards
    {
        // Creates and registers card data for each card class
        static void Postfix(ref bool __result)
        {
            __result = true;
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

