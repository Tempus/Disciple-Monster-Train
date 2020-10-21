using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Ascend
    {
        public static string IDName = "Ascend";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBump",
                        ParamInt = 1,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Monsters,
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
