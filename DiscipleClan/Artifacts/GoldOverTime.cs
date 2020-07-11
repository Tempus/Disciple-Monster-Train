using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class GoldOverTime
    {
        public static string ID = "GoldOverTime";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicAddStatusEffectOnSpawn",
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "loaded", count = 5 } },
                         ParamSourceTeam = Team.Type.Heroes,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
