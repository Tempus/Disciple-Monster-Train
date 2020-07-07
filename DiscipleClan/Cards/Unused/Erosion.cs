using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;
using UnityEngine;

namespace DiscipleClan.Cards.Unused
{
    class Erosion
    {
        public static string IDName = "Erosion";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 5,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 5,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 5,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        cardTriggers = new List<CardTriggerData>
                        {
                            new CardTriggerData
                            {
                                 persistenceMode = PersistenceMode.SingleBattle,
                                 cardTriggerEffect = "CardTriggerEffectBuffSpellDamage",
                                 paramInt = -2,
                                 buffEffectType = "CardEffectBuffDamage",
                            }
                        }
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
