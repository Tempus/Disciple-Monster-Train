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

namespace DiscipleClan.Cards.Spells
{
    class TestBed
    {
        public static string IDName = "For Testing";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Common,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    //new CardEffectDataBuilder
                    //{
                    //    EffectStateName = "CardEffectBump",
                    //    ParamInt = -4,
                    //    TargetMode = TargetMode.DropTargetCharacter,
                    //    TargetTeamType = Team.Type.Monsters,
                    //}
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitSpellAffinity"
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Initiative_back.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
