using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;

namespace DiscipleClan.Cards.Units
{
    class ChainDragon
    {
        public static string IDName = "Chain Dragon";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Rare,
                Description = "Relocate: Enhance with Multistrike 1.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "IMG_20190731_020156.png");

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
                Health = 15,
                AttackDamage = 2
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // This is relocate, basically! But I think it will only work for this character
            var ascendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostAscension};
            var descendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostDescension};

            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectAddStatusEffect",
                TargetMode = TargetMode.Self
            };
            effectBuilder.AddStatusEffect(typeof(MTStatusEffect_Multistrike), 1);

            ascendTrigger.Effects.Add(effectBuilder.Build());
            descendTrigger.Effects.Add(effectBuilder.Build());

            characterDataBuilder.Triggers.Add(ascendTrigger.Build());
            characterDataBuilder.Triggers.Add(descendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
