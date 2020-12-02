using Trainworks.Managers;
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
    class WardStatePyreHarvest : WardState
    {
        public WardStatePyreHarvest() 
        {
            ID = "PyreHarvest";
            tooltipTitleKey = "HaruspexWardBeta_Name";
            tooltipBodyKey = "HaruspexWardBeta_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "Unit Assets/HaruspexWard.png"));
        }

        public override IEnumerator OnTrigger(List<CharacterState> targets)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
            roomManager.GetPyreRoom().GetPyreHeart().BuffDamage(power);

            ProviderManager.TryGetProvider<RelicManager>(out RelicManager relicManager);
            int pyredamage = roomManager.GetPyreRoom().GetPyreHeart().GetAttackDamage();
            ProviderManager.SaveManager.pyreAttackChangedSignal.Dispatch(pyredamage, 1 + ((relicManager != null) ? relicManager.GetPyreStatusEffectCount("multistrike") : 0));

            yield break;
        }
    }

    [HarmonyPatch(typeof(CharacterState), "CheckDeathCardTriggers")]
    class PyreHarvestWardTrigger
    {
        static IEnumerator Postfix(IEnumerator __result, CharacterState __instance, CardState damageSourceCard, CharacterState attacker)
        {
            if (!__instance.IsDead || ProviderManager.SaveManager.PreviewMode)
            {
                yield break;
            }
            if (__instance.GetSpawnPoint() != null && __instance.GetSpawnPoint().GetRoomOwner() != null)
            {
                yield return WardManager.TriggerWards("PyreHarvest", __instance.GetCurrentRoomIndex());
            }
            yield return __result;
        }
    }
}
