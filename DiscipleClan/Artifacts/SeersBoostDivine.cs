using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;

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
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
