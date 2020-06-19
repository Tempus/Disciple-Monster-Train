using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though), and Pyrebound (couldn't find it)

namespace MonsterTrainTestMod.Cards.Units
{
    class Waxwing
    {
        public static string IDName = "Waxwing";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Rare,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "(TODO)Icarian: (TODO)Pyre attacks every floor.",
                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,
                AssetPath = "netstandard2.0/chrono/flyingRat.gif",
            };

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit(),
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
                AttackDamage = 10,
                AssetPath = "netstandard2.0/chrono/flyingRat.gif",
            };

            // Resolve
            var resolveTrigger = new CharacterTriggerDataBuilder
            {
                Trigger = CharacterTriggerData.Trigger.PostCombat
            };
            
            var icarianBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = 1,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(icarianBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
