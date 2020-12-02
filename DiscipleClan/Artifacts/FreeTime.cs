using DiscipleClan.Cards;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaRelicPoolIDs;
using HarmonyLib;

namespace DiscipleClan.Artifacts
{
    class FreeTime
    {
        public static string ID = "BullshitThing";

        public static void Make()
        {
            var relic = new CollectableRelicDataBuilder
            {
                IconPath = "Relic/FreeTime.png",
                RelicPoolIDs = new List<string> { MegaRelicPool },
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicEffectModifyCardTypeCost",
                         ParamCardType = CardType.Spell,
                         ParamInt = -1,
                    },
                    new RelicEffectDataBuilder
                    {
                         RelicEffectClassName = "RelicEffectAddTrait",
                         ParamCardType = CardType.Spell,
                         ParamSourceTeam = Team.Type.Monsters,
                         Traits = new List<CardTraitData> { new CardTraitData { traitStateName = "CardTraitExhaustState" } }
                    },
                }
            };
            Utils.AddRelic(relic, ID);

            var r = relic.BuildAndRegister();
            r.GetNameEnglish();
        }
    }

    [HarmonyPatch(typeof(RelicEffectAddTrait), "ApplyCardStateModifiers")]
    class FreeTimeLoadingPatch
    {
        static bool Prefix(RelicEffectAddTrait __instance, CardState cardState, SaveManager saveManager, CardManager cardManager, RelicManager relicManager)
        {
            if (cardManager == null)
                return false;
            return true;
        }
    }
}
