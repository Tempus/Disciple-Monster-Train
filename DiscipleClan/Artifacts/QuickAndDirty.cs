using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class QuickAndDirty
    {
        public static string ID = "QuickAndDirty";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/Metronome.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectApplyStatusOnHitIfStatus).AssemblyQualifiedName,
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "dazed", count = 1 } },
                         ParamSourceTeam = Team.Type.Monsters,
                         ParamTrigger = CharacterTriggerData.Trigger.OnAttacking,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }
}
