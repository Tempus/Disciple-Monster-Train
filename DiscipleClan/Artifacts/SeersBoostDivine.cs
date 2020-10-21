using DiscipleClan.Cards;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class SeersBoostDivine
    {
        public static string ID = "SeersBoostDivine";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/Sample.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
            };
            Utils.AddRelic(relic, ID);

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
