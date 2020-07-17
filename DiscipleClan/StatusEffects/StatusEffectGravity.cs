using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectGravity : StatusEffectState
    {
        public const string statusId = "gravity";
        public bool shouldDie = false;

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // This makes them unable to move
            outputTriggerParams.movementSpeed = 0;

            return true;
        }

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // At end of turn, descend, and if we did Descend remove a stack of gravity.
            yield return Descend(GetAssociatedCharacter(), inputTriggerParams);
        }
        public IEnumerator Descend(CharacterState target, InputTriggerParams inputTriggerParams)
        {
            RoomManager roomManager;
            ProviderManager.TryGetProvider<RoomManager>(out roomManager);
            CombatManager combatManager = inputTriggerParams.combatManager;

            int bumpAmount = -1;
            SpawnPoint oldSpawnPoint = target.GetSpawnPoint(false);
            SpawnPoint newSpawnPoint = (SpawnPoint)null;

            // Immobile, no ascending
            if (target.HasStatusEffect("immobile")) { yield break; }

            // Rooted, no ascending
            if (target.HasStatusEffect("rooted"))
            {
                target.RemoveStatusEffect("rooted", false, 1, true);
                yield break;
            }

            newSpawnPoint = this.FindBumpSpawnPoint(target, bumpAmount, roomManager, inputTriggerParams.combatManager.GetMonsterManager());
            // target.ShowNotification(CardEffectTeleport.GetErrorMessage(bumpError), PopupNotificationUI.Source.General, (RelicState)null);

            if (newSpawnPoint != null)
            {
                if (target.IsOuterTrainBoss())
                    yield return (object)combatManager.GetHeroManager().ForceMoveBoss(target, oldSpawnPoint.GetRoomOwner(), newSpawnPoint.GetRoomOwner(), CardEffectBump.BumpDirection.Down);
                else if (!combatManager.GetSaveManager().PreviewMode)
                {
                    oldSpawnPoint.SetCharacterState((CharacterState)null);
                    newSpawnPoint.SetCharacterState(target);
                    oldSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(target.GetTeamType(), -1, false, false);
                    newSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(target.GetTeamType(), -1, false, false);
                    target.SetSpawnPoint(newSpawnPoint, false, false, (Action)null, 0.0f);
                }
            }
            else { yield break; }

            target.GetCharacterUI().SetHighlightVisible(false, SelectionStyle.DEFAULT);

            roomManager.AllowEnchantmentUpdates = false;
            int targetIndex = 0;

            target.ChatterClearedSignal.Dispatch(true);

            target.MoveUpDownTrain(newSpawnPoint, targetIndex, oldSpawnPoint.GetRoomOwner().GetRoomIndex(), (Action)null, false);
            yield return new WaitUntil((Func<bool>)(() => target.IsMovementDone));
            roomManager.AllowEnchantmentUpdates = true;
            target.RemoveStatusEffect("gravity", false, 1, true);
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
                int roomIndex2 = Mathf.Clamp(roomIndex1 + index * -1, 0, max);
                RoomState room = roomManager.GetRoom(roomIndex2);
                SpawnPoint spawnPoint2 = (SpawnPoint)null;
                if (!room.IsRoomEnabled())
                {
                    //bumpError = CardEffectTeleport.BumpError.DestroyedRoom;
                    break;
                }
                if (target.IsOuterTrainBoss())
                {
                    if (room.GetIsPyreRoom() && target.GetBossState().GetCurrentAttackPhase() != BossState.AttackPhase.Relentless)
                    {
                        //bumpError = CardEffectTeleport.BumpError.BossFurnaceRoom;
                        break;
                    }
                    spawnPoint2 = room.GetOuterTrainSpawnPoint();
                }
                else if (target.GetTeamType() == Team.Type.Heroes)
                    spawnPoint2 = room.GetFirstEmptyHeroPoint();
                else if (target.GetTeamType() == Team.Type.Monsters)
                {
                    if (room.GetIsPyreRoom())
                    {
                        //bumpError = CardEffectTeleport.BumpError.FurnaceRoom;
                        break;
                    }
                    spawnPoint2 = room.GetFirstEmptyMonsterPoint();
                }

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
                StatusEffectStateName = typeof(StatusEffectGravity).AssemblyQualifiedName,
                StatusId = "gravity",
                DisplayCategory = StatusEffectData.DisplayCategory.Positive,
                TriggerStage = StatusEffectData.TriggerStage.OnPostRoomCombat,
                IsStackable = true,
                Icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/weight.png"),
            }.Build();
        }
    }
}
