using HarmonyLib;
using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Upgrades
{
    [HarmonyPatch(typeof(CardTooltipContainer), "AddTooltipsCharacter")]
    class DiscipleEphemeral_AddTooltips
    {
        static void Postfix(CardTooltipContainer __instance, CardState cardState, CardStateModifiers cardModifiers, CardStateModifiers tempCardModifiers, SaveManager saveManager)
        {
            // This generates additional generic tooltips for both the card and the character, hardcoded to this character to avoid conflicts
            if (cardState.GetID() != "e124d0b1-0c5e-4f6b-98a0-4b70faabf752")
                return;

            // This check will find cards that have upgrades already applied to them
            foreach (CardUpgradeState cardUpgrade in cardModifiers.GetCardUpgrades())
            {
                foreach (RoomModifierData roomModifier in cardUpgrade.GetRoomModifierUpgrades())
                {
                    if (roomModifier.GetRoomStateModifierClassName() == typeof(RoomStateModifierStartersConsumeRebate).AssemblyQualifiedName)
                    {
                        // Add tooltips for both Consume and Rebate (in that order)
                        __instance.InstantiateTooltip(CardTraitData.GetTraitCardTextLocalizationKey("CardTraitExhaustState"), TooltipDesigner.TooltipDesignType.Keyword)?.InitCardExplicitTooltip(CardTraitData.GetTraitCardTextLocalizationKey("CardTraitExhaustState"), CardTraitData.GetTraitTooltipTextLocalizationKey("CardTraitExhaustState"));
                        __instance.InstantiateTooltip("Rebate_TooltipTitle", TooltipDesigner.TooltipDesignType.Keyword)?.InitCardExplicitTooltip("Rebate_TooltipTitle", "Rebate_TooltipBody");
                    }
                }
            }

            // This check will find cards with temporary upgrades (ie. In the Dark Forge)
            foreach (CardUpgradeState cardUpgrade in tempCardModifiers.GetCardUpgrades())
            {
                foreach (RoomModifierData roomModifier in cardUpgrade.GetRoomModifierUpgrades())
                {
                    Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Found Temp RoomModifierData");
                    if (roomModifier.GetRoomStateModifierClassName() == typeof(RoomStateModifierStartersConsumeRebate).AssemblyQualifiedName)
                    {
                        Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Match!");
                        // Add tooltips for both Consume and Rebate (in that order)
                        __instance.InstantiateTooltip(CardTraitData.GetTraitCardTextLocalizationKey("CardTraitExhaustState"), TooltipDesigner.TooltipDesignType.Keyword)?.InitCardExplicitTooltip(CardTraitData.GetTraitCardTextLocalizationKey("CardTraitExhaustState"), CardTraitData.GetTraitTooltipTextLocalizationKey("CardTraitExhaustState"));
                        __instance.InstantiateTooltip("Rebate_TooltipTitle", TooltipDesigner.TooltipDesignType.Keyword)?.InitCardExplicitTooltip("Rebate_TooltipTitle", "Rebate_TooltipBody");
                    }
                }
            }
        }
    }

    class DiscipleEphemeralBasic
    {
        public static string IDName = "EphemeralUpgradeBasic";
        public static int buffAmount = 1;

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                UpgradeTitleKey = IDName + "_Name",
                UpgradeDescriptionKey = IDName + "_Desc",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Clan Assets/clan_32.png"),
                //HideUpgradeIconOnCard = false,
                UseUpgradeHighlightTextTags = true,
                //BonusDamage = 5,
                BonusHP = 20,
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
                         roomStateModifierClassName = typeof(RoomStateModifierStartersConsumeRebate).AssemblyQualifiedName,
                         DescriptionKey = "RoomStateModifierStartersConsumeRebate_Desc1",
                         ParamInt = 1,
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