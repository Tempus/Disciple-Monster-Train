using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

namespace DiscipleClan.Cards.Units
{
    class FortuneFinder
    {
        public static string IDName = "Fortune Finder";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,
                Description = "Relocate: Gain 10 Gold.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "D7SPZADW4AI6G19.png");

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
                AssetPath = "Disciple/chrono/Unit Assets/D7SPZADW4AI6G19.png",
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
