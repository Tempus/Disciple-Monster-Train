using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - There's no card effect that applies upgrades onto itself. See CardEffectFreezeCard for a template for a new effect

namespace MonsterTrainTestMod.Cards.Units
{
    class FriggaOwl
    {
        private static string IDName = "Frigga Owl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "Permafrost. Reserve: (TODO)Enhance with +5 damage, +5 health.",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,
                AssetPath = "netstandard2.0/chrono/Puffling.png",

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitRetain"
                    }
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        //EffectBuilders = new List<CardEffectDataBuilder>
                        //{
                        //    new CardEffectDataBuilder
                        //    {
                        //        EffectStateName = "CardEffectBuffDamage",
                        //        ParamInt = 5,
                        //        TargetMode = TargetMode.Self
                        //    },
                        //    new CardEffectDataBuilder
                        //    {
                        //        EffectStateName = "CardEffectBuffMaxHealth",
                        //        ParamInt = 5,
                        //        TargetMode = TargetMode.Self
                        //    }
                        //}
                    }
                }
            };

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit()
            };
            railyard.Effects.Add(spawnEffectBuilder.Build());

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
                AttackDamage = 5,
                AssetPath = "netstandard2.0/chrono/Puffling.png",
            };

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
