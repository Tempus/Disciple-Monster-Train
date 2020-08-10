﻿using BepInEx.Logging;
using DiscipleClan.Cards.Shifter;
using DiscipleClan.Upgrades;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Units
{
    class SecondDisciple
    {
        public static string IDName = "SecondDisciple";
        public static string imgName = "Disciple";
        public static void Make()
        {
            // Basic Card Stats 
            ChampionCardDataBuilder railyard = new ChampionCardDataBuilder
            {
                Cost = 0,
                Champion = BuildUnit(),
                ChampionIcon = "chrono/Clan Assets/Icon_ClassSelect_Disciple.png",
                StarterCardData = CustomCardManager.GetCardDataByID(PatternShift.IDName),
                UpgradeTree = new CardUpgradeTreeDataBuilder
                {
                    UpgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEmbermarkBasic.Builder(),
                            DiscipleEmbermarkPremium.Builder(),
                            DiscipleEmbermarkPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleNimbleBasic.Builder(),
                            DiscipleNimblePremium.Builder(),
                            DiscipleNimblePro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleTarotBasic.Builder(),
                            DiscipleTarotPremium.Builder(),
                            DiscipleTarotPro.Builder(),
                        },
                    },
                },

                CardID = IDName,
                NameKey = IDName + "_Name",
                OverrideDescriptionKey = IDName + "_Desc",
                LinkedClass = DiscipleClan.getClan(),
                ClanID = Clan.IDName,

                CardPoolIDs = new List<string> { "Chrono", UnitsAllBanner },
                CardType = CardType.Monster,
                TargetsRoom = true,

                AssetPath = Utils.rootPath + Utils.ucardPath,
            };

            if (!railyard.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, railyard.NameKey + ",Text,,,,," + railyard.CardID + ",,,,,");
            if (!railyard.OverrideDescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, railyard.OverrideDescriptionKey + ",Text,,,,,<desc>,,,,,");

            Utils.AddImg(railyard, imgName + ".png");

            // Do this to complete
            railyard.BuildAndRegister(1);
        }

        // Builds the unit
        public static CharacterDataBuilder BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                NameKey = IDName + "_Name",

                Size = 2,
                Health = 10,
                AttackDamage = 5,
            };

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder;
        }
    }
}