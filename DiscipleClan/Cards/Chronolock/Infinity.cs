using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Chronolock
{
    class Infinity
    {
        public static string IDName = "Infinity";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Relentless), 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "zyzzy.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
