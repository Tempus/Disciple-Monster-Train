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

namespace DiscipleClan
{
    public class MTCardPool_ChronoPool : IMTCardPool { public string ID => "ChronoPool"; }

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

                UpgradeTree = copyClan.GetUpgradeTree(),
                //UpgradeTreeBuilder = new CardUpgradeTreeDataBuilder
                //{
                //    champion = CustomCardManager.GetCharacterDataByID(Disciple.IDName),
                //    upgradeTrees = new List<List<CardUpgradeDataBuilder>>
                //    {
                //        new List<CardUpgradeDataBuilder>
                //        {
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //        },
                //        new List<CardUpgradeDataBuilder>
                //        {
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //        },
                //        new List<CardUpgradeDataBuilder>
                //        {
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //            new CardUpgradeDataBuilder {},
                //        },
                //    },
                //},
                StartingChampion = (ChampionData)UnityEngine.ScriptableObject.CreateInstance("ChampionData"),

                ChampionIcon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/Icon_ClassSelect_Disciple.png"),
                ClanSelectSfxCue = copyClan.GetClanSelectSfxCue(),

                Icons = new List<Sprite>
                {
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"), // Clan Choice Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_silhouette.png"), // Also compendium...? 56x56
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_92.png"), // Large Coloured Icon
                    CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_silhouette.png"), // Compendium Silhouette 56x56
                },

                CardStyle = ClassCardStyle.Umbra,
                UiColor = new Color(0.43f, 0.15f, 0.81f, 1f),
                UiColorDark = new Color(0.12f, 0.42f, 0.39f, 1f),

                MainClassStartingCards = new List<ClassData.StartingCardOptions>
                {
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                },

                SubclassStartingCards = new List<ClassData.StartingCardOptions>
                {
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                    new ClassData.StartingCardOptions
                    {
                        cards = new List<CardData>
                        {
                            CustomCardManager.GetCardDataByID(PatternShift.IDName),
                        }
                    },
                }
            };

            clan.StartingChampion.cardData = CustomCardManager.GetCardDataByID(Disciple.IDName);
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

    [HarmonyPatch(typeof(SaveManager), "LoadClassesById")]
    class DebugPatchA
    {
        static void Prefix(ref SaveManager __instance, string mainClassId, string subClassId)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Class IDs: " + mainClassId + ", " + CustomClassManager.CustomClassData.ContainsKey(mainClassId) + " - " + subClassId);   
        }
    }

    [HarmonyPatch(typeof(SaveManager), "LoadClassAndSubclass")]
    class DebugPatchB
    {
        static void Prefix(ref SaveManager __instance, ClassData mainClass, ClassData subClass)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Class: " + mainClass.GetTitle());
            API.Log(BepInEx.Logging.LogLevel.All, "Starter Deck: " + mainClass.CreateMainClassStartingDeck().Count);
            API.Log(BepInEx.Logging.LogLevel.All, "Starter Card: " + mainClass.CreateMainClassStartingDeck()[0].GetName());
            API.Log(BepInEx.Logging.LogLevel.All, "Champion: " + mainClass.GetStartingChampionCard().GetName());
        }
    }

    [HarmonyPatch(typeof(SaveManager), "AddCardStateFromData")]
    class DebugPatchC
    {
        static void Prefix(ref SaveManager __instance, CardData cardData)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "CardState Add: " + cardData.GetName());
        }
    }

    [HarmonyPatch(typeof(SaveManager), "AddCardToDeck")]
    class DebugPatchd
    {
        static void Prefix(ref SaveManager __instance, CardData cardData)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "CardtoDeck Add: " + cardData.GetName());
        }
    }

}
