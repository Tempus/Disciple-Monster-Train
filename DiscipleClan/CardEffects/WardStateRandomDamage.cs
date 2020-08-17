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
    class WardStateRandomDamage : WardState
    {
        public WardStateRandomDamage() 
        {
            ID = "RandomDamage";
            tooltipTitleKey = "SpikeWard_Name";
            tooltipBodyKey = "SpikeWard_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "chrono/Unit Assets/SpikeWard.png"));
        }

        public override IEnumerator OnTrigger(List<CharacterState> targets)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
            if (targets.Count > 0)
                yield return ProviderManager.CombatManager.ApplyDamageToTarget(power, targets[RandomManager.Range(0, targets.Count, RngId.Battle)], new CombatManager.ApplyDamageToTargetParameters());
        }
    }

    [HarmonyPatch(typeof(CombatManager), "DoUnitCombat")]
    class SpikeWardTrigger
    {
        static IEnumerator Postfix(IEnumerator __result, CombatManager __instance, RoomState room)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            List<CharacterState> targets = new List<CharacterState>();
            room.AddCharactersToList(targets, Team.Type.Heroes);

            yield return WardManager.TriggerWards("RandomDamage", room.GetRoomIndex(), targets);
            yield return __result;
        }
    }
}
