using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class SeersBoostDivine
    {
        public static string ID = "SeersBoostDivine";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
