using DiscipleClan.Triggers;
using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Pyrepact
{
    class Cinderborn
    {
        public static string IDName = "Cinderborn";
        public static string imgName = "Clocktopus";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Cinderborn");
            railyard.OverrideDescriptionKey = string.Empty; // We have to do this to get the Trigger tooltips to display correctly

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
                SubtypeKeys = new List<string> { "ChronoSubtype_Eternal" },

                Size = 2,
                Health = 10,
                AttackDamage = 4,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = OnGainEmber.OnGainEmberCharTrigger.GetEnum(),
                        DescriptionKey = IDName + "_Desc",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(CardEffectScalingUpgrade).AssemblyQualifiedName,
                                ParamCardUpgradeData = new CardUpgradeDataBuilder {
                                     BonusDamage = 2,
                                }.Build(),
                                TargetMode = TargetMode.Self,
                            }
                        }
                    }
                }
            };
            // Unit art asset, complex stuff!
            Utils.AddUnitAnim(characterDataBuilder, "cinderborn");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
