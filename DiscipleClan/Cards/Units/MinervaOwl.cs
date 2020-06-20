using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;

// TODO Pyreboost not implemented

namespace DiscipleClan.Cards.Units
{
    class MinervaOwl
    {
        public static string IDName = "Minerva Owl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "(TODO)Pyreboost.",
                AssetPath = "netstandard2.0/chrono/15924082478465092503139501393540.jpg",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false
            };

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit()
            };
            railyard.Effects.Add(spawnEffectBuilder.Build());

            // Do this to complete
            railyard.BuildAndRegister();
        }

        // Builds the unit
        public static CharacterData BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                Name = IDName,

                Size = 1,
                Health = 5,
                AttackDamage = 0,

                AssetPath = "netstandard2.0/chrono/15924082478465092503139501393540.jpg",
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.AddStartingStatusEffect(typeof(MTStatusEffect_Sweep), 1);

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
