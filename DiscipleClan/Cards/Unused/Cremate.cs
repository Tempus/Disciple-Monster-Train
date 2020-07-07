using System;
using System.Collections.Generic;
using System.Text;
using DiscipleClan.CardEffects;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Unused
{
    class Cremate
    {
        public static string IDName = "Cremate";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Rare,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectPyreAttack).AssemblyQualifiedName,
                        TargetMode = TargetMode.FrontInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectPyreAttack).AssemblyQualifiedName,
                        TargetMode = TargetMode.FrontInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        TargetMode = TargetMode.Pyre,
                        TargetIgnorePyre = false,
                        ParamInt = 10,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    },
                },
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitMagneticState",
                    },
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "sigmaligma.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
