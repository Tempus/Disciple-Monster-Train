using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class BullshitThing
    {
        public static string ID = "BullshitThing";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicEffectModifyCardTypeCost",
                         ParamCardType = CardType.Spell,
                         ParamInt = -99,
                    },
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicEffectAddTrait",
                         ParamCardType = CardType.Spell,
                         ParamSourceTeam = Team.Type.Monsters,
                         Traits = new List<CardTraitData> { new CardTraitData { traitStateName = "CardTraitExhaustState" } }
                    },
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
