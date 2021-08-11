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
using Trainworks;
using Trainworks.Managers;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;

namespace DiscipleClan.Cards
{
    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(JellyScholar.IDName));

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

    [HarmonyPatch(typeof(CompendiumRelicUI), "SetLocked")]
    class RevealAllRelics
    {
        // Creates and registers card data for each card class
        static bool Prefix(CompendiumRelicUI __instance)
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(UpgradeTreeUI), "RefreshDiscovered")]
    class RevealAllChamps
    {
        // Sets all upgrades for champs to revealed in the log
        static void Prefix(UpgradeTreeUI __instance, MetagameSaveData metagameSave)
        {
            foreach (ClassData classData in ProviderManager.SaveManager.GetBalanceData().GetClassDatas())
            {
                for (int i = 0; i < 2; i++)
                {
                    foreach (CardUpgradeTreeData.UpgradeTree upgradeTree in classData.GetUpgradeTree(i).GetUpgradeTrees())
                    {
                        foreach (CardUpgradeData cardUpgrade in upgradeTree.GetCardUpgrades())
                        {
                            metagameSave.MarkChampionUpgradeDiscovered(cardUpgrade);
                        }
                    }
                }
            }
        }
    }

    //[HarmonyPatch(typeof(CardState), "Setup")]
    //class DebugCrashCards
    //{
    //    // Creates and registers card data for each card class
    //    static void Prefix(CardState __instance, CardData setCardData)
    //    {
    //        Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Processing Card: " + setCardData.GetID() + " - " + setCardData.GetNameKey());
    //    }
    //}

    //[HarmonyPatch(typeof(CardState), "SetupBodyStatusEffectsText")]
    //class DebugCrashCards
    //{
    //    // Creates and registers card data for each card class
    //    static void Prefix(CardState __instance)
    //    {
    //        Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Processing Card Status from: " + __instance.GetID() + " - " + __instance.GetAssetName());
    //    }
    //}
}

