using BepInEx.Logging;
using DiscipleClan.Cards.Chronolock;
using DiscipleClan.Cards.Pyrepact;
using DiscipleClan.Cards.Shifter;
using DiscipleClan.Upgrades;
using Trainworks;
using Trainworks.Builders;
using Trainworks.Managers;
using System;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Units
{
    class SecondDisciple
    {
        public static string IDName = "SecondDisciple";
        public static string imgName = "Peingoop";
        public static void Make()
        {
            var random = new Random();

            // Basic Card Stats 
            ChampionCardDataBuilder railyard = new ChampionCardDataBuilder
            {
                Cost = 0,
                Champion = BuildUnit(),
                ChampionIconPath = "chrono/Clan Assets/Icon_ClassSelect_Hero2.png",
                ChampionSelectedCue = "",
                StarterCardData = CustomCardManager.GetCardDataByID(Analog.IDName),
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
                    },
                },

                CardID = IDName,
                NameKey = IDName + "_Name",
                //OverrideDescriptionKey = IDName + "_Desc",
                LinkedClass = DiscipleClan.getClan(),
                ClanID = Clan.IDName,

                CardPoolIDs = new List<string> { "Chrono", UnitsAllBanner },
                CardType = CardType.Monster,
                TargetsRoom = true,

                AssetPath = Utils.rootPath + Utils.ucardPath,
            };

            Utils.AddCardPortrait(railyard, "Hero2");

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

            Utils.AddUnitAnim(characterDataBuilder, "hero2");
            return characterDataBuilder;
        }
    }
}
