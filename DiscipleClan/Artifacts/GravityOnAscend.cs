using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class GravityOnAscend
    {
        public static string ID = "GravityOnAscend";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
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

            relic.BuildAndRegister();
        }
    }
}
