using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Innate, Can't target floor above/below easily? Look into target filters, also 'how to tell which floor I'm on'. Otherwise, COMPRESS EVERYTHING chronoTicked

namespace DiscipleClan.Cards.Units
{
    class Constrictor
    {
        public static string IDName = "Constrictor";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "(TODO)Innate. (Broken)Summon: Ascends enemies on the floor below. Descends enemies on the floor above.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "people-lazy-fit-tough.png");

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
                Health = 15,
                AttackDamage = 12,
                AssetPath = "Disciple/chrono/Unit Assets/people-lazy-fit-tough.png",
            };

            // Pulls everything that it can to this floor on summon
            var strikeTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.OnSpawn};
            var downBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = -1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(downBuilder.Build());

            var upBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = 1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(upBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());


            return characterDataBuilder.BuildAndRegister();
        }
    }
}
