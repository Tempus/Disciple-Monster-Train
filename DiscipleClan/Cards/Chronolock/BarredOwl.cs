using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using DiscipleClan.Triggers;

namespace DiscipleClan.Cards.Units
{
    class BarredOwl
    {
        public static string IDName = "BarredOwl";
        public static string imgName = "SpectacledOwl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "Relocate: Daze Floor.",
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, imgName + ".png");

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
                NameKey = IDName + "_Name",

                Size = 2,
                Health = 20,
                AttackDamage = 12,

                // Relocate
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder {
                        Trigger = CustomTriggerManager.GetTrigger(typeof(MTCharacterTrigger_Relocate)),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Heroes,
                            }
                        }
                    },
                }
            };

            characterDataBuilder.TriggerBuilders[0].EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Dazed), 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
