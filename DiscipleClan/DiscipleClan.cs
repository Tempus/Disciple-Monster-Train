using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Harmony;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ShinyShoe;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Interfaces;
using MonsterTrainModdingAPI.Managers;
using DiscipleClan;
using System.Linq;
using DiscipleClan.Cards.Units;
using DiscipleClan.Cards.Spells;
using DiscipleClan.StatusEffects;

namespace DiscipleClan
{
    // Credit to Rawsome, Stable Infery for the base of this method.
    [BepInPlugin("ca.chronometry.disciple", "Disciple Clan", "0.1")]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    [BepInDependency("api.modding.train.monster")]
    public class DiscipleClan : BaseUnityPlugin, IInitializable
    {
        public static ClassData clanRef;

        void Awake()
        {
            var harmony = new Harmony("ca.chronometry.disciple");
            harmony.PatchAll();
        }

        public void Initialize()
        {
            clanRef = Clan.Make();
            MakeStatuses();
            MakeCards();

            //foreach (SubtypeData s in SubtypeManager.AllData)
            //{
            //    API.Log(BepInEx.Logging.LogLevel.All, "Subtype: " + s.LocalizedName + " - Key: " + s.Key);
            //}
        }

        static void MakeStatuses()
        {
            // Status Effects
            //var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.StatusEffects"));
            //foreach (var status in types) { Make(status); }
            // Dunno why the above doesn't work
            StatusEffectPyreboost.Make();
            StatusEffectChronolock.Make();
            StatusEffectLoaded.Make();
            StatusEffectSlow.Make();
            StatusEffectIcarian.Make();
        }

        static void MakeCards()
        {
            // Units
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Units"));
            foreach (var card in types) { Make(card); }

            // Spells
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Spells") && !t.Name.Contains("<>"));
            foreach (var card in types) { Make(card); }
        }

        public static void Make(Type cardType)
        {
            MethodInfo make = cardType.GetMethod("Make");
            make.Invoke(null, null);
        }

        public static ClassData getClan() { return clanRef; }
    }
}
