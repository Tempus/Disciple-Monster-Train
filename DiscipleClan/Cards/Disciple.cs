using BepInEx.Logging;
using DiscipleClan.Cards.Shifter;
using DiscipleClan.Upgrades;
using Trainworks;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Units
{
    class Disciple
    {
        public static string IDName = "Disciple";
        public static string imgName = "Disciple";
        public static void Make()
        {
            // Basic Card Stats 
            ChampionCardDataBuilder railyard = new ChampionCardDataBuilder
            {
                Cost = 0,
                Champion = BuildUnit(),
                ChampionIconPath = "chrono/Clan Assets/Icon_ClassSelect_Disciple.png",
                ChampionSelectedCue = "",
                StarterCardData = CustomCardManager.GetCardDataByID(PatternShift.IDName),
                UpgradeTree = new CardUpgradeTreeDataBuilder
                {
                    UpgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleWardmasterBasic.Builder(),
                            DiscipleWardmasterPremium.Builder(),
                            DiscipleWardmasterPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleSymbioteBasic.Builder(),
                            DiscipleSymbiotePremium.Builder(),
                            DiscipleSymbiotePro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleShifterBasic.Builder(),
                            DiscipleShifterPremium.Builder(),
                            DiscipleShifterPro.Builder(),
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

            Utils.AddCardPortrait(railyard, "Disciple");

            // Do this to complete
            railyard.BuildAndRegister();
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

            Utils.AddUnitAnim(characterDataBuilder, "disciple");
            return characterDataBuilder;
        }
    }
}
