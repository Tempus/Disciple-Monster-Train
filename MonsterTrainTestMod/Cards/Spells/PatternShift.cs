using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace MonsterTrainTestMod.Cards.Spells
{
    class PatternShift
    {
        private static string IDName = "Pattern Shift";

        public static void Make()
        {
            Random random = new Random();

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Description = "Teleports to a random floor./n TODO: Random *other* floor, seeded, unweighted",
                Cost = 1,
                Rarity = CollectableRarity.Starter,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = true,
                Targetless = false,

                //AssetPath = "chrono/blueeyes.png",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectTeleport).AssemblyQualifiedName,
                        ParamInt = random.Next(-2, 3),
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },
            };

            railyard.CreateAndSetCardArtPrefabVariantRef(
                "Assets/GameData/CardArt/Portrait_Prefabs/CardArt_Spell_FlashFreeze.prefab",
                "52471f4f40ea12d4a9a80a91f211fd07"
            );

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
