using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.Cards.Units
{
    class ShifterWard
    {
        public static string IDName = "ShifterWard";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Common,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    },
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
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.PostCombat,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(ShinyShoe.CardEffectTeleport).AssemblyQualifiedName,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                                TargetIgnoreBosses = true,
                            }
                        }
                    },
                }
            };
            characterDataBuilder.AddStartingStatusEffect(typeof(MTStatusEffect_Immobile), 1);

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
