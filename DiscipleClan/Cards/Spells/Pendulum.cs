using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Spells
{
    class Pendulum
    {
        public static string IDName = "Pendulum";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Rare,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectTransferAllStatusEffects",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
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

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterface("IMTStatusEffect", true) != null);
            foreach (Type status in types) {
                API.Log(BepInEx.Logging.LogLevel.All, "Adding Status to Pendulum: " + status.Name);
                railyard.EffectBuilders[0].AddStatusEffect(status, 1);
            }
            
            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Good_art.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
