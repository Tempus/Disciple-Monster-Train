using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

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
                RelicPoolIDs = new List<string> { MegaRelicPool },
                //EffectBuilders = new List<RelicEffectDataBuilder>
                //{
                //    new RelicEffectDataBuilder
                //    {
                //         RelicEffectClassName = "RelicEffectAddStatusEffectOnSpawn",
                //         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "loaded", count = 5 } },
                //         ParamSourceTeam = Team.Type.Heroes,
                //         ParamBool = false,
                //         ParamTargetMode = TargetMode.FrontInRoom,
                //         ParamCardType = CardType.Monster,
                //    }
                //}
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
