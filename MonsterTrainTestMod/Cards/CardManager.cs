using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

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
            // NotHornBreakDataCreator.CreateCardData(allGameData);

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
            // NotHornBreakDataCreator.CreateCardData(allGameData);
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SetupRun")]
    class AddToStartingDeck
    {
        // Adds cards to the starting deck
        static void Postfix(ref SaveManager __instance)
        {
            // __instance.AddCardToDeck(CustomCardManager.GetCardDataByID("NotHornBreak"));
        }
    }
}
