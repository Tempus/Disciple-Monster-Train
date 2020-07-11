using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Pyrepact
    {
        public static string IDName = "Pyrepact";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Rare,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        TargetMode = TargetMode.Pyre,
                        TargetIgnorePyre = false,
                        ParamInt = 10,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    },
                },
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitMagneticState",
                    },
                },
            };

            railyard.EffectBuilders[0].AddStatusEffect("pyreboost", 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "sigmaligma.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
