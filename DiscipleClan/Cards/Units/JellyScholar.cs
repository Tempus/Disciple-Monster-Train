using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - There's no way to increase size every turn, besides applying a temp upgrade. Do that?

namespace DiscipleClan.Cards.Units
{
    class JellyScholar
    {
        public static string IDName = "Jelly Scholar";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Uncommon,
                Description = "Resolve: Enhance with +10 dmg, +15 health, +1 capacity.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "Tima.png");

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
                AttackDamage = 15,
                AssetPath = "Disciple/chrono/Unit Assets/Tima.jpg",
            };

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
                ParamInt = 15,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
