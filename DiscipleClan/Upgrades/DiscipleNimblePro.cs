using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Upgrades
{
    class DiscipleNimblePro
    {
        public static string IDName = "NimbleUpgradePro";
        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                upgradeTitleKey = IDName + "_Name",
                upgradeDescriptionKey = IDName + "_Desc",
                //upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
                //hideUpgradeIconOnCard = false,
                useUpgradeHighlightTextTags = true,
                bonusDamage = 5,
                //bonusHP = 0,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //bonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                triggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { 
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.AfterSpawnEnchant,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="ambush" } },
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    }
                },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //roomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
                statusEffectUpgrades = new List<StatusEffectStackData> { 
                    new StatusEffectStackData { count = 4, statusId = "multistrike" }
                },
            };

            return railtie;
        }

        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}
