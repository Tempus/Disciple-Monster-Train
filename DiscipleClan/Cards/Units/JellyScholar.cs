using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - There's no way to increase size every turn, besides applying a temp upgrade. Do that?

namespace MonsterTrainTestMod.Cards.Units
{
    class JellyScholar
    {
        public static string IDName = "Jelly Scholar";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 3,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "Resolve: Enhance with +10 dmg, +15 health, +1 capacity.",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,
                AssetPath = "netstandard2.0/chrono/Tima.jpg",

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

                Size = 3,
                Health = 5,
                AttackDamage = 15,
                AssetPath = "netstandard2.0/chrono/Tima.jpg",
            };

            // Resolve
            var resolveTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostCombat};
            var resolveBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBuffDamage",
                ParamInt = 10,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(resolveBuilder.Build());

            var healthBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBuffMaxHealth",
                ParamInt = 15,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
