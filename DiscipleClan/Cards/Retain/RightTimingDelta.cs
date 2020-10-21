using DiscipleClan.CardEffects;
using HarmonyLib;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Retain
{
    class RightTimingDelta
    {
        public static string IDName = "RightTimingDelta";

        public static void Make()
        {
            CardPool cardPool = UnityEngine.ScriptableObject.CreateInstance<CardPool>();

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Common,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 15,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        CardEffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddBattleCard),
                                AdditionalParamInt = 1,
                                ParamInt = 1,
                                ParamCardPool = cardPool
                            }
                        }
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = typeof(CardTraitScalingAddDamagePerCard).AssemblyQualifiedName,
                        ParamTrackedValue = CardStatistics.TrackedValueType.TypeInDeck,
                        ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                        ParamCardType = CardStatistics.CardTypeTarget.Any,
                        ParamStr = IDName,
                        ParamUseScalingParams = true,
                        ParamInt = 15,
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Right-Timing.png");

            // Do this to complete
            CardData forPool = railyard.BuildAndRegister();

            var cardDataList = (Malee.ReorderableArray<CardData>)AccessTools.Field(typeof(CardPool), "cardDataList").GetValue(cardPool);
            cardDataList.Add(forPool);
        }
    }
}
