using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainTestMod.Cards.Units;
using MonsterTrainTestMod.Cards.Spells;

namespace MonsterTrainTestMod.Cards
{
    [HarmonyPatch(typeof(SaveManager), "Initialize")]
    class RegisterCards
    {
        // Creates and registers card data for each card class
        static void Postfix(ref SaveManager __instance)
        {
            AllGameData allGameData = __instance.GetAllGameData();

            // Champion
            // Snecko.Make();

            // Starter
            PatternShift.Make();

            // Units
			AncientSavant.Make();
			Auspex.Make();
			Backilisk.Make();
			ChainDragon.Make();
			Constrictor.Make();
			DestinyStealer.Make();
			DivineroftheInfinite.Make();
			FortuneFinder.Make();
			FriggaOwl.Make();
			HermesOwl.Make();
			HoleAnole.Make();
			IdunOwl.Make();
			JellyScholar.Make();
			LakshimiOwl.Make();
			LashLizard.Make();
			MinervaOwl.Make();
			RaganaOwl.Make();
			SampatiOwl.Make();
			Sklink.Make();
			Snecko.Make();
			TimeEater.Make();
			Waxwing.Make();
			Yoremaster.Make();

            // Spells
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Pattern Shift"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Pattern Shift"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Pattern Shift"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Pattern Shift"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Ancient Savant"));

            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Ancient Savant"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Auspex"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Backilisk"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Chain Dragon"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Constrictor"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Destiny Stealer"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Diviner of the Infinite"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Fortune Finder"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Frigga Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Hermes Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Hole Anole"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Idun Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Jelly Scholar"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Lakshimi Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Lash Lizard"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Minerva Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Ragana Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Sampati Owl"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Sklink"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Snecko"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Time Eater"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Waxwing"));
            __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("Yoremaster"));
        }
    }
}
