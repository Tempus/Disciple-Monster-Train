using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    class DiscipleEphemeralPremium
    {
        public static string IDName = "EphemeralUpgradePremium";
        public static int buffAmount = 2;
        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,
                BonusDamage = 5,
                BonusHP = 50,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //BonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                //TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder> { },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder>
                {
                    new RoomModifierDataBuilder
                    {
                         roomStateModifierClassName = typeof(RoomStateModifierRefund).AssemblyQualifiedName,
                         ParamInt = buffAmount,
                         DescriptionKey = "RoomModifierRefund_Desc" + buffAmount,
                         ExtraTooltipTitleKey = "RoomModifierRefund_TooltipTitle",
                         ExtraTooltipBodyKey = "RoomModifierRefund_TooltipBody",
                    },
                    new RoomModifierDataBuilder
                    {
                         roomStateModifierClassName = typeof(RoomStateModifierStartersTempConsume).AssemblyQualifiedName,
                         ParamInt = buffAmount,
                         DescriptionKey = "RoomStateModifierStartersTempConsume_Desc",
                    },
                },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
                //StatusEffectUpgrades = new List<StatusEffectStackData> { },
            };

            return railtie;
        }

        public static CardUpgradeData Make() { return Builder().Build(); }

    }
}