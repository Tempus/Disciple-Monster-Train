using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class PendulumDelta
    {
        public static string IDName = "PendulumDelta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectFlipStatusEffects).AssemblyQualifiedName,
                        ParamBool = true,
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectFlipStatusEffects).AssemblyQualifiedName,
                        ParamBool = false,
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Monsters
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

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Pendulum.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
