using System;
using System.Collections.Generic;
using System.Text;
using DiscipleClan.CardEffects;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Spells
{
    class Refractor
    {
        public static string IDName = "Refractor";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Rarity = CollectableRarity.Rare,

                EffectTriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.CardSpellPlayed,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(CardEffectHealTrainPassive).AssemblyQualifiedName,
                                ParamInt = 1,
                            }
                        }
                    }
                },

                //TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                //{
                //    new CardTriggerEffectDataBuilder
                //    {
                //        trigger = CardTriggerType.OnAnyUnitDeathOnFloor,
                //        EffectBuilders = new List<CardEffectDataBuilder>
                //        {
                //            new CardEffectDataBuilder
                //            {
                //                EffectStateName = "CardEffectHealTrain",
                //                ParamInt = 1,
                //            }
                //        }
                //    }
                //}

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitUnplayable"
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
