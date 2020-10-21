using DiscipleClan.Triggers;
using Trainworks.Builders;
using Trainworks.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using static Trainworks.Constants.VanillaEnhancerPoolIDs;

namespace DiscipleClan.Enhancers
{
    class UnitUpgradeRelocate
    {
        public static string ID = "UnitUpgradeRelocate";

        public static void Make()
        {
            new EnhancerDataBuilder
            {
                ID = ID,
                ClanID = DiscipleClan.clanRef.GetID(),
                NameKey = ID + "_Name",
                DescriptionKey = ID + "_Desc",
                AssetPath = "chrono/Enhancer/" + ID + ".png",
                Rarity = CollectableRarity.Common,
                CardType = CardType.Monster,
                EnhancerPoolIDs = new List<string> { UnitUpgradePoolCommon },
                Upgrade = new CardUpgradeDataBuilder
                {
                    UpgradeTitleKey = ID + "_Name",
                    UpgradeDescriptionKey = ID + "_CardDesc",
                    HideUpgradeIconOnCard = false,
                    UpgradeIconPath = ("chrono/Enhancer/" + ID + ".png"),
                    TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>
                    {
                        new CharacterTriggerDataBuilder {
                            Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                            EffectBuilders = new List<CardEffectDataBuilder>
                            {
                                new CardEffectDataBuilder
                                {
                                    EffectStateName = "CardEffectAddStatusEffect",
                                    TargetMode = TargetMode.Self,
                                    ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId="gravity", count=1 } }
                                }
                            }
                        },
                    },
                    FiltersBuilders = new List<CardUpgradeMaskDataBuilder>
                    {
                        new CardUpgradeMaskDataBuilder
                        {
                            CardType = CardType.Monster,
                            UpgradeDisabledReason = CardState.UpgradeDisabledReason.NotEligible,
                            ExcludedStatusEffects = new List<StatusEffectStackData>
                            {
                                new StatusEffectStackData { statusId = "immobile", count=1 }
                            },
                        }
                    }
                },
            }.BuildAndRegister();
        }

    }
}
