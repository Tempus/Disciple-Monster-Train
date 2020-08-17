using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace DiscipleClan.CardEffects
{
    class WardStateShifter : WardState
    {
        public WardStateShifter() 
        {
            ID = "Shifter";
            tooltipTitleKey = "ShifterWardBeta_Name";
            tooltipBodyKey = "ShifterWardBeta_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "chrono/Unit Assets/ShifterWard.png"));
        }

        public override IEnumerator OnTrigger(List<CharacterState> targets)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            var cardEffectParams = new CardEffectParams
            {
                saveManager = ProviderManager.CombatManager.GetSaveManager(),
                combatManager = ProviderManager.CombatManager,
                heroManager = ProviderManager.CombatManager.GetHeroManager(),
                roomManager = roomManager,
                targets = targets,
            };
            var bumper = new CardEffectBump();
            yield return bumper.Bump(cardEffectParams, -1);
        }
    }

    [HarmonyPatch(typeof(CombatManager), "DoUnitPostCombat")]
    class ShifterWardTrigger
    {
        static IEnumerator Postfix(IEnumerator __result, CombatManager __instance, RoomState room)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            List<CharacterState> targets = new List<CharacterState>();
            room.AddCharactersToList(targets, Team.Type.Monsters);

            yield return WardManager.TriggerWards("Shifter", room.GetRoomIndex(), targets);
            yield return __result;
        }
    }
}
