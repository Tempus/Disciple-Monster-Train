using DiscipleClan.Upgrades;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan
{
    class Clan
    {
        public static string IDName = "Chrono";

        public static ClassData Make()
        {
            var copyClan = CustomClassManager.SaveManager.GetAllGameData().GetAllClassDatas()[1];

            ClassDataBuilder clan = new ClassDataBuilder
            {
                ClassID = IDName,
                Name = IDName + "_Name",
                Description = IDName + "_Class",
                SubclassDescription = IDName + "_SubClass",

                UpgradeTreeBuilder = new CardUpgradeTreeDataBuilder
                {
                    UpgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEmbermarkBasic.Builder(),
                            DiscipleEmbermarkPremium.Builder(),
                            DiscipleEmbermarkPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleFlamelinkedBasic.Builder(),
                            DiscipleFlamelinkedPremium.Builder(),
                            DiscipleFlamelinkedPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleShifterBasic.Builder(),
                            DiscipleShifterPremium.Builder(),
                            DiscipleShifterPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleTarotBasic.Builder(),
                            DiscipleTarotPremium.Builder(),
                            DiscipleTarotPro.Builder(),
                        },
                    },
                },

                ChampionIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_ClassSelect_Disciple.png"),
                ClanSelectSfxCue = copyClan.GetClanSelectSfxCue(),

                DraftIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_CardBack_Disciple.png"),

                Icons = new List<Sprite>
                {
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_92.png"), // Clan Choice Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_silhouette.png"), // Also compendium...? 56x56
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_92.png"), // Large Coloured Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_silhouette.png"), // Compendium Silhouette 56x56
                },

                CardFrameUnit = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/unit-cardframe-disciple.png"),
                CardFrameSpell = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/spell-cardframe-disciple.png"),
                UiColor = new Color(0.964f, 0.729f, 0.015f, 1f),
                UiColorDark = new Color(0.015f, 0.015f, 0.403f, 1f),
            };

            clan.StartingChampion.championCharacterArt = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_ClassSelect_Disciple.png");
            clan.StartingChampion.storyCharacterData = copyClan.GetStartingChampionData().storyCharacterData;

            return clan.BuildAndRegister();
        }

        public static void RegisterBanner()
        {
            CardPool cardPool = UnityEngine.ScriptableObject.CreateInstance<CardPool>();
            var cardDataList = (Malee.ReorderableArray<CardData>)AccessTools.Field(typeof(CardPool), "cardDataList").GetValue(cardPool);

            // This shit needs to be automated in a loop
            foreach (var card in CustomCardManager.CustomCardData)
            {
                if (card.Value.GetLinkedClassID() == "Chrono" && card.Value.GetSpawnCharacterData() != null && !card.Value.GetSpawnCharacterData().IsChampion())
                    cardDataList.Add(card.Value);
            }

            new RewardNodeDataBuilder()
            {
                RewardNodeID = "Disciple_UnitBanner",
                MapNodePoolIDs = new List<string> { "RandomChosenMainClassUnit", "RandomChosenSubClassUnit" },
                Name = "RewardNodeData_Disciple_UnitBanner_TooltipBodyKey",
                Description = "RewardNodeData_Disciple_UnitBanner_TooltipTitleKey",
                RequiredClass = CustomClassManager.GetClassDataByID("Chrono"),
                FrozenSpritePath = "Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Frozen.png",
                EnabledSpritePath = "Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                DisabledSpritePath = "Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Disabled.png",
                DisabledVisitedSpritePath = "Disciple/chrono/Clan Assets/AllCardsBanner_Disabled_Visited.png",
                GlowSpritePath = "Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                MapIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png"),
                MinimapIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png"),
                SkipCheckInBattleMode = true,
                OverrideTooltipTitleBody = false,
                NodeSelectedSfxCue = "Node_Banner",
                RewardBuilders = new List<IRewardDataBuilder>
            {
                new DraftRewardDataBuilder()
                {
                    DraftRewardID = "Disciple_UnitsDraft",
                    _RewardSprite = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png"),
                    _RewardTitleKey = "ArcadianReward_Title",
                    _RewardDescriptionKey = "ArcadianReward_Desc",
                    Costs = new int[] { 100 },
                    _IsServiceMerchantReward = false,
                    DraftPool = cardPool,
                    ClassType = (RunState.ClassType)7,
                    DraftOptionsCount = 2,
                    RarityFloorOverride = CollectableRarity.Uncommon
                }
            }
            }.BuildAndRegister();
        }
    }
}
