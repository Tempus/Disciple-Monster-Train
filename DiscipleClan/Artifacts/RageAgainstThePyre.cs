using DiscipleClan.Cards;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using static CharacterState;

namespace DiscipleClan.Artifacts
{
    class RageAgainstThePyre
    {
        public static string ID = "RageAgainstThePyre";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                AssetPath = "Sample.png",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassName = "RelicEffectAddStatusEffectOnSpawn",
                        ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId = "buff", count = 4 } },
                        ParamSourceTeam = Team.Type.Heroes,
                        ParamBool = false,
                        ParamTargetMode = TargetMode.FrontInRoom,
                        ParamCardType = CardType.Monster,
                    },
                },
            };
            Utils.AddRelic(relic, ID);

            relic.BuildAndRegister();
        }
    }

    [HarmonyPatch(typeof(CharacterState), "ApplyDamage")]
    class PyreHeartRageImmune
    {
        static void Prefix(CharacterState __instance, ref int damage, ApplyDamageParams damageParams)
        {
            SaveManager saveManager;
            ProviderManager.TryGetProvider<SaveManager>(out saveManager);

            if (damageParams.attacker != null)
                if (damageParams.attacker.GetStatusEffectStacks("buff") > 0)
                    if (saveManager.GetRelicCount("RageAgainstThePyre") > 0)
                        damage = 0;
        }
    }
}
