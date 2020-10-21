using Trainworks.Builders;
using System.Collections.Generic;

// TODO - The timing of this effect is not going to be correct right now I think. Maybe this needs to be a roomModifier

namespace DiscipleClan.Cards.Unused
{
    class Yoremaster
    {
        public static string IDName = "Yoremaster";
        public static string imgName = "Peingoop";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
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

                Size = 2,
                Health = 22,
                AttackDamage = 5,



                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnTurnBegin,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.Room
                            }
                        }
                    }
                }
            };

            characterDataBuilder.AddStartingStatusEffect("slow", 1);
            characterDataBuilder.TriggerBuilders[0].EffectBuilders[0].AddStatusEffect("slow", 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
