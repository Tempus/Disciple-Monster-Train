using DiscipleClan.CardEffects;
using HarmonyLib;
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
                AssetPath = "chrono/Enhancer/" + ID + ".png",
                Rarity = CollectableRarity.Uncommon,
                CardType = CardType.Spell,
                EnhancerPoolIDs = new List<string> { SpellUpgradePool },
                Upgrade = new CardUpgradeDataBuilder
                {
                    UpgradeTitleKey = ID + "_Name",
                    UpgradeDescriptionKey = ID + "_CardDesc",
                    HideUpgradeIconOnCard = false,
                    UpgradeIconPath = ("chrono/Enhancer/" + ID + ".png"),
                    TraitDataUpgradeBuilders = new List<CardTraitDataBuilder>
                    {
                        new CardTraitDataBuilder
                        {
                            TraitStateType = typeof(CardTraitPyreboost)
                        },
                    },
                    FiltersBuilders = new List<CardUpgradeMaskDataBuilder>
                    {
                        new CardUpgradeMaskDataBuilder
                        {
                            CardType = CardType.Monster,
                            UpgradeDisabledReason = CardState.UpgradeDisabledReason.NotEligible,
                            RequiredCardEffects = new List<string>
                            {
                                "CardEffectDamage",
                                "CardEffectHeal",
                                "CardEffectHealAndDamageRelative",
                                typeof(CardEffectEmberwave).AssemblyQualifiedName
                            },
                            RequiredCardEffectsOperator = CardUpgradeMaskDataBuilder.CompareOperator.Or,
                            DisallowedCardPools = Traverse.Create(ProviderManager.SaveManager.GetAllGameData().FindEnhancerDataByName("SpellMagicPower").GetEffects()[0].GetParamCardUpgradeData().GetFilters()[0]).Field("disallowedCardPools").GetValue<List<CardPool>>(),
                        },
                    }
                },
            }.BuildAndRegister();
        }

    }
}
