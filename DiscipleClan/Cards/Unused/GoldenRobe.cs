using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class GoldenRobe
    {
        public static string IDName = "GoldenRobe";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Rare,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes,
                        TargetIgnoreBosses = true,
                        TargetIgnorePyre = true,
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect("loaded", 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "hi.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
