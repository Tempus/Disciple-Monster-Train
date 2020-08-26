using DiscipleClan.CardEffects;
using DiscipleClan.Cards;
using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaRelicPoolIDs;

namespace DiscipleClan.Artifacts
{
    class FirstBuffExtraStack
    {
        public static string ID = "FirstBuffExtraStack";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "chrono/Relic/Mirrorc.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = typeof(RelicEffectFirstBuffExtraStack).AssemblyQualifiedName,
                         ParamInt = 1,
                         ParamSourceTeam = Team.Type.Monsters,
                    }
                },
                
            };

            ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statMan);
            List<StatusEffectStackData> statuses = new List<StatusEffectStackData>();
            foreach (var status in statMan.GetAllStatusEffectsData().GetStatusEffectData())
            {
                if (status.GetDisplayCategory() != StatusEffectData.DisplayCategory.Persistent)
                {
                    var stack = new StatusEffectStackData { statusId = status.GetStatusId(), count = 1 };
                    statuses.Add(stack);
                }
            }
            relic.EffectBuilders[0].ParamStatusEffects = statuses.ToArray();
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }
}
