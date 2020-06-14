using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

namespace MonsterTrainTestMod.Cards.Units
{
    class LashLizard
    {
        public static void Make()
        {
            private static string IDName = "Lash Lizard";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
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
                Health = 10,
                AttackDamage = 5
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // This is relocate, basically! But I think it will only work for this character
            var ascendTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostAscension};
            var descendTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostDescension};

            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectAddStatusEffect",
                TargetMode = TargetMode.Self
            };
            effectBuilder.CardEffectAddStatusEffect(MTStatusEffect.Sweep, 1);
            ascendTrigger.Effects.Add(effectBuilder.Build());
            descendTrigger.Effects.Add(effectBuilder.Build());

            // Resolve
            var resolveTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostCombat};
            var resolveBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectRemoveStatusEffect",
                TargetMode = TargetMode.Self
            };
            resolveBuilder.CardEffectAddStatusEffect(MTStatusEffect.Sweep, 1);
            resolveTrigger.Effects.Add(resolveBuilder.Build());


            characterDataBuilder.Triggers.Add(resolveTrigger.Build());
            characterDataBuilder.Triggers.Add(ascendTrigger.Build());
            characterDataBuilder.Triggers.Add(descendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
