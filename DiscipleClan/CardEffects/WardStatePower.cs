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
    class WardStatePower : WardState
    {
        public WardStatePower() 
        {
            ID = "Power";
            tooltipTitleKey = "PowerWardBeta_Name";
            tooltipBodyKey = "PowerWardBeta_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "chrono/Unit Assets/PowerWard.png"));
        }

        public override void OnTriggerNow(List<CharacterState> targets)
        {
            foreach (var unit in targets)
            {
                if (unit.GetTeamType() == Team.Type.Monsters)
                    unit.BuffDamage(power);
            }
        }
    }
}
