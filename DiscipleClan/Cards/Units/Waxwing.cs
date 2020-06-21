using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though), and Pyrebound (couldn't find it)

namespace DiscipleClan.Cards.Units
{
    class Waxwing
    {
        public static string IDName = "Waxwing";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Rare,
                Description = "(TODO)Icarian: (TODO)Pyre attacks every floor.",
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
                Health = 5,
                AttackDamage = 10,
                AssetPath = "Disciple/chrono/Unit Assets/flyingRat.gif",
            };

            // Resolve
            var resolveTrigger = new CharacterTriggerDataBuilder
            {
                Trigger = CharacterTriggerData.Trigger.PostCombat
            };
            
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
