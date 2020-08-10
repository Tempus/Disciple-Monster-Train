﻿using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class GravityOnAscend
    {
        public static string ID = "GravityOnAscend";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/Sample.png",
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
