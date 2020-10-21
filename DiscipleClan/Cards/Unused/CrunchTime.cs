using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Unused
{
    class CrunchTime
    {
        public static string IDName = "Crunch Time";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes,
                    }

                },
            };

            railyard.EffectBuilders[0].AddStatusEffect(Dazed, 1);
            railyard.EffectBuilders[1].AddStatusEffect(Quick, 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "body-building-fitness-sports-athlete-implementation-person-royalty-free-thumbnail.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
