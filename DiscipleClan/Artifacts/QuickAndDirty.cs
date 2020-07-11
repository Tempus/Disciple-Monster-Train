using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Artifacts
{
    class QuickAndDirty
    {
        public static string ID = "QuickAndDirty";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectApplyStatusOnHitIfStatus).AssemblyQualifiedName,
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "dazed", count = 1 } },
                         ParamSourceTeam = Team.Type.Monsters,
                         ParamTrigger = CharacterTriggerData.Trigger.OnHit,
                    }
                }
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
