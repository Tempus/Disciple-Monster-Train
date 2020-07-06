using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    public class MTStatusEffect_Gravity : IMTStatusEffect { public string ID => "gravity"; }

    class StatusEffectGravity : StatusEffectState
    {
        public const string StatusId = "gravity";
        public bool shouldDie = false;

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // At end of turn, descend, and if we did Descend remove a stack of gravity.
            bool descended = Descend(inputTriggerParams.associatedCharacter, inputTriggerParams);
            if (descended)
                inputTriggerParams.associatedCharacter.RemoveStatusEffect("gravity", false, 1, true);
            yield break;
        }

        public bool Descend(CharacterState target, InputTriggerParams inputTriggerParams)
        {
            RoomManager roomManager = GameObject.FindObjectOfType<RoomManager>() as RoomManager;
            CombatManager combatManager = inputTriggerParams.combatManager;
            int bumpAmount = -1;
            SpawnPoint oldSpawnPoint = target.GetSpawnPoint(false);
            SpawnPoint newSpawnPoint = (SpawnPoint)null;

            // Immobile, no ascending
            if (target.HasStatusEffect("immobile")) { return false; }

            // Rooted, no ascending
            if (target.HasStatusEffect("rooted"))
            {
                target.RemoveStatusEffect("rooted", false, 1, true);
                return false;
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
            } else { return false; }

            target.GetCharacterUI().SetHighlightVisible(false, SelectionStyle.DEFAULT);

            roomManager.AllowEnchantmentUpdates = false;
            int targetIndex = 0;

            target.ChatterClearedSignal.Dispatch(true);

            target.MoveUpDownTrain(oldSpawnPoint, targetIndex, newSpawnPoint.GetRoomOwner().GetRoomIndex(), (Action)null, true);
            roomManager.AllowEnchantmentUpdates = true;
            return true;
        }

        private SpawnPoint FindBumpSpawnPoint(
          CharacterState target,
          int bumpAmount,
          RoomManager roomManager,
          MonsterManager monsterManager)
        {
            SpawnPoint spawnPoint1 = target.GetSpawnPoint(false);
            int roomIndex1 = target.GetCurrentRoomIndex();
            int max = roomManager.GetNumRooms() - 1;
            bumpAmount = Mathf.Clamp(bumpAmount, -max, max);
            for (int index = 1; index <= Mathf.Abs(bumpAmount); ++index)
            {
                int roomIndex2 = Mathf.Clamp(roomIndex1 + index * 1, 0, max);
                RoomState room = roomManager.GetRoom(roomIndex2);
                SpawnPoint spawnPoint2 = (SpawnPoint)null;
                if (!room.IsRoomEnabled())
                {
                    //bumpError = CardEffectTeleport.BumpError.DestroyedRoom;
                    break;
                }
                if (room.GetIsPyreRoom())
                {
                    //bumpError = CardEffectTeleport.BumpError.FurnaceRoom;
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
                statusEffectStateName = typeof(StatusEffectGravity).AssemblyQualifiedName,
                statusId = "gravity",
                displayCategory = StatusEffectData.DisplayCategory.Positive,
                triggerStage = StatusEffectData.TriggerStage.OnPostAttacking,
                isStackable = true,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
