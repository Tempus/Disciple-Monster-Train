using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleWardmasterPro
    {
        public static string IDName = "WardmasterUpgradePro";
        public static int damageAmount = 20;

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 0,
                BonusHP = 60,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.PostCombat,
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