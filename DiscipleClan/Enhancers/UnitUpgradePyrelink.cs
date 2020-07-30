using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using static MonsterTrainModdingAPI.Constants.VanillaEnhancerPoolIDs;

namespace DiscipleClan.Enhancers
{
    class UnitUpgradePyrelink
    {
        public static string ID = "UnitUpgradePyrelink";

        public static void Make()
        {
            new EnhancerDataBuilder
            {
                ID = ID,
                ClanID = DiscipleClan.clanRef.GetID(),
                NameKey = ID + "_Name",
                DescriptionKey = ID + "_Desc",
                AssetPath = "Disciple/chrono/Enhancer/" + ID + ".png",
                Rarity = CollectableRarity.Common,
                CardType = CardType.Monster,
                EnhancerPoolIDs = new List<string> { UnitUpgradePoolCommon },
                Upgrade = new CardUpgradeDataBuilder
                {
                    UpgradeTitleKey = ID + "_Name",
                    UpgradeDescriptionKey = ID + "_CardDesc",
                    HideUpgradeIconOnCard = false,
                    UpgradeIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Enhancer/" + ID + ".png"),
                    StatusEffectUpgrades = new List<StatusEffectStackData>
                    {
                        new StatusEffectStackData
                        {
                            statusId = "pyrelink",
                            count = 2,
                        }
                    },
                    FiltersBuilders = new List<CardUpgradeMaskDataBuilder>
                    {
                        new CardUpgradeMaskDataBuilder
                        {
                            CardType = CardType.Monster,
                            UpgradeDisabledReason = CardState.UpgradeDisabledReason.CardType,
                        }
                    }
                },
            }.BuildAndRegister();
        }

    }
}
