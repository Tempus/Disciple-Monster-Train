﻿using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleWardmasterBasic
    {
        public static string IDName = "WardmasterUpgradeBasic";
        public static int damageAmount = 5;

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                UseUpgradeHighlightTextTags = true,
                BonusDamage = -5,
                BonusHP = 10,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                        DescriptionKey = IDName + "_Desc",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddWard),
                                ParamStr = "WardStateRandomDamage",
                                ParamInt = damageAmount,
                                ParamBool = true,
                            },

                        }
                    },
                },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //StatusEffectUpgrades = new List<StatusEffectStackData> { },
            };

            return railtie;
        }
        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}