using DiscipleClan.CardEffects;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Chronolock
{
    class Analog
    {
        public static string IDName = "Analog";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Common,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddClassStatus),
                        ParamInt = 2,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Pendulum.png");
            railyard.CardPoolIDs = new List<string>();

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
