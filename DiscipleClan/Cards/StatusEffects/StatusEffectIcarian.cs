using System;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.Cards.StatusEffects
{
    public class MTStatusEffect_Icarian : IMTStatusEffect { public string ID => "icarian"; }

    class StatusEffectIcarian : StatusEffectState
    {
        public const string StatusId = "icarian";

        // This makes them unable to be targetted
        public override bool GetUnitIsTargetable(bool inCombat)
        {
            return !inCombat;
        }

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // At end of turn, ascend and if we try to ascend into the Pyre then we kaboom and do something.
            Ascend(inputTriggerParams.associatedCharacter, inputTriggerParams);
            return true;
        }

        public void DoPyreEffects(CharacterState target, MonsterManager monsterManager)
        {
            monsterManager.RemoveCharacter(target, true, false);
        }

        public void Ascend(CharacterState target, InputTriggerParams inputTriggerParams)
        {
            RoomManager roomManager = inputTriggerParams.roomManager;
            CombatManager combatManager = inputTriggerParams.combatManager;
            int bumpAmount = 1;
            SpawnPoint oldSpawnPoint = target.GetSpawnPoint(false);
            SpawnPoint newSpawnPoint = (SpawnPoint)null;

            // Immobile, no ascending
            if (target.HasStatusEffect("immobile")) { return; }

            // Rooted, no ascending
            if (target.HasStatusEffect("rooted"))
            {
                target.RemoveStatusEffect("rooted", false, 1, true);
                return;
            }

            newSpawnPoint = this.FindBumpSpawnPoint(target, bumpAmount, roomManager, inputTriggerParams.combatManager.GetMonsterManager());
            // target.ShowNotification(CardEffectTeleport.GetErrorMessage(bumpError), PopupNotificationUI.Source.General, (RelicState)null);

            if (newSpawnPoint != null)
            {
                oldSpawnPoint.SetCharacterState((CharacterState)null);
                newSpawnPoint.SetCharacterState(target);
                oldSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(target.GetTeamType(), -1, false, false);
                newSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(target.GetTeamType(), -1, false, false);
                target.SetSpawnPoint(newSpawnPoint, false, false, (Action)null, 0.0f);
            } else { return; }

            target.GetCharacterUI().SetHighlightVisible(false, SelectionStyle.DEFAULT);

            roomManager.AllowEnchantmentUpdates = false;
            int targetIndex = 0;

            target.ChatterClearedSignal.Dispatch(true);

            target.MoveUpDownTrain(oldSpawnPoint, targetIndex, newSpawnPoint.GetRoomOwner().GetRoomIndex(), (Action)null, true);
            roomManager.AllowEnchantmentUpdates = true;
        }

        private SpawnPoint FindBumpSpawnPoint(
          CharacterState target,
          int bumpAmount,
          RoomManager roomManager,
          MonsterManager monsterManager)
        {
            SpawnPoint spawnPoint1;
            RoomState roomOwner = (spawnPoint1 = target.GetSpawnPoint(false)).GetRoomOwner();
            if ((UnityEngine.Object)roomOwner == (UnityEngine.Object)null)
            {
                // bumpError = CardEffectTeleport.BumpError.NoRoom;
                return (SpawnPoint)null;
            }
            int roomIndex1 = roomOwner.GetRoomIndex();
            int max = roomManager.GetNumRooms() - 1;
            bumpAmount = Mathf.Clamp(bumpAmount, -max, max);
            for (int index = 1; index <= Mathf.Abs(bumpAmount); ++index)
            {
                int roomIndex2 = Mathf.Clamp(roomIndex1 + index * 1, 0, max);
                RoomState room = roomManager.GetRoom(roomIndex2);
                SpawnPoint spawnPoint2 = (SpawnPoint)null;

                if (room.GetIsPyreRoom())
                {
                    //bumpError = CardEffectTeleport.BumpError.FurnaceRoom;
                    DoPyreEffects(target, monsterManager);
                    break;
                }
                spawnPoint2 = room.GetFirstEmptyMonsterPoint();

                if (spawnPoint2 == null)
                {
                    // bumpError = CardEffectTeleport.BumpError.FullRoom;
                    break;
                }
                spawnPoint1 = spawnPoint2;
            }
            if ((spawnPoint1 != null ? spawnPoint1.GetRoomOwner().GetRoomIndex() : roomIndex1) != roomIndex1)
                return spawnPoint1;
            // bumpError = CardEffectTeleport.BumpError.SameRoom;
            return (SpawnPoint)null;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectIcarian).AssemblyQualifiedName,
                statusId = "icarian",
                displayCategory = StatusEffectData.DisplayCategory.Persistent,
                triggerStage = StatusEffectData.TriggerStage.OnPostRoomCombat,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
