using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Icarian

namespace DiscipleClan.Cards.Units
{
    class SampatiOwl
    {
        public static string IDName = "Sampati Owl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Rare,
                Description = "(TODO)Icarian. Resolve: Enhance with +10 dmg, +3 health.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "15924082478465092503139501393540.jpg");

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
                Health = 3,
                AttackDamage = 20
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

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
                ParamInt = 3,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthBuilder.Build());

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
