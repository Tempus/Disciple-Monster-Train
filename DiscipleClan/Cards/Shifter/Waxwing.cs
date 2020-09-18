using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though)

namespace DiscipleClan.Cards.Pyrepact
{
    class Waxwing
    {
        public static string IDName = "Waxwing";
        public static string imgName = "FountainPenguin";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Rare,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, imgName + ".png");
            railyard.CardPoolIDs = new List<string> { "Chrono", MegaPool };

            // Do this to complete
            railyard.BuildAndRegister();
        }

        // Builds the unit
        public static CharacterData BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                NameKey = IDName + "_Name",
                SubtypeKeys = new List<string> { "ChronoSubtype_Seer" },
                PriorityDraw = false,

                Size = 1,
                Health = 3,
                AttackDamage = 20,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                                ParamCardUpgradeData = new CardUpgradeDataBuilder {
                                     BonusDamage = 5,
                                }.Build(),
                                TargetMode = TargetMode.Self,
                            }
                        }
                    }
                }
            };
            characterDataBuilder.AddStartingStatusEffect("icarian", 1);

            Utils.AddUnitAnim(characterDataBuilder, "waxwing");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
