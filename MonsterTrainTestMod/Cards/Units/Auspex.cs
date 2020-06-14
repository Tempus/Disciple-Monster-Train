using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

// TODO Unclear about rearrangement indices here

namespace MonsterTrainTestMod.Cards.Units
{
    class Auspex
    {
        public static void Make()
        {
            private static string IDName = "Auspex";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false
            };

            // Art Prefab, we can probably instantiate this ourselves later
            railyard.CreateAndSetCardArtPrefabVariantRef(
                "Assets/GameData/CardArt/Portrait_Prefabs/CardArt_TrainSteward.prefab",
                "a21c55c24d2e5d645a01230d874e26a9"
            );

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit()
            };
            railyard.Effects.Add(spawnEffectBuilder.Build());


            // Putting it in card pools... I feel like there's a better place for this
            railyard.SetCardClan(MTClan.Awoken);
            railyard.AddToCardPool(MTCardPool.AwokenBannerPool);

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
                Health = 40,
                AttackDamage = 5
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // This is relocate, basically! But I think it will only work for this character
            var ascendTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.OnHit};

            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectFloorRearrange",
                ParamInt = 0,
                TargetMode = TargetMode.Self
            };
            ascendTrigger.Effects.Add(effectBuilder.Build());
            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectDamage",
                ParamInt = 5,
                TargetMode = TargetMode.LastAttackerCharacter
            };
            ascendTrigger.Effects.Add(effectBuilder.Build());

            // Resolve
            var resolveTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostCombat};
            var resolveBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectFloorRearrange",
                ParamInt = 99,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(resolveBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());
            characterDataBuilder.Triggers.Add(ascendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
