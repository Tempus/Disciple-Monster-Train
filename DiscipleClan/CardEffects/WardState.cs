using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    public class WardState
    {
        public string ID;
        public int floor = -1;
        public ParticleSystem wardPresentVfx;
        public string tooltipTitleKey;
        public string tooltipBodyKey;
        public Sprite wardIcon;
        public int power = 0;

        public virtual void OnAdd(int floor)
        {
            this.floor = floor;

            //!saveManager.PreviewMode && 
            if (wardPresentVfx != null)
            {
                wardPresentVfx.Play();
            }
        }

        public virtual IEnumerator OnTrigger(List<CharacterState> targets) { yield break; }
        public virtual void OnTriggerNow(List<CharacterState> targets) { }

        public List<CharacterState> GetRoomTargets(Team.Type team)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            List<CharacterState> targets = new List<CharacterState>();
            roomManager.GetRoom(this.floor).AddCharactersToList(targets, team);

            return targets;
        }

        public string GetTooltipTitle()
        {
            return tooltipTitleKey.Localize();
        }

        public string GetTooltipBody()
        {
            return tooltipBodyKey.Localize();
        }
    }
}
