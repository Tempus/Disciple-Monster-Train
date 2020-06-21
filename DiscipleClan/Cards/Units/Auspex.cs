using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO Unclear about rearrangement indices here

namespace DiscipleClan.Cards.Units
{
    class Auspex
    {
            public static string IDName = "Auspex";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon, 
                Description = "Resolve: Move to the Front. Revenge: Attack, and move to the back.",
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
            var ascendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.OnHit};

            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectFloorRearrange",
                ParamInt = -99,
                TargetMode = TargetMode.Self
            };
            ascendTrigger.Effects.Add(effectBuilder.Build());
            var effectBuilderB = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectDamage",
                ParamInt = 5,
                TargetMode = TargetMode.LastAttackerCharacter
            };
            ascendTrigger.Effects.Add(effectBuilderB.Build());

            // Resolve
            var resolveTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostCombat};
            var resolveBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectFloorRearrange",
                ParamInt = 1,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(resolveBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());
            characterDataBuilder.Triggers.Add(ascendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
