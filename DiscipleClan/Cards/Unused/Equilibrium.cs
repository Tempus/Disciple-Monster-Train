using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Equilibrium
    {
        public static string IDName = "Equilibrium";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Common,

                TargetsRoom = true,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBump",
                        ParamInt = -1,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBump",
                        ParamInt = 1,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Monsters,
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "cropped.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
