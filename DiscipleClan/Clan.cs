using MonsterTrainModdingAPI.Builders;
using System;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MonsterTrainTestMod
{
    class Clan
    {
        public static string IDName = "Chrono";

        public static void Make()
        {
            var copyClan = CustomClassManager.SaveManager.GetAllGameData().GetAllClassDatas()[0];

            ClassDataBuilder clan = new ClassDataBuilder
            {
                ClassID = IDName,
                Name = IDName + "_Name",
                Description = IDName + "_Class",
                SubclassDescription = IDName + "_SubClass",

                UpgradeTree = copyClan.GetUpgradeTree(),

                //StartingChampion = new ChampionDataBuilder { },
                //UpgradeTree = new UpgradeTreeBuilder { },
                ChampionIcon = CustomAssetManager.LoadSpriteFromPath("MonsterTrainTestMod/testclan_large.png"),
                ClanSelectSfxCue = copyClan.GetClanSelectSfxCue(),

                Icons = new List<Sprite>
                {
                    CustomAssetManager.LoadSpriteFromPath("MonsterTrainTestMod/testclan_small.png"), // 32x32
                    CustomAssetManager.LoadSpriteFromPath("MonsterTrainTestMod/testclan_small.png"), // ??x??
                    CustomAssetManager.LoadSpriteFromPath("MonsterTrainTestMod/testclan_small.png"), // 89x89
                    CustomAssetManager.LoadSpriteFromPath("MonsterTrainTestMod/testclan_small.png"), // 43x43
                },

                CardStyle = ClassCardStyle.Umbra,
                UiColor = new Color(0.43f, 0.15f, 0.81f, 1f),
	            UiColorDark = new Color(0.12f, 0.42f, 0.39f, 1f),

            };

            clan.BuildAndRegister();
        }
    }
}
