using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;

// TODO - Chronolocked doesn't exist yet... we could fake it with Stealth, Daze, Rooted though

namespace DiscipleClan.Cards.Units
{
    class IdunOwl
    {
        public static string IDName = "Idun Owl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "Strike: Apply (Consolidate?)Chronolock.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "Puffling.png");

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
                AttackDamage = 1
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // Drop down a floor on hit
            var strikeTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.OnAttacking};
            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectAddStatusEffect",
                TargetMode = TargetMode.LastAttackedCharacter
            };
            effectBuilder.AddStatusEffect(typeof(MTStatusEffect_Stealth), 1);
            effectBuilder.AddStatusEffect(typeof(MTStatusEffect_Rooted), 1);
            effectBuilder.AddStatusEffect(typeof(MTStatusEffect_Dazed), 1);

            strikeTrigger.Effects.Add(effectBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
