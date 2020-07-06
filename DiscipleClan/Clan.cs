using MonsterTrainModdingAPI.Builders;
using System;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using DiscipleClan.Cards.Spells;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using DiscipleClan.Cards.Units;
using MonsterTrainModdingAPI;
using DiscipleClan.Upgrades;

namespace DiscipleClan
{
    public class MTCardPool_ChronoPool : IMTCardPool { public string ID => "Chrono"; }

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
                    upgradeTrees = new List<List<CardUpgradeDataBuilder>>
                    {
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleRetainBasic.Builder(),
                            DiscipleRetainPremium.Builder(),
                            DiscipleRetainPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleRewindBasic.Builder(),
                            DiscipleRewindPremium.Builder(),
                            DiscipleRewindPro.Builder(),
                        },
                        new List<CardUpgradeDataBuilder>
                        {
                            DiscipleShifterBasic.Builder(),
                            DiscipleShifterPremium.Builder(),
                            DiscipleShifterPro.Builder(),
                        },
                    },
                },

                ChampionIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_ClassSelect_Disciple.png"),
                ClanSelectSfxCue = copyClan.GetClanSelectSfxCue(),

                Icons = new List<Sprite>
                {
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_92.png"), // Clan Choice Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_silhouette.png"), // Also compendium...? 56x56
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_92.png"), // Large Coloured Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/ClanLogo_silhouette.png"), // Compendium Silhouette 56x56
                },

                CardFrameUnit = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/unit-cardframe-disciple.png"),
                CardFrameSpell = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/spell-cardframe-disciple.png"),
                UiColor = new Color(0.43f, 0.15f, 0.81f, 1f),
                UiColorDark = new Color(0.12f, 0.42f, 0.39f, 1f),
            };

            clan.StartingChampion.championCharacterArt = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_ClassSelect_Disciple.png");
            clan.StartingChampion.storyCharacterData = copyClan.GetStartingChampionData().storyCharacterData;

            return clan.BuildAndRegister();
        }
    }

    [HarmonyPatch(typeof(RandomMapDataContainer), "GetMapNodeBucketData")]
    class MapNodeBucketDataPatch
    {
        static void Prefix(ref RandomMapDataContainer __instance)
        {
            var mapNodeDataList = (Malee.ReorderableArray<MapNodeData>)AccessTools.Field(typeof(RandomMapDataContainer), "mapNodeDataList").GetValue(__instance);
            Debug.Log("COUNT: " + mapNodeDataList.Count + "  " + __instance.name);
            if (__instance.name == "RandomChosenMainClassUnit" || __instance.name == "RandomChosenSubClassUnit")
            {
                var rewardNode = GameObject.Instantiate(mapNodeDataList[0]);
                rewardNode.name = "RewardNodeUnitPackTest";
                AccessTools.Field(typeof(RewardNodeData), "requiredClass").SetValue(rewardNode, CustomClassManager.GetClassDataByID(Clan.IDName));
                mapNodeDataList.Add(rewardNode);
            }
            foreach (MapNodeData mapNodeData in mapNodeDataList)
            {
                Debug.Log(mapNodeData.name + " " + mapNodeData.GetID());
            }
        }
    }
}
