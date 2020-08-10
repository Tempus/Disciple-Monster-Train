using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class BullshitThing
    {
        public static string ID = "BullshitThing";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/Sample.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
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

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
