using System;
using System.Collections.Generic;
using System.Text;
using DiscipleClan.Cards.CardEffects;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.Cards.Units
{
    class StasisWard
    {
        public static string IDName = "StasisWard";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Rare,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    }
                }
            };

            Utils.AddWard(railyard, IDName, BuildUnit());
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

                Size = 0,
                Health = 1,
                AttackDamage = 0,
                AssetPath = "Disciple/chrono/Unit Assets/IMG_20190731_020156.png",
                RoomModifierBuilders = new List<RoomModifierDataBuilder>
                {
                    new RoomModifierDataBuilder
                    {
                        roomStateModifierClassName = typeof(RoomStateModifierRelocateStatusEffect).AssemblyQualifiedName,
                        paramStatusEffects = new StatusEffectStackData[]
                        {
                            new StatusEffectStackData { count = 1, statusId = "dazed"},
                        }
                    }
                }
            };

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
