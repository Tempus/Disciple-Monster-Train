using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

namespace MonsterTrainTestMod.Cards.Units
{
    class FortuneFinder
    {
        private static string IDName = "Fortune Finder";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "Relocate: Gain 10 Gold.",
                AssetPath = "netstandard2.0/chrono/D7SPZADW4AI6G19.png",

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

                Size = 2,
                Health = 10,
                AttackDamage = 2,
                AssetPath = "netstandard2.0/chrono/D7SPZADW4AI6G19.png",
            };

            // This is relocate, basically! But I think it will only work for this character
            var ascendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostAscension};
            var descendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostDescension};

            var damageEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectRewardGold",
                ParamInt = 10,
                TargetMode = TargetMode.Self
            };
            ascendTrigger.Effects.Add(damageEffectBuilder.Build());
            descendTrigger.Effects.Add(damageEffectBuilder.Build());

            characterDataBuilder.Triggers.Add(ascendTrigger.Build());
            characterDataBuilder.Triggers.Add(descendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
