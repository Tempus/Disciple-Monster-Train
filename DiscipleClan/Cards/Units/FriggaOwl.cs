using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - There's no card effect that applies upgrades onto itself. See CardEffectFreezeCard for a template for a new effect

namespace DiscipleClan.Cards.Units
{
    class FriggaOwl
    {
        public static string IDName = "Frigga Owl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "Permafrost. Reserve: (TODO)Enhance with +5 damage, +5 health.",

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
                AttackDamage = 5,
                AssetPath = "Disciple/chrono/Unit Assets/Puffling.png",
            };

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
