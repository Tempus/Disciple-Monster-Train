using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleSymbioteBasic
    {
        public static string IDName = "SymbioteUpgradeBasic";
        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                //UpgradeDescriptionKey = IDName + "_Desc",
                //upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 0,
                BonusHP = 10,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //BonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                //TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> {
                    new RoomModifierDataBuilder
                    {
                        RoomStateModifierClassName = typeof(RoomStateModifierTempUpgradePerSpaceUsed).AssemblyQualifiedName,
                        DescriptionKey = IDName + "_Desc",
                        ParamInt = 5
                    }
                },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
            };

            return railtie;
        }

        public static CardUpgradeData Make() { return Builder().Build(); }
    }
}
