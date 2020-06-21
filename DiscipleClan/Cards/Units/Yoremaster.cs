using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Slow is not implemented. Unclear if my slow will target everyone or only the other team (it should target everyone)

namespace DiscipleClan.Cards.Units
{
    class Yoremaster
    {
        public static string IDName = "Yoremaster";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "(TODO) Slow. (TODO)Units on this floor gain (TODO)slow.",
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

                Size = 2,
                Health = 22,
                AttackDamage = 5
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // characterDataBuilder.AddStartingStatusEffect(MTStatusEffect.Slow, 1);

            // Drop down a floor on hit
            var strikeTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.OnTurnBegin};
            var effectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectAddStatusEffect",
                TargetMode = TargetMode.Room
            };
            // effectBuilder.AddStatusEffect(typeof(MTStatusEffect.Slow, 1);
            strikeTrigger.Effects.Add(effectBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
