using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using static MonsterTrainModdingAPI.Constants.VanillaEnhancerPoolIDs;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Enhancers
{
    class UnitUpgradeSweep
    {
        public static string ID = "UnitUpgradeSweep";

        public static void Make()
        {
            new EnhancerDataBuilder
            {
                ID = ID,
                //ClanID = DiscipleClan.clanRef.GetID(),
                NameKey = ID + "_Name",
                DescriptionKey = ID + "_Desc",
                AssetPath = "Disciple/chrono/Enhancer/" + ID + ".png",
                Rarity = CollectableRarity.Uncommon,
                CardType = CardType.Monster,
                EnhancerPoolIDs = new List<string> { UnitUpgradePool },
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
                            statusId = Sweep,
                            count = 1,
                        }
                    }
                },
            }.BuildAndRegister();
        }

    }
}
