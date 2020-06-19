using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Innate, Can't target floor above/below easily? Look into target filters, also 'how to tell which floor I'm on'. Otherwise, COMPRESS EVERYTHING chronoTicked

namespace MonsterTrainTestMod.Cards.Units
{
    class Constrictor
    {
        public static string IDName = "Constrictor";
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
                Description = "(TODO)Innate. (Broken)Summon: Ascends enemies on the floor below. Descends enemies on the floor above.",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,
                AssetPath = "netstandard2.0/chrono/people-lazy-fit-tough.png",
                
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

                Size = 2,
                Health = 15,
                AttackDamage = 12,
                AssetPath = "netstandard2.0/chrono/people-lazy-fit-tough.png",
            };

            // Pulls everything that it can to this floor on summon
            var strikeTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.OnSpawn};
            var downBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = -1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(downBuilder.Build());

            var upBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = 1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(upBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());


            return characterDataBuilder.BuildAndRegister();
        }
    }
}
