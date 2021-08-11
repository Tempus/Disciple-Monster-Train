using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Essences
{
    class FateBehemothEssence
    {
        public static string IDName = "FateBehemothEssence";

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                SourceSynthesisUnit = Trainworks.Managers.CustomCharacterManager.GetCharacterDataByID("Diviner of the Infinite"),
                //upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,

                //BonusDamage = 0,
                //BonusHP = 2,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //BonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                //TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
                TraitDataUpgradeBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitFreeze"
                    }
                },

                CardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        CardEffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(CardEffectAddTempUpgrade).AssemblyQualifiedName,
                                ParamCardUpgradeData = new CardUpgradeDataBuilder {
                                    CostReduction = 1,
                                }.Build(),
                                TargetMode = TargetMode.Self,
                            }
                        }
                    }
                }

                /*StatusEffectUpgrades = new List<StatusEffectStackData> {
                    new StatusEffectStackData
                    {
                        statusId = "pyreboost",
                        count = 1,
                    },
                    new StatusEffectStackData
                    {
                        statusId = "hideuntilboss",
                        count = 1,
                    },
                },*/
            };

            return railtie;
        }
        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}