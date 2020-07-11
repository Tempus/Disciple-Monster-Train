using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class PyreDamageOnEmber
    {
        public static string ID = "PyreDamageOnEmber";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectPyreDamageOnEmber).AssemblyQualifiedName,
                         ParamInt = 1,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
