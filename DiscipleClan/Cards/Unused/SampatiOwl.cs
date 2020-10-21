using Trainworks.Builders;
using System.Collections.Generic;

// TODO - Icarian

namespace DiscipleClan.Cards.Unused
{
    class SampatiOwl
    {
        public static string IDName = "Sampati Owl";
        public static string imgName = "HornedOwl";
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

                Size = 1,
                Health = 3,
                AttackDamage = 20,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.EndTurnPreHandDiscard,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffDamage",
                                ParamInt = 10,
                                TargetMode = TargetMode.Self
                            },
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffMaxHealth",
                                ParamInt = 3,
                                TargetMode = TargetMode.Self
                            },
                        }
                    }
                }
            };
            characterDataBuilder.AddStartingStatusEffect("icarian", 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
