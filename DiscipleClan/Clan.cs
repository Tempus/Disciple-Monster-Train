using DiscipleClan.Upgrades;
using HarmonyLib;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan
{
    class Clan
    {
        public static string IDName = "Chrono";

        public static ClassData Make()
        {
            ClassDataBuilder clan = new ClassDataBuilder
             {
                ClassID = IDName,
                DraftIconPath = "Clan Assets/Icon_CardBack_Disciple.png",

                IconAssetPaths = new List<string>
                {
                    "Clan Assets/ClanLogo_92_stroke1.png", // Clan Choice Icon
                    "Clan Assets/ClanLogo_92_stroke2.png", // Also compendium...? 56x56
                    "Clan Assets/ClanLogo_92_stroke1.png", // Large Coloured Icon
                    "Clan Assets/ClanLogo_silhouette.png", // Compendium Silhouette 56x56
                },

                CardFrameUnitPath =  "Clan Assets/unit-cardframe-arcadian.png",
                CardFrameSpellPath = "Clan Assets/spell-cardframe-arcadian.png",

                UiColor = new Color(0.964f, 0.729f, 0.015f, 1f),
                UiColorDark = new Color(0.12f, 0.375f, 0.5f, 1f),
            };

            return clan.BuildAndRegister();
        }

        public static void RegisterBanner()
        {
            CardPool cardPool = UnityEngine.ScriptableObject.CreateInstance<CardPool>();
            var cardDataList = (Malee.ReorderableArray<CardData>)AccessTools.Field(typeof(CardPool), "cardDataList").GetValue(cardPool);

            SubtypeData wardSub;
            CustomCharacterManager.CustomSubtypeData.TryGetValue("SubtypesData_Chosen", out wardSub);

            // This shit needs to be automated in a loop
            foreach (var card in CustomCardManager.CustomCardData)
            {
                if (card.Value.GetLinkedClassID() == DiscipleClan.getClan().GetID() && card.Value.GetSpawnCharacterData() != null && !card.Value.GetSpawnCharacterData().IsChampion())
                {
                    foreach (var subtype in card.Value.GetSpawnCharacterData().GetSubtypes())
                    {
                        if (subtype.Key == "SubtypesData_Chosen")
                        {
                            cardDataList.Add(card.Value);
                            Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Unit added to Banner: " + card.Value.GetName());
                        }
                    }
                }
            }

            new RewardNodeDataBuilder()
            {
                RewardNodeID = "Disciple_UnitBanner",
                MapNodePoolIDs = new List<string> { "RandomChosenMainClassUnit", "RandomChosenSubClassUnit" },
                Name = "RewardNodeData_Disciple_UnitBanner_TooltipBodyKey",
                Description = "RewardNodeData_Disciple_UnitBanner_TooltipTitleKey",
                RequiredClass = DiscipleClan.getClan(),
                FrozenSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_Frozen.png",
                EnabledSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                EnabledVisitedSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                DisabledSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_Disabled.png",
                DisabledVisitedSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_VisitedDisabled.png",
                GlowSpritePath = "Clan Assets/MSK_Map_Clan_CDisciple_01.png",
                MapIconPath = "Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
                MinimapIconPath = "Clan Assets/Icon_MiniMap_ClanBanner.png",
                SkipCheckInBattleMode = true,
                OverrideTooltipTitleBody = false,
                NodeSelectedSfxCue = "Node_Banner",
                RewardBuilders = new List<IRewardDataBuilder>
                    {
                        new DraftRewardDataBuilder()
                        {
                            DraftRewardID = "Disciple_UnitsDraft",
                            _RewardSpritePath = "Clan Assets/POI_Map_Clan_CDisciple_Enabled.png",
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
