using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Shifter
{
    class AppleElixir
    {
        public static string IDName = "AppleElixir";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Common,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                        TargetIgnoreBosses = true,
                    },
                },
            };

            railyard.EffectBuilders[0].AddStatusEffect("gravity", 4);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Apple-Elixir.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
