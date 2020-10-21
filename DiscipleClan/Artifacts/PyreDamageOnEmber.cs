using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class PyreDamageOnEmber
    {
        public static string ID = "PyreDamageOnEmber";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/PyreFyre.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
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

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
