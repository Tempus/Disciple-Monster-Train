using DiscipleClan.Cards;
using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class EmberOnDivine
    {
        public static string ID = "EmberOnDivine";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicEffectEmberOnUnitTrigger",
                         ParamTrigger = OnDivine.OnDivineCharTrigger.GetEnum(),
                         ParamSourceTeam = Team.Type.Monsters,
                         ParamInt = 2,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
