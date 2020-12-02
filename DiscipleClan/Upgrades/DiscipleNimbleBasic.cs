using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleNimbleBasic
    {
        public static string IDName = "NimbleUpgradeBasic";
        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                //upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 35,
                //BonusHP = 0,
                
                StatusEffectUpgrades = new List<StatusEffectStackData> {
                    new StatusEffectStackData { count = 1, statusId = "ambush" },
                    new StatusEffectStackData { count = 1, statusId = "adapted" },
                },
            };

            return railtie;
        }

        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}
