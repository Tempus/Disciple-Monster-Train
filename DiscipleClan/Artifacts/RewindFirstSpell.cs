using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class RewindFirstSpell
    {
        public static string ID = "RewindFirstSpell";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "Relic/SecondHand.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectRewind).AssemblyQualifiedName,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
