using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Utilities;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Retain
{
    class Newtons
    {
        public static string IDName = "Newtons";
        public static string imgName = "Coconewt";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder r = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Rare,
            };

            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();
            r.ClanID = Clan.IDName;

            r.CardPoolIDs = new List<string> { "Chrono", UnitsAllBanner };
            r.CardType = CardType.Monster;
            r.TargetsRoom = true;

            r.AssetPath = "chrono/" + "Card Assets/ProtoUnitCardArt/";
            r.EffectBuilders.Add(
                new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectSpawnMonster",
                    TargetMode = TargetMode.DropTargetCharacter,
                    ParamCharacterData = BuildUnit(),
                    ParamInt = 3,
                });

            Utils.AddImg(r, imgName + ".png");

            // Do this to complete
            r.BuildAndRegister();
        }

        // Builds the unit
        public static CharacterData BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                NameKey = IDName + "_Name",
                SubtypeKeys = new List<string> { "ChronoSubtype_Pythian" },

                Size = 1,
                Health = 1,
                AttackDamage = 1,

                BundleLoadingInfo = new BundleAssetLoadingInfo
                {
                    FilePath = "chrono/arcadian_units",
                    SpriteName = "assets/newton.png",
                    ObjectName = "assets/newton.prefab",
                    AssetType = AssetRefBuilder.AssetTypeEnum.Character
                },
            };

            characterDataBuilder.AddStartingStatusEffect("gravity", 1);

            //Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
