using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using static MonsterTrainModdingAPI.Constants.VanillaEnhancerPoolIDs;

namespace DiscipleClan.Enhancers
{
    class SpellUpgradePyreboost
    {
        public static string ID = "SpellUpgradePyreboost";

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
                CardType = CardType.Spell,
                EnhancerPoolIDs = new List<string> { SpellUpgradePool },
                Upgrade = new CardUpgradeDataBuilder
                {
                    UpgradeTitleKey = ID + "_Name",
                    UpgradeDescriptionKey = ID + "_CardDesc",
                    HideUpgradeIconOnCard = false,
                    UpgradeIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Enhancer/" + ID + ".png"),
                    TraitDataUpgradeBuilders = new List<CardTraitDataBuilder>
                    {
                        new CardTraitDataBuilder
                        {
                            TraitStateType = typeof(CardTraitPyreboost)
                        }
                    }
                },
            }.BuildAndRegister();
        }

    }
}
