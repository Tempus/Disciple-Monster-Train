using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Upgrades
{
    class DiscipleFlamelinkedPro
    {
        public static string IDName = "FlamelinkedUpgradePro";
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
                //bonusDamage = 0,
                //bonusHP = 0,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //bonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                //triggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //roomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
                statusEffectUpgrades = new List<StatusEffectStackData> {
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
                    new StatusEffectStackData
                    {
                        statusId = "pyrelink",
                        count = 20,
                    },
                }
            };

            return railtie;
        }
        public static CardUpgradeData Make() { return Builder().Build(); }

    }
}
