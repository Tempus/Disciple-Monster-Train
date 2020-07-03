using DiscipleClan.Cards.Triggers;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Cards.Upgrades
{
    class DiscipleShifterPro
    {
        public static string IDName = "ShifterUpgradePro";
        public static int buffAmount = 6;

        public static CardUpgradeDataBuilder Builder()
        {
            CardUpgradeDataBuilder railtie = new CardUpgradeDataBuilder
            {
                upgradeTitleKey = IDName + "_Name",
                upgradeDescriptionKey = IDName + "_Desc",
                upgradeNotificationKey = IDName + "_Notice",
                //upgradeIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
                //hideUpgradeIconOnCard = false,
                useUpgradeHighlightTextTags = true,
                bonusDamage = 15,
                bonusHP = 25,
                //costReduction = 0,
                //xCostReduction = 0,
                //bonusHeal = 0,
                //bonusSize = 0,

                //traitDataUpgradeBuilders = new List<CardTraitDataBuilder> { },
                triggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>
                {
                    // Shifter
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.EndTurnPreHandDiscard,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(ShinyShoe.CardEffectTeleport).AssemblyQualifiedName,
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Heroes,
                            }
                        }
                    },

                    // Buff everyone on the floor when we shift
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CustomTriggerManager.GetTrigger(typeof(MTCharacterTrigger_Relocate)),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffDamage",
                                ParamInt = buffAmount,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            },

                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffMaxHealth",
                                ParamInt = buffAmount,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    },
                },
                //cardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder> { },
                //roomModifierUpgradeBuilders = new List<RoomModifierDataBuilder> { },
                //filtersBuilders = new List<CardUpgradeMaskDataBuilder> { },
                //upgradesToRemoveBuilders = new List<CardUpgradeDataBuilder> { },
                //statusEffectUpgrades = new List<StatusEffectStackData> { },
            };

            return railtie;
        }
        public static CardUpgradeData Make() { return Builder().Build(); }

    }
}