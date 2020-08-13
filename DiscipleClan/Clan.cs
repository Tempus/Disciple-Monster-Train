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

                UpgradeTreeBuilder = new CardUpgradeTreeDataBuilder
                {
                    UpgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEphemeralBasic.Builder(),
                            DiscipleEphemeralPremium.Builder(),
                            DiscipleEphemeralPro.Builder(),
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
                            DiscipleWardmasterBasic.Builder(),
                            DiscipleWardmasterPremium.Builder(),
                            DiscipleWardmasterPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleEchoBasic.Builder(),
                            DiscipleEchoPremium.Builder(),
                            DiscipleEchoPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleNimbleBasic.Builder(),
                            DiscipleNimblePremium.Builder(),
                            DiscipleNimblePro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleSymbioteBasic.Builder(),
                            DiscipleSymbiotePremium.Builder(),
                            DiscipleSymbiotePro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleChainDragonBasic.Builder(),
                            DiscipleChainDragonPremium.Builder(),
                            DiscipleChainDragonPro.Builder(),
                        },
                    },
                },

                ChampionIconPath = ("chrono/Clan Assets/Icon_ClassSelect_Disciple.png"),
                ClanSelectSfxCue = copyClan.GetClanSelectSfxCue(),

                DraftIconPath = ("chrono/Clan Assets/Icon_CardBack_Disciple.png"),

                IconAssetPaths = new List<string>
                {
                    ("chrono/Clan Assets/ClanLogo_92_stroke1.png"), // Clan Choice Icon
                    ("chrono/Clan Assets/ClanLogo_92_stroke2.png"), // Also compendium...? 56x56
                    ("chrono/Clan Assets/ClanLogo_92_stroke1.png"), // Large Coloured Icon
                    ("chrono/Clan Assets/ClanLogo_silhouette.png"), // Compendium Silhouette 56x56
                },

                CardFrameUnitPath = ("chrono/Clan Assets/unit-cardframe-arcadian.png"),
                CardFrameSpellPath = ("chrono/Clan Assets/spell-cardframe-arcadian.png"),
                UiColor = new Color(0.964f, 0.729f, 0.015f, 1f),
                UiColorDark = new Color(0.12f, 0.375f, 0.5f, 1f),
            };

            clan.StartingChampion.championCharacterArt = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/Icon_ClassSelect_Disciple.png");
            clan.StartingChampion.storyCharacterData = copyClan.GetStartingChampionData().storyCharacterData;

            return clan.BuildAndRegister();
        }

        public static void RegisterBanner()
        {
            CardPool cardPool = UnityEngine.ScriptableObject.CreateInstance<CardPool>();
            var cardDataList = (Malee.ReorderableArray<CardData>)AccessTools.Field(typeof(CardPool), "cardDataList").GetValue(cardPool);

            SubtypeData wardSub;
            CustomCharacterManager.CustomSubtypeData.TryGetValue("ChronoSubtype_Ward", out wardSub);

            // This shit needs to be automated in a loop
            foreach (var card in CustomCardManager.CustomCardData)
            {
                if (card.Value.GetLinkedClassID() == "Chrono" && card.Value.GetSpawnCharacterData() != null && !card.Value.GetSpawnCharacterData().IsChampion())
                {
                    if (!card.Value.GetSpawnCharacterData().GetSubtypes()[0].Equals(wardSub))
                        cardDataList.Add(card.Value);
                }

                new RewardNodeDataBuilder()
                {
                    RewardNodeID = "Disciple_UnitBanner",
                    MapNodePoolIDs = new List<string> { "RandomChosenMainClassUnit", "RandomChosenSubClassUnit" },
                    Name = "RewardNodeData_Disciple_UnitBanner_TooltipBodyKey",
                    Description = "RewardNodeData_Disciple_UnitBanner_TooltipTitleKey",
                    RequiredClass = CustomClassManager.GetClassDataByID("Chrono"),
                    FrozenSpritePath = "chrono/Clan Assets/POI_Map_Clan_CDisciple_Frozen.png",
                    EnabledSpritePath = "chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                    DisabledSpritePath = "chrono/Clan Assets/POI_Map_Clan_CDisciple_Disabled.png",
                    DisabledVisitedSpritePath = "chrono/Clan Assets/AllCardsBanner_Disabled_Visited.png",
                    GlowSpritePath = "chrono/Clan Assets/MSK_Map_Clan_CDisciple_01.png",
                    MapIcon = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png"),
                    MinimapIcon = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/Icon_MiniMap_ClanBanner.png"),
                    SkipCheckInBattleMode = true,
                    OverrideTooltipTitleBody = false,
                    NodeSelectedSfxCue = "Node_Banner",
                    RewardBuilders = new List<IRewardDataBuilder>
                    {
                        new DraftRewardDataBuilder()
                        {
                            DraftRewardID = "Disciple_UnitsDraft",
                            _RewardSprite = CustomAssetManager.LoadSpriteFromPath("chrono/Clan Assets/POI_Map_Clan_CDisciple_Enabled.png"),
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
}
