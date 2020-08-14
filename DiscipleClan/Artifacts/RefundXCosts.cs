using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class RefundXCosts
    {
        public static string ID = "RefundXCosts";

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
                         RelicEffectClassName = typeof(RelicEffectXCostRefund).AssemblyQualifiedName,
                         ParamInt = 2,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
