using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class GravityOnAscend
    {
        public static string ID = "GravityOnAscend";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "Relic/ErrantPinwheel.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectAddStatusOnMonsterAscend).AssemblyQualifiedName,
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "gravity", count = 1 } },
                         ParamSourceTeam = Team.Type.Monsters,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
