using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using DiscipleClan.Cards.Units;
using DiscipleClan.Cards.Spells;
using ShinyShoe.Logging;
using MonsterTrainModdingAPI;
using BepInEx.Logging;
using System.Reflection;
using System.Linq;
using UnityEngine.Assertions.Must;
using System.IO;
using I2.Loc;

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

    //[HarmonyPatch(typeof(CardState), "SetupEffects")]
    //class DebugCrashCards
    //{
    //    // Creates and registers card data for each card class
    //    static void Prefix(CardState __instance, CardData cardData, SaveManager saveManager)
    //    {
    //        API.Log(BepInEx.Logging.LogLevel.All, "Processing Card Effect from: " + __instance.GetID() + " - " + __instance.GetAssetName());
    //    }
    //}

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            // Units
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Units"));
            foreach (var cardType in types) {
                var field = cardType.GetField("IDName");
                __instance.AddCardToDeck(CustomCardManager.GetCardDataByID((string)field.GetValue(null)));
            }

            // Spells
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Spells") && !t.Name.Contains("<>"));
            foreach (var cardType in types)
            {
                var field = cardType.GetField("IDName");
                __instance.AddCardToDeck(CustomCardManager.GetCardDataByID((string)field.GetValue(null)));
            }

            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(Scry.IDName));

            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("[S] Ascend"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("[S] Descend"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Disperse"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Equilibrium"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Instant Replay"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Past Mistakes"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("For Testing"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Wax Pinion"));

            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Crunch Time"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Infinity"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Pendulum"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Rebound"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Time Lash"));

            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Seek"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Seek"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Seek"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Seek"));

            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Ancient Savant"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Auspex"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Backilisk"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Chain Dragon"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Constrictor"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Destiny Stealer"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Diviner of the Infinite"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Fortune Finder"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Frigga Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Hermes Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Hole Anole"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Idun Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Jelly Scholar"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Lakshimi Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Lash Lizard"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID(MinervaOwl.IDName));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Ragana Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Sampati Owl"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Sklink"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Snecko"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Time Eater"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Waxwing"));
            //__instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Yoremaster"));
            //}
        }
    }
}

