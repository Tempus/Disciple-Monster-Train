using MonsterTrainModdingAPI.Builders;
using ShinyShoe;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Shifter
{
    class PatternShift
    {
        public static string IDName = "Pattern Shift";

        public static void Make()
        {
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Starter,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectTeleport).AssemblyQualifiedName,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "IMAG0286.jpg");

            railyard.BuildAndRegister();
        }
    }
}
