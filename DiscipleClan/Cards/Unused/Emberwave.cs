using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Emberwave
    {
        public static string IDName = "Emberwave";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Common,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectEmberwave).AssemblyQualifiedName,
                        ParamInt = 5,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    },
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Emberwave.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
