using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Essences
{
    class JellyScholarEssence
    {
        public static string IDName = "JellyScholarEssence";

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                SourceSynthesisUnit = Trainworks.Managers.CustomCharacterManager.GetCharacterDataByID("Jelly Scholar"),
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
                TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.PostCombat,
                        AdditionalTextOnTrigger = "The Jelly Grows",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                                TargetMode = TargetMode.Self,
                                ParamCardUpgradeData = new CardUpgradeDataBuilder
                                {
                                    BonusHP = 12,
                                    BonusSize = 1,
                                    HideUpgradeIconOnCard = true,
                                }.Build(),
                            },
                        }
                    }
                }

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                //TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
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