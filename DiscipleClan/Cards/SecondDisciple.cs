using BepInEx.Logging;
using DiscipleClan.Cards.Chronolock;
using DiscipleClan.Cards.Pyrepact;
using DiscipleClan.Cards.Shifter;
using DiscipleClan.Upgrades;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Units
{
    class SecondDisciple
    {
        public static string IDName = "SecondDisciple";
        public static string imgName = "Peingoop";
        public static void Make()
        {
            var random = new Random();

            List<string> starterCards = new List<string> { Analog.IDName, Firewall.IDName, Flashwing.IDName };
            int index = random.Next(starterCards.Count);
            // Basic Card Stats 
            ChampionCardDataBuilder railyard = new ChampionCardDataBuilder
            {
                Cost = 0,
                Champion = BuildUnit(),
                ChampionIconPath = "chrono/Clan Assets/Icon_ClassSelect_Disciple.png",
                ChampionSelectedCue = "",
                StarterCardData = CustomCardManager.GetCardDataByID(starterCards[index]),
                UpgradeTree = new CardUpgradeTreeDataBuilder
                {
                    UpgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEphemeralBasic.Builder(),
                            DiscipleEphemeralPremium.Builder(),
                            DiscipleEphemeralPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleFlamelinkedBasic.Builder(),
                            DiscipleFlamelinkedPremium.Builder(),
                            DiscipleFlamelinkedPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEchoBasic.Builder(),
                            DiscipleEchoPremium.Builder(),
                            DiscipleEchoPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleNimbleBasic.Builder(),
                            DiscipleNimblePremium.Builder(),
                            DiscipleNimblePro.Builder(),
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
