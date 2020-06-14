using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

// TODO - There's no way to increase size every turn, besides applying a temp upgrade. Do that?

namespace MonsterTrainTestMod.Cards.Units
{
    class JellyScholar
    {
        public static void Make()
        {
            private static string IDName = "Jelly Scholar";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 3,
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

                Size = 3,
                Health = 5,
                AttackDamage = 15
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // Resolve
            var resolveTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostCombat};
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

            var healthUpBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectHeal",
                ParamInt = 15,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthUpBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
