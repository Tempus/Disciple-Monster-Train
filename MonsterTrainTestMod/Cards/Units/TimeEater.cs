using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

// TODO - unclear if this triggers in time, also Empowered isn't a thing yet (no way to check current energy here)
// Also, Slow is not implemented.

namespace MonsterTrainTestMod.Cards.Units
{
    class TimeEater
    {
        public static void Make()
        {
            private static string IDName = "Time Eater";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 3,
                Rarity = CollectableRarity.Rare,

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
                Health = 20,
                AttackDamage = 13
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // Drop down a floor on hit
            var strikeTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.EndTurnPreHandDiscard};
            // var resolveBuilder = new CardEffectDataBuilder
            // {
            //     EffectStateName = "CardEffectBump",
            //     ParamInt = -1,
            //     TargetMode = TargetMode.LastAttackedCharacter
            // };
            // strikeTrigger.Effects.Add(resolveBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
