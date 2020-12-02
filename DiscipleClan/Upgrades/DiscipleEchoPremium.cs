using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleEchoPremium
    {
        public static string IDName = "EchoUpgradePremium";
        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                //upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 35,
                BonusHP = 10,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //BonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> {
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.OnUnscaledSpawn,
                        DescriptionKey = IDName + "_Desc",
                        HideTriggerTooltip = true,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectShareBuffs),
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Monsters,
                                ParamBool = true,
                                ParamInt = 2,
                                AdditionalParamInt = 1,
                                ParamMultiplier = 0.5f,
                                AdditionalTooltips = new AdditionalTooltipData[] {
                                    new AdditionalTooltipData {
                                        isTriggerTooltip = true,
                                        titleKey = "Echo_TooltipTitle",
                                        descriptionKey = "Echo_TooltipText",
                                    },
                                    new AdditionalTooltipData {
                                        isTriggerTooltip = true,
                                        titleKey = "Reflect_TooltipTitle",
                                        descriptionKey = "Reflect_TooltipText",
                                    },
                                },
                                HideTooltip = true,

                            }
                        }
                    }
                },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },

            };

            return railtie;
        }

        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}
