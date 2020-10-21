using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Upgrades
{
    class DiscipleChainDragonBasic
    {
        public static string IDName = "DiscipleChainDragonBasic";

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 0,
                BonusHP = 11,

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
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.Self,
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId=Multistrike } }
                            }
                        }
                    },
                },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //StatusEffectUpgrades = new List<StatusEffectStackData> { new StatusEffectStackData { count = 1, statusId = "spellless" } },
            };

            return railtie;
        }
        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}