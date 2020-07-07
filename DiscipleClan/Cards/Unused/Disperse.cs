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

namespace DiscipleClan.Cards.Unused
{
    class Disperse
    {
        public static string IDName = "Disperse";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Common,

                TargetsRoom = true,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectTeleport).AssemblyQualifiedName,
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    },
                },
            };
            
            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "zoidberg.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
