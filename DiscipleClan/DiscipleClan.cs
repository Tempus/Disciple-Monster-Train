using BepInEx;
using DiscipleClan.Artifacts;
using DiscipleClan.CardEffects;
using DiscipleClan.Cards.Units;
using DiscipleClan.Enhancers;
using DiscipleClan.StatusEffects;
using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Interfaces;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            RegisterSubtypes();
            MakeStatuses();
            MakeEnhancers();
            MakeCards();
            Disciple.Make();
            Clan.RegisterBanner();
            MakeArtifacts();

            //PrintCardStats();
            //foreach (SubtypeData s in SubtypeManager.AllData)
            //{
            //    API.Log(BepInEx.Logging.LogLevel.All, "Subtype: " + s.LocalizedName + " - Key: " + s.Key);
            //}
        }

        static void MakeStatuses()
        {
            // Status Effects
            //var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.StatusEffects"));
            //foreach (var status in types) {
            //    if (status.Name.StartsWith("StatusEffect") ){
            //        API.Log(BepInEx.Logging.LogLevel.All, "Artifact Name: " + status.Name);
            //        Make(status); } }

            // Dunno why the above doesn't work, it works for cards

            StatusEffectPyreboost.Make();
            //StatusEffectChronolock.Make();
            StatusEffectLoaded.Make();
            StatusEffectSlow.Make();
            StatusEffectIcarian.Make();
            StatusEffectEmberboost.Make();
            StatusEffectGravity.Make();
            StatusEffectPyrelink.Make();
            StatusEffectHideUntilBoss.Make();
            StatusEffectPastGlory.Make();
            StatusEffectSymbiote.Make();
            StatusEffectAdapted.Make();
        }

        static void MakeEnhancers()
        {
            SpellUpgradePyreboost.Make();
            // UnitUpgradePyreboost.Make();
            // UnitUpgradePyrelink.Make();
            UnitUpgradeRelocate.Make();
            // UnitUpgradeSweep.Make();

            // This section below edits existing Enhancers to allow my custom cards.
            AddToSpellPowerEnhancers(typeof(CardEffectEmberwave).AssemblyQualifiedName);
            AddToSpellPowerEnhancers(typeof(CardEffectEmberwaveFibonacci).AssemblyQualifiedName);
            AddToSpellPowerEnhancers(typeof(CardEffectEmberwaveEmberDmg).AssemblyQualifiedName);
            //AddToDoublestackEnhancers(typeof(CardEffectIncreaseStatusEffects).AssemblyQualifiedName);
        }

        public static void AddToSpellPowerEnhancers(string CardEffectID)
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();
            Traverse.Create(allGameData.FindEnhancerDataByName("SpellMagicPower").GetEffects()[0].GetParamCardUpgradeData().GetFilters()[0]).Field("requiredCardEffects").GetValue<List<string>>().Add(CardEffectID);
            Traverse.Create(allGameData.FindEnhancerDataByName("SpellMagicPowerBigExtraCost").GetEffects()[0].GetParamCardUpgradeData().GetFilters()[0]).Field("requiredCardEffects").GetValue<List<string>>().Add(CardEffectID);
        }

        public static void AddToDoublestackEnhancers(string CardEffectID)
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();
            Traverse.Create(allGameData.FindEnhancerDataByName("UpgradeTraitAddJuice").GetEffects()[0].GetParamCardUpgradeData().GetFilters()[0]).Field("requiredCardEffects").GetValue<List<string>>().Add(CardEffectID);
        }

        static void MakeArtifacts()
        {
            //var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Artifacts") && !t.Name.Contains("<>"));

            //foreach (var relic in types) { API.Log(BepInEx.Logging.LogLevel.All, "Artifact Name: " + relic.Name);  Make(relic); }

            BullshitThing.Make();
            // EmberOnDivine.Make();
            GoldOverTime.Make();
            GravityOnAscend.Make();
            RewindFirstSpell.Make();
            PyreDamageOnEmber.Make();
            QuickAndDirty.Make();
            RageAgainstThePyre.Make();
            RefundXCosts.Make();
            // SeersBoostDivine.Make();
            FirstBuffExtraStack.Make();
        }

        static void MakeCards()
        {
            // Prophecy
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Prophecy") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Prophecy Cards");
            foreach (var card in types) { Make(card); }

            // Pyrepact
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Pyrepact") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Pyrepact Cards");
            foreach (var card in types) { Make(card); }

            // Retain
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Retain") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Retain Cards");
            foreach (var card in types) { Make(card); }

            // Shifter
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Shifter") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Shifter Cards");
            foreach (var card in types) { Make(card); }

            // Speedtime
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Speedtime") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Speedtime Cards");
            foreach (var card in types) { Make(card); }

            // Chronolock
            types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith("DiscipleClan.Cards.Chronolock") && !t.Name.Contains("<>"));
            API.Log(BepInEx.Logging.LogLevel.All, "Making " + types.ToList().Count + " Chronolock Cards");
            foreach (var card in types) { Make(card); }
        }

        public static void Make(Type cardType)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Making... " + cardType.Name);
            MethodInfo make = cardType.GetMethod("Make");
            make.Invoke(null, null);
        }

        public static ClassData getClan() { return clanRef; }

        public static void RegisterSubtypes()
        {
            CustomCharacterManager.RegisterSubtype("ChronoSubtype_Seer");
            CustomCharacterManager.RegisterSubtype("ChronoSubtype_Pythian");
            CustomCharacterManager.RegisterSubtype("ChronoSubtype_Eternal");
            CustomCharacterManager.RegisterSubtype("ChronoSubtype_Ward");
        }

        public static void PrintCardStats()
        {
            int totalCards = 0;
            int commons = 0;
            int uncommons = 0;
            int rares = 0;
            int units = 0;
            int spells = 0;

            foreach (var card in CustomCardManager.CustomCardData)
            {
                totalCards++;
                if (card.Value.GetRarity() == CollectableRarity.Common)
                    commons++;
                if (card.Value.GetRarity() == CollectableRarity.Uncommon)
                    uncommons++;
                if (card.Value.GetRarity() == CollectableRarity.Rare)
                    rares++;
                if (card.Value.GetCardType() == CardType.Spell)
                    spells++;
                if (card.Value.GetCardType() == CardType.Monster)
                    units++;
            }

            API.Log(BepInEx.Logging.LogLevel.All, "Total Cards: " + totalCards);
            API.Log(BepInEx.Logging.LogLevel.All, "Common Cards: " + commons);
            API.Log(BepInEx.Logging.LogLevel.All, "Uncommon Cards: " + uncommons);
            API.Log(BepInEx.Logging.LogLevel.All, "Rare Cards: " + rares);
            API.Log(BepInEx.Logging.LogLevel.All, "Units + Wards: " + units);
            API.Log(BepInEx.Logging.LogLevel.All, "Spell Cards: " + spells);
        }
    }
}
